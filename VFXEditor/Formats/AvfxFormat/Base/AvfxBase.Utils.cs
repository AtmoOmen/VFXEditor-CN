﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace VfxEditor.AvfxFormat {
#pragma warning disable CS0675
    public unsafe partial class AvfxBase {
        public static int CalculatePadding( int size ) => size % 4 == 0 ? 0 : 4 - size % 4;

        public static void Peek( BinaryReader reader, AvfxBase item, int size ) => Peek( reader, [item], size );

        public static void Peek( BinaryReader reader, List<AvfxBase> items, int size ) {
            var startPosition = reader.BaseStream.Position;
            ReadNested( reader, items, size );
            reader.BaseStream.Position = startPosition; // reset position
        }

        public static void WriteNested<T>( BinaryWriter writer, List<T> children ) where T : AvfxBase => children.ForEach( child => child.Write( writer ) );

        public static void ReadNested<T>( BinaryReader reader, List<T> children, int size ) where T : AvfxBase => ReadNested( reader, ( _reader, _name, _size ) => {
            foreach( var child in children ) {
                if( child.GetAvfxName() == _name ) {
                    child.Read( _reader, _size );
                    break;
                }
            }
        }, size );

        public static void ReadNested( BinaryReader reader, Action<BinaryReader, string, int> onRead, int size ) {
            var bytesRead = 0;
            while( bytesRead < size ) {
                var avfxName = ReadAvfxName( reader );
                bytesRead += 4;

                var contentSize = reader.ReadInt32();
                bytesRead += 4;

                var finalPosition = reader.BaseStream.Position + contentSize;

                onRead( reader, avfxName, contentSize );

                reader.BaseStream.Position = finalPosition;
                bytesRead += contentSize;

                var padding = CalculatePadding( contentSize );
                reader.ReadBytes( padding );
                bytesRead += padding;
            }
        }

        public static void WriteLeaf( BinaryWriter writer, string avfxName, int size, int value ) {
            WriteAvfxName( writer, avfxName );
            writer.Write( size );
            writer.Write( value );
        }

        public static void WriteAvfxName( BinaryWriter writer, string avfxName ) {
            var reversed = new List<byte>( Encoding.ASCII.GetBytes( avfxName ) );
            reversed.Reverse();

            var nameBytes = reversed.ToArray();
            writer.Write( nameBytes );
            WritePad( writer, 4 - avfxName.Length );
        }

        public static string ReadAvfxName( BinaryReader reader ) => ReadAvfxName( BitConverter.GetBytes( reader.ReadInt32() ) );

        public static string ReadAvfxName( byte[] bytes ) {
            var reversed = new List<byte>( bytes );
            reversed.Reverse();

            var nonZeroBytes = reversed.Where( x => x != 0 ).ToArray();
            return Encoding.ASCII.GetString( nonZeroBytes );
        }

        public static void WritePad( BinaryWriter writer, int count ) {
            for( var i = 0; i < count; i++ ) writer.Write( ( byte )0 );
        }

        public static byte[] HalfToBytes( float val ) => BitConverter.GetBytes( Pack( val ) );

        public static float BytesToHalf( byte[] bytes ) => Unpack( bytes, 0 );

        public static unsafe ushort Pack( float value ) {
            var num5 = *( uint* )&value;
            var num3 = ( uint )( ( num5 & -2147483648 ) >> 0x10 );
            var num = num5 & 0x7fffffff;
            if( num > 0x47ffefff ) {
                return ( ushort )( num3 | 0x7fff );
            }
            if( num >= 0x38800000 ) return ( ushort )( num3 | num + -939524096 + 0xfff + ( num >> 13 & 1 ) >> 13 );

            var num6 = num & 0x7fffff | 0x800000;
            var num4 = 0x71 - ( int )( num >> 0x17 );
            num = num4 > 0x1f ? 0 : num6 >> num4;
            return ( ushort )( num3 | num + 0xfff + ( num >> 13 & 1 ) >> 13 );
        }

        public static float Unpack( byte[] buffer, int offset ) => Unpack( BitConverter.ToUInt16( buffer, offset ) );

        public static unsafe float Unpack( ushort value ) {
            uint num3;
            if( ( value & -33792 ) == 0 ) {
                if( ( value & 0x3ff ) != 0 ) {
                    var num2 = 0xfffffff2;
                    var num = ( uint )( value & 0x3ff );
                    while( ( num & 0x400 ) == 0 ) {
                        num2--;
                        num <<= 1;
                    }
                    num &= 0xfffffbff;
                    num3 = ( uint )( ( value & 0x8000 ) << 0x10 | num2 + 0x7f << 0x17 ) | num << 13;
                }
                else {
                    num3 = ( uint )( ( value & 0x8000 ) << 0x10 );
                }
            }
            else {
                num3 =
                    ( uint )
                    ( ( value & 0x8000 ) << 0x10 | ( value >> 10 & 0x1f ) - 15 + 0x7f << 0x17
                     | ( value & 0x3ff ) << 13 );
            }
            return *( float* )&num3;
        }
    }
}
