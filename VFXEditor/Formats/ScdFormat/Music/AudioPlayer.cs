﻿using Dalamud.Interface;
using Dalamud.Interface.Utility.Raii;
using ImGuiNET;
using NAudio.Wave;
using System;
using System.IO;
using System.Numerics;
using VfxEditor.FileBrowser;
using VfxEditor.ScdFormat.Music.Data;
using VfxEditor.Utils;

namespace VfxEditor.ScdFormat {
    public class AudioPlayer {
        private readonly ScdAudioEntry Entry;
        private PlaybackState State => CurrentOutput == null ? PlaybackState.Stopped : CurrentOutput.PlaybackState;
        private PlaybackState PrevState = PlaybackState.Stopped;

        private WaveStream LeftStream;
        private WaveStream RightStream;
        private IWaveProvider Volume;
        private MultiplexingWaveProvider LeftRightCombined;
        private WasapiOut CurrentOutput;

        private double TotalTime => LeftStream?.TotalTime == null ? 0 : LeftStream.TotalTime.TotalSeconds - 0.01f;
        private double CurrentTime => LeftStream?.CurrentTime == null ? 0 : LeftStream.CurrentTime.TotalSeconds;

        private bool IsVorbis => Entry.Format == SscfWaveFormat.Vorbis;
        private bool LoopSound => ( IsVorbis && Plugin.Configuration.LoopMusic ) || ( !IsVorbis && Plugin.Configuration.LoopSoundEffects );

        private double QueueSeek = -1;

        private bool ShowChannelSelect => Entry.NumChannels > 2;
        private int Channel1 = 0;
        private int Channel2 = 1;

        public AudioPlayer( ScdAudioEntry entry ) {
            Entry = entry;
        }

        public void Draw() {
            using var tabBar = ImRaii.TabBar( "栏" );
            if( !tabBar ) return;
            DrawPlayer();
            DrawChannels();
            ProcessQueue(); // Loop, etc.
            Entry.DrawTabs();
        }

        private void DrawControls() {
            using( var style = ImRaii.PushStyle( ImGuiStyleVar.FramePadding, ImGui.GetStyle().FramePadding with { X = 4 } ) )
            using( var font = ImRaii.PushFont( UiBuilder.IconFont ) ) {
                if( State == PlaybackState.Stopped ) {
                    if( ImGui.Button( FontAwesomeIcon.Play.ToIconString() ) ) Play();
                }
                else if( State == PlaybackState.Playing ) {
                    if( ImGui.Button( FontAwesomeIcon.Pause.ToIconString() ) ) CurrentOutput.Pause();
                }
                else if( State == PlaybackState.Paused ) {
                    if( ImGui.Button( FontAwesomeIcon.Play.ToIconString() ) ) CurrentOutput.Play();
                }
            }

            var selectedTime = ( float )CurrentTime;
            ImGui.SameLine( 25f );
            ImGui.SetNextItemWidth( 221f );
            var drawPos = ImGui.GetCursorScreenPos();

            using( var stopped = ImRaii.PushStyle( ImGuiStyleVar.Alpha, 0.5f, State == PlaybackState.Stopped ) ) {
                if( ImGui.SliderFloat( "##Drag", ref selectedTime, 0, ( float )TotalTime ) ) {
                    if( State != PlaybackState.Stopped && selectedTime > 0 && selectedTime < TotalTime ) {
                        CurrentOutput.Pause();
                        LeftStream.CurrentTime = TimeSpan.FromSeconds( selectedTime );
                        RightStream.CurrentTime = TimeSpan.FromSeconds( selectedTime );
                    }
                }
            }

            if( State != PlaybackState.Stopped && Entry.LoopTime.Y > 0 && Plugin.Configuration.SimulateScdLoop ) {
                if( Entry.LoopTime.X > ( TotalTime + 1f ) || Entry.LoopTime.Y > ( TotalTime + 1f ) ) return; // out of bounds

                var dragWidth = ImGui.GetStyle().GrabMinSize + 4f;
                var range = Entry.LoopTime * ( ( 221f - dragWidth ) / ( float )TotalTime ) + new Vector2( dragWidth / 2f );

                var startPos = drawPos + new Vector2( range.X - 1, 0 );
                var endPos = drawPos + new Vector2( range.Y - 1, 0 );

                var height = ImGui.GetFrameHeight();
                var drawList = ImGui.GetWindowDrawList();
                drawList.AddRectFilled( startPos, startPos + new Vector2( 2, height ), ImGui.GetColorU32( UiUtils.PARSED_GREEN ), 1 );
                drawList.AddRectFilled( endPos, endPos + new Vector2( 2, height ), ImGui.GetColorU32( UiUtils.DALAMUD_RED ), 1 );
            }
        }

