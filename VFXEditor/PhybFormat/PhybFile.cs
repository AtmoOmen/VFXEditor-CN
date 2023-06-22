using ImGuiNET;
using OtterGui.Raii;
using System.IO;
using VfxEditor.FileManager;
using VfxEditor.Parsing;
using VfxEditor.Parsing.Int;
using VfxEditor.PhybFormat.Collision;
using VfxEditor.PhybFormat.Simulator;
using VfxEditor.PhybFormat.Utils;
using VfxEditor.Utils;

namespace VfxEditor.PhybFormat {
    public class PhybFile : FileManagerFile {
        public readonly ParsedIntByte4 Version = new( "Version" );
        public readonly ParsedUInt DataType = new( "Data Type" );

        public readonly PhybCollision Collision;
        public readonly PhybSimulation Simulation;

        public PhybFile( BinaryReader reader, bool checkOriginal = true ) : base( new( Plugin.PhybManager ) ) {
            var original = checkOriginal ? FileUtils.GetOriginal( reader ) : null;

            Version.Read( reader );

            if( Version.Value > 0 ) {
                DataType.Read( reader );
            }

            var collisionOffset = reader.ReadUInt32();
            var simOffset = reader.ReadUInt32();

            reader.BaseStream.Seek( collisionOffset, SeekOrigin.Begin );
            Collision = new( this, reader, collisionOffset == simOffset );

            reader.BaseStream.Seek( simOffset, SeekOrigin.Begin );
            Simulation = new( this, reader, simOffset == reader.BaseStream.Length );

            if( checkOriginal ) Verified = FileUtils.CompareFiles( original, ToBytes(), out var _ );
        }

        public override void Write( BinaryWriter writer ) {
            writer.BaseStream.Seek( 0, SeekOrigin.Begin );

            Version.Write( writer );

            if( Version.Value > 0 ) {
                DataType.Write( writer );
            }

            var offsetPos = writer.BaseStream.Position; // coming back here later
            writer.Write( 0 ); // placeholders
            writer.Write( 0 );

            if( Version.Value == 0 ) return;

            var collisionOffset = writer.BaseStream.Position;
            Collision.Write( writer );

            var simOffset = writer.BaseStream.Position;
            var simWriter = new SimulationWriter();
            Simulation.Write( simWriter );
            simWriter.WriteTo( writer );

            writer.BaseStream.Seek( offsetPos, SeekOrigin.Begin );
            writer.Write( ( int )collisionOffset );
            writer.Write( ( int )simOffset );
        }

        public override void Draw() {
            ImGui.Separator();
            ImGui.SetCursorPosY( ImGui.GetCursorPosY() + 2 );

            Version.Draw( CommandManager.Phyb );
            DataType.Draw( CommandManager.Phyb );

            ImGui.SetCursorPosY( ImGui.GetCursorPosY() + 3 );

            using var tabBar = ImRaii.TabBar( "Tabs", ImGuiTabBarFlags.NoCloseWithMiddleMouseButton );
            if( !tabBar ) return;

            using( var tab = ImRaii.TabItem( "Collision" ) ) {
                if( tab ) Collision.Draw();
            }

            using( var tab = ImRaii.TabItem( "Simulation" ) ) {
                if( tab ) Simulation.Draw();
            }
        }
    }
}