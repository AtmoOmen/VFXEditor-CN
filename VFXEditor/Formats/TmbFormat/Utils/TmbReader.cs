﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Numerics;
using VfxEditor.Parsing.Utils;
using VfxEditor.TmbFormat.Actor;
using VfxEditor.TmbFormat.Entries;
using VfxEditor.TmbFormat.Tmfcs;
using VfxEditor.Utils;

namespace VfxEditor.TmbFormat.Utils {
    public class TmbReader : ParsingReader {
        public long StartPosition;
        private readonly Dictionary<int, TmbItemWithId> ItemsWithId = [];

        public TmbReader( BinaryReader reader ) : base( reader ) { }

        public void UpdateStartPosition() {
            StartPosition = Reader.BaseStream.Position;
        }

        public void ParseItem( TmbFile file, List<Tmac> actors, List<Tmtr> tracks, List<TmbEntry> entries, List<Tmfc> tmfcs, ref VerifiedStatus verified ) {
            var savePos = Reader.BaseStream.Position;
            var magic = ReadString( 4 );
            var size = ReadInt32();
            Reader.BaseStream.Position = savePos;

            TmbItem entry;

            if( magic == "TMAC" ) {
                var actor = new Tmac( file, this );
                actors.Add( actor );
                entry = actor;
            }
            else if( magic == "TMTR" ) {
                var track = new Tmtr( file, this );
                tracks.Add( track );
                entry = track;
            }
            else if( magic == "TMFC" ) {
                var tmfc = new Tmfc( file, this );
                entries.Add( tmfc );
                tmfcs.Add( tmfc );
                entry = tmfc;
            }
            else {
                if( !TmbUtils.ItemTypes.TryGetValue( magic, out var value ) ) {
                    Dalamud.Log( $"未知条目 {magic}" );
                    verified = VerifiedStatus.ERROR;
                    Reader.ReadBytes( size ); //  skip
                    return;
                }

                var type = value.Type;
                var constructor = type.GetConstructor( [typeof( TmbFile ), typeof( TmbReader )] );
                if( constructor == null ) {
                    Dalamud.Log( $"TmbReader 构造函数在 {magic} 处发生错误" );
                    verified = VerifiedStatus.ERROR;
                    Reader.ReadBytes( size );
                    return;
                }

                var item = constructor.Invoke( [file, this] );
                if( item == null ) {
                    Dalamud.Log( $"构造函数由于 {magic} 发生错误" );
                    verified = VerifiedStatus.ERROR;
                    Reader.ReadBytes( size );
                    return;
                }

                entry = ( TmbItem )item;
                entries.Add( ( TmbEntry )item );
            }

            if( entry is TmbItemWithId entryId ) ItemsWithId.Add( entryId.Id, entryId );
        }

        public List<T> Pick<T>( List<int> ids ) where T : TmbItemWithId {
            var ret = new List<T>();
            foreach( var id in ids ) {
                if( !ItemsWithId.TryGetValue( id, out var item ) ) continue;
                if( item == null ) continue;
                if( item is T pickedItem ) ret.Add( pickedItem );
            }
            return ret;
        }

        public bool ReadAtOffset( Action<BinaryReader> func ) => ReadAtOffset( Reader.ReadInt32(), func );

        public bool ReadAtOffset( int offset, Action<BinaryReader> func ) {
            if( offset == 0 ) return false; // nothing to read
            var savePos = Reader.BaseStream.Position;
            Reader.BaseStream.Position = StartPosition + 8 + offset;
            func( Reader );
            Reader.BaseStream.Position = savePos;
            return true;
        }

        public string ReadOffsetString() {
            var offset = Reader.ReadInt32();
            var savePos = Reader.BaseStream.Position;
            Reader.BaseStream.Position = StartPosition + 8 + offset;
            var res = FileUtils.ReadString( Reader );
            Reader.BaseStream.Position = savePos;
            return res;
        }

        public List<int> ReadOffsetTimeline() {
            var offset = Reader.ReadInt32();
            var count = Reader.ReadInt32();
            var savePos = Reader.BaseStream.Position;
            Reader.BaseStream.Position = StartPosition + 8 + offset;
            var res = new List<int>();
            for( var i = 0; i < count; i++ ) {
                res.Add( Reader.ReadInt16() );
            }
            Reader.BaseStream.Position = savePos;
            return res;
        }

        public Vector3 ReadOffsetVector3() {
            var offset = Reader.ReadInt32();
            var count = Reader.ReadInt32();

            if( count != 3 ) return new Vector3( 0 );

            var currentPos = Reader.BaseStream.Position;
            Reader.BaseStream.Position = offset + StartPosition + 8;

            var result = new Vector3() {
                X = Reader.ReadSingle(),
                Y = Reader.ReadSingle(),
                Z = Reader.ReadSingle(),
            };

            Reader.BaseStream.Position = currentPos;
            return result;
        }

        public Vector4 ReadOffsetVector4() {
            var offset = Reader.ReadInt32();
            var count = Reader.ReadInt32();

            if( count != 4 ) return new Vector4( 0 );

            var currentPos = Reader.BaseStream.Position;
            Reader.BaseStream.Position = offset + StartPosition + 8;

            var result = new Vector4() {
                X = Reader.ReadSingle(),
                Y = Reader.ReadSingle(),
                Z = Reader.ReadSingle(),
                W = Reader.ReadSingle(),
            };

            Reader.BaseStream.Position = currentPos;
            return result;
        }

        public static void Import( TmbFile file, byte[] data, out List<Tmac> actors, out List<Tmtr> tracks, out List<TmbEntry> entries, out List<Tmfc> tmfcs, bool hasCount ) {
            using var ms = new MemoryStream( data );
            using var reader = new BinaryReader( ms );
            var tmbReader = new TmbReader( reader );

            var verified = VerifiedStatus.VERIFIED;
            actors = [];
            tracks = [];
            entries = [];
            tmfcs = [];

            var count = hasCount ? reader.ReadInt32() : 1;
            for( var i = 0; i < count; i++ ) {
                tmbReader.ParseItem( file, actors, tracks, entries, tmfcs, ref verified );
            }
        }
    }
}