        private void DrawPlayer() {
            using var tabItem = ImRaii.TabItem( "音乐" );
            if( !tabItem ) return;
            using var _ = ImRaii.PushId( "Music" );

            DrawControls();

            // Save
            ImGui.SameLine();
            using( var font = ImRaii.PushFont( UiBuilder.IconFont ) ) {
                if( ImGui.Button( FontAwesomeIcon.Save.ToIconString() ) ) {
                    if( IsVorbis ) ImGui.OpenPopup( "SavePopup" );
                    else SaveWaveDialog();
                }
            }
            UiUtils.Tooltip( "导出音效文件为 .wav 或 .ogg 格式" );

            using( var popup = ImRaii.Popup( "SavePopup" ) ) {
                if( popup ) {
                    if( ImGui.Selectable( ".wav" ) ) SaveWaveDialog();
                    if( ImGui.Selectable( ".ogg" ) ) SaveOggDialog();
                }
            }

            // Import
            using( var font = ImRaii.PushFont( UiBuilder.IconFont ) )
            using( var style = ImRaii.PushStyle( ImGuiStyleVar.ItemSpacing, new Vector2( 3, 4 ) ) ) {
                ImGui.SameLine();
                if( ImGui.Button( FontAwesomeIcon.Upload.ToIconString() ) ) ImportDialog();
            }
            UiUtils.Tooltip( "替换音效文件" );

            var loop = Entry.LoopTime;
            ImGui.SetNextItemWidth( 246f );
            if( ImGui.InputFloat2( "##LoopStartEnd", ref loop ) ) Entry.LoopTime = loop;

            ImGui.SameLine();
            ImGui.Text( "循环时间" );

            ImGui.TextDisabled( $"{Entry.Format} / {Entry.NumChannels} 声道 / {Entry.SampleRate}Hz / 0x{Entry.DataLength:X8} 字节" );
        }

        private void DrawChannels() {
            if( !ShowChannelSelect ) return;

            using var tabItem = ImRaii.TabItem( "声道" );
            if( !tabItem ) return;

            using var _ = ImRaii.PushId( "Channels" );

            ImGui.TextDisabled( "在预览音频文件时播放哪些声道。不影响 .scd 文件" );

            if( ImGui.BeginCombo( "预览声道 1", $"Channel #{Channel1}" ) ) {
                for( var i = 0; i < Entry.NumChannels; i++ ) {
                    if( ImGui.Selectable( $"声道 #{i}", Channel1 == i ) ) Channel1 = i;
                }
                ImGui.EndCombo();
            }

            if( ImGui.BeginCombo( "预览声道 2", $"Channel #{Channel2}" ) ) {
                for( var i = 0; i < Entry.NumChannels; i++ ) {
                    if( ImGui.Selectable( $"声道 #{i}", Channel2 == i ) ) Channel2 = i;
                }
                ImGui.EndCombo();
            }

            if( ImGui.Button( "刷新" ) ) Reset();
        }

        private void ProcessQueue() {
            var currentState = State;
            var justQueued = false;

            if( currentState == PlaybackState.Stopped && PrevState == PlaybackState.Playing && LoopSound ) {
                Play();
                if( Plugin.Configuration.SimulateScdLoop && Entry.LoopTime.X > 0 && Entry.LoopTime.Y > 0 ) {
                    if( QueueSeek == -1 ) {
                        QueueSeek = Entry.LoopTime.X;
                        justQueued = true;
                    }
                }
            }
            else if( currentState == PlaybackState.Playing && Entry.LoopTime.Y > 0 && Plugin.Configuration.SimulateScdLoop && Math.Abs( Entry.LoopTime.Y - CurrentTime ) < 0.1f ) {
                if( QueueSeek == -1 ) {
                    QueueSeek = Entry.LoopTime.X;
                    justQueued = true;
                }
            }

            if( currentState == PlaybackState.Playing && QueueSeek != -1 && !justQueued ) {
                LeftStream.CurrentTime = TimeSpan.FromSeconds( QueueSeek );
                RightStream.CurrentTime = TimeSpan.FromSeconds( QueueSeek );
                QueueSeek = -1;
            }

            PrevState = currentState;
        }

        private void Play() {
            Reset();
            try {
                var stream = Entry.Data.GetStream();
                var format = stream.WaveFormat;
                LeftStream = ConvertStream( stream );
                RightStream = ConvertStream( Entry.Data.GetStream() );

                var firstChannel = ShowChannelSelect ? Channel1 : 0;
                var secondChannel = ShowChannelSelect ? Channel2 : ( format.Channels > 1 ? 1 : 0 );

                var leftStreamIsolated = new MultiplexingWaveProvider( [LeftStream], 1 );
                leftStreamIsolated.ConnectInputToOutput( firstChannel, 0 );

                var rightStreamIsolated = new MultiplexingWaveProvider( [RightStream], 1 );
                rightStreamIsolated.ConnectInputToOutput( secondChannel, 0 );

                LeftRightCombined = new MultiplexingWaveProvider( [leftStreamIsolated, rightStreamIsolated], 2 );
                LeftRightCombined.ConnectInputToOutput( 0, 0 );
                LeftRightCombined.ConnectInputToOutput( 1, 1 );

                if( format.Encoding == WaveFormatEncoding.IeeeFloat ) {
                    var floatVolume = new WaveFloatTo16Provider( LeftRightCombined ) {
                        Volume = Plugin.Configuration.ScdVolume
                    };
                    Volume = floatVolume;
                }
                else {
                    var pcmVolume = new VolumeWaveProvider16( LeftRightCombined ) {
                        Volume = Plugin.Configuration.ScdVolume
                    };
                    Volume = pcmVolume;
                }

                CurrentOutput = new WasapiOut();
                CurrentOutput.Init( Volume );
                CurrentOutput.Play();
            }
            catch( Exception e ) {
                Dalamud.Error( e, "播放音效时发生错误" );
            }
        }

        public void UpdateVolume() {
            if( Volume == null ) return;
            if( Volume is WaveFloatTo16Provider floatVolume ) {
                floatVolume.Volume = Plugin.Configuration.ScdVolume;
            }
            else if( Volume is VolumeWaveProvider16 pcmVolume ) {
                pcmVolume.Volume = Plugin.Configuration.ScdVolume;
            }
        }

        public void Reset() {
            CurrentOutput?.Stop();
            CurrentOutput?.Dispose();
            LeftStream?.Dispose();
            RightStream?.Dispose();
            LeftStream = null;
            RightStream = null;
            CurrentOutput = null;
        }

        public void Dispose() {
            CurrentOutput?.Stop();
            Reset();
        }

        // ======================

        private void ImportDialog() {
            FileBrowserManager.OpenFileDialog( "导入文件", "音频文件{.ogg,.wav},.*", ( bool ok, string res ) => {
                if( ok ) {
                    Reset();
                    Entry.File.Import( res, Entry );
                }
            } );
        }

        private void SaveWaveDialog() {
            FileBrowserManager.SaveFileDialog( "选择保存位置", ".wav", "ExportedSound", "wav", ( bool ok, string res ) => {
                if( ok ) {
                    using var stream = Entry.Data.GetStream();
                    WaveFileWriter.CreateWaveFile( res, stream );
                }
            } );
        }

        private void SaveOggDialog() {
            FileBrowserManager.SaveFileDialog( "选择保存位置", ".ogg", "ExportedSound", "ogg", ( bool ok, string res ) => {
                if( ok ) {
                    var data = ( ScdVorbis )Entry.Data;
                    File.WriteAllBytes( res, data.Data );
                }
            } );
        }

        private static WaveStream ConvertStream( WaveStream stream ) => stream.WaveFormat.Encoding switch {
            WaveFormatEncoding.Pcm => WaveFormatConversionStream.CreatePcmStream( stream ),
            WaveFormatEncoding.Adpcm => WaveFormatConversionStream.CreatePcmStream( stream ),
            _ => stream
        };
    }
}
