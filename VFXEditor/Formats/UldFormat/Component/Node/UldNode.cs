using Dalamud.Interface;
using Dalamud.Interface.Utility.Raii;
using ImGuiNET;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using VfxEditor.Parsing;
using VfxEditor.Parsing.Data;
using VfxEditor.Parsing.Int;
using VfxEditor.Ui.Components.Base;
using VfxEditor.UldFormat.Component.Node.Data;
using VfxEditor.UldFormat.Component.Node.Data.Component;
using VfxEditor.UldFormat.Timeline;

namespace VfxEditor.UldFormat.Component.Node {
    public enum NodeType : int {
        Container = 1,
        Image = 2,
        Text = 3,
        NineGrid = 4,
        Counter = 5,
        Collision = 8,
        ClippingMask = 10,
    }

    [Flags]
    public enum NodeFlags {
        Visible = 0x01,
        Enabled = 0x02,
        Clip = 0x04,
        Fill = 0x08,
        AnchorTop = 0x10,
        AnchorBottom = 0x20,
        AnchorLeft = 0x40,
        AnchorRight = 0x80
    }

    public class UldNode : UldWorkspaceItem, IItemWithData<UldGenericData> {
        private readonly List<UldComponent> Components;
        private readonly UldWorkspaceItem Parent;

        public readonly SelectView<UldNode> NodeView;
        public readonly ParsedIntSelect<UldNode> ParentId;
        public readonly ParsedIntSelect<UldNode> NextSiblingId;
        public readonly ParsedIntSelect<UldNode> PrevSiblingId;
        public readonly ParsedIntSelect<UldNode> ChildNodeId;

        public bool IsComponentNode = false;
        public readonly ParsedInt ComponentTypeId = new( "##ComponentId" );
        public UldGenericData Data = null;

        public readonly ParsedDataEnum<NodeType, UldGenericData> Type;
        private readonly List<ParsedBase> Parsed;

        public readonly ParsedShort TabIndex = new( "标签页索引" );
        public readonly ParsedInt Unk1 = new( "未知 1" );
        public readonly ParsedInt Unk2 = new( "未知 2" );
        public readonly ParsedInt Unk3 = new( "未知 3" );
        public readonly ParsedInt Unk4 = new( "未知 4" );
        public readonly ParsedShort2 Position = new( "位置" );
        public readonly ParsedShort2 Size = new( "大小" );
        public readonly ParsedRadians Rotation = new( "旋转" );
        public readonly ParsedFloat2 Scale = new( "缩放" );
        public readonly ParsedShort2 Origin = new( "原点" );
        public readonly ParsedUInt Priority = new( "优先级", size: 2 );
        public readonly ParsedFlag<NodeFlags> Flags = new( "标识", size: 1 );
        public readonly ParsedInt Unk7 = new( "未知 7", size: 1 );
        public readonly ParsedShort3 MultiplyColor = new( "颜色乘算" );
        public readonly ParsedShort3 AddColor = new( "颜色加算" );
        public readonly ParsedInt Alpha = new( "透明度", size: 1 );
        public readonly ParsedInt ClipCount = new( "片段数", size: 1 );

        public readonly ParsedIntSelect<UldTimeline> TimelineId = new( "时间线", 0,
            () => Plugin.UldManager.File.TimelineDropdown,
            ( UldTimeline item ) => ( int )item.Id.Value,
            ( UldTimeline item, int _ ) => item.GetText(),
            size: 2
        );

        // need to wait until all components are initialized, so store this until then
        private readonly long _Position;
        private readonly int _Size;
        private readonly int _Type;

        public UldNode( uint id, List<UldComponent> components, UldWorkspaceItem parent, SelectView<UldNode> nodeView ) : base( id ) {
            Parent = parent;
            Components = components;
            Type = new( this, "Type" );

            Parsed = [
                TabIndex,
                Unk1,
                Unk2,
                Unk3,
                Unk4,
                Position,
                Size,
                Rotation,
                Scale,
                Origin,
                Priority,
                Flags,
                Unk7,
                MultiplyColor,
                AddColor,
                Alpha,
                ClipCount
            ];

            NodeView = nodeView;
            ParentId = new( "父级", 0,
                () => NodeView,
                ( UldNode item ) => ( int )item.Id.Value,
                ( UldNode item, int _ ) => item.GetText()
            );
            NextSiblingId = new( "下一节点", 0,
                () => NodeView,
                ( UldNode item ) => ( int )item.Id.Value,
                ( UldNode item, int _ ) => item.GetText()
            );
            PrevSiblingId = new( "上一节点", 0,
                () => NodeView,
                ( UldNode item ) => ( int )item.Id.Value,
                ( UldNode item, int _ ) => item.GetText()
            );
            ChildNodeId = new( "子级", 0,
                () => NodeView,
                ( UldNode item ) => ( int )item.Id.Value,
                ( UldNode item, int _ ) => item.GetText()
            );
        }

        public UldNode( BinaryReader reader, List<UldComponent> components, UldWorkspaceItem parent, SelectView<UldNode> nodeView ) : this( 0, components, parent, nodeView ) {
            var pos = reader.BaseStream.Position;

            Id.Read( reader );
            ParentId.Read( reader );
            NextSiblingId.Read( reader );
            PrevSiblingId.Read( reader );
            ChildNodeId.Read( reader );

            var nodeType = reader.ReadInt32();
            var size = reader.ReadUInt16();
            // TODO: what if offset <= 88

            if( nodeType > 1000 ) {
                IsComponentNode = true;
                ComponentTypeId.Value = nodeType;
            }
            else {
                Type.Value = ( NodeType )nodeType;
            }

            Parsed.ForEach( x => x.Read( reader ) );
            TimelineId.Read( reader );

            _Position = reader.BaseStream.Position;
            _Size = ( int )( pos + size - reader.BaseStream.Position ) - 12;
            _Type = nodeType;

            reader.BaseStream.Position = pos + size;
        }

        // Needs to be initialized later since it depends on component list being filled
        public void InitData( BinaryReader reader ) {
            reader.BaseStream.Position = _Position;

            UpdateData();
            if( Data == null && _Type > 1 ) {
                Dalamud.Log( $"未知节点类型 {_Type} / {_Size} @ {reader.BaseStream.Position:X8}" );
            }
            if( Data is BaseNodeData custom ) custom.Read( reader, _Size );
            else Data?.Read( reader );
        }

        public void Write( BinaryWriter writer ) {
            var pos = writer.BaseStream.Position;

            Id.Write( writer );
            ParentId.Write( writer );
            NextSiblingId.Write( writer );
            PrevSiblingId.Write( writer );
            ChildNodeId.Write( writer );

            if( IsComponentNode ) ComponentTypeId.Write( writer );
            else Type.Write( writer );

            var savePos = writer.BaseStream.Position;
            writer.Write( ( ushort )0 );

            Parsed.ForEach( x => x.Write( writer ) );
            TimelineId.Write( writer );

            Data?.Write( writer );

            var finalPos = writer.BaseStream.Position;
            var size = finalPos - pos;
            writer.BaseStream.Position = savePos;
            writer.Write( ( ushort )size );
            writer.BaseStream.Position = finalPos;
        }

        public void UpdateData() {
            if( IsComponentNode ) {
                var component = Components.Where( x => x.Id.Value == ComponentTypeId.Value ).FirstOrDefault();
                if( component == null ) Data = null;
                else {
                    Data = component.Type.Value switch {
                        //ComponentType.Custom => new CustomNodeData(),
                        ComponentType.Button => new ButtonNodeData(),
                        ComponentType.Window => new WindowNodeData(),
                        ComponentType.CheckBox => new CheckboxNodeData(),
                        ComponentType.RadioButton => new RadioButtonNodeData(),
                        ComponentType.Gauge => new GaugeNodeData(),
                        ComponentType.Slider => new SliderNodeData(),
                        ComponentType.TextInput => new TextInputNodeData(),
                        ComponentType.NumericInput => new NumericInputNodeData(),
                        ComponentType.List => new ListNodeData(),
                        ComponentType.Tabbed => new TabbedNodeData(),
                        ComponentType.ListItem => new ListItemNodeData(),
                        ComponentType.NineGridText => new NineGridTextNodeData(),
                        ComponentType.HoldButton => new HoldButtonNodeData(),
                        _ => new UldNodeComponentData()
                    };
                }
            }
            else {
                Data = Type.Value switch {
                    NodeType.Image => new ImageNodeData(),
                    NodeType.Text => new TextNodeData(),
                    NodeType.NineGrid => new NineGridNodeData(),
                    NodeType.Counter => new CounterNodeData(),
                    NodeType.Collision => new CollisionNodeData(),
                    NodeType.ClippingMask => new ClippingMaskNodeData(),
                    _ => null
                };
            }
        }

        public void SetData( UldGenericData data ) { Data = data; }

        public UldGenericData GetData() => Data;

        public override void Draw() {
            DrawRename();
            Id.Draw();

            if( ImGui.Checkbox( "是否为组件节点", ref IsComponentNode ) ) CommandManager.Add( new UldNodeDataCommand( this, true ) );

            if( IsComponentNode ) {
                ComponentTypeId.Draw();
                using var style = ImRaii.PushStyle( ImGuiStyleVar.ItemSpacing, ImGui.GetStyle().ItemInnerSpacing );
                ImGui.SameLine();
                using( var font = ImRaii.PushFont( UiBuilder.IconFont ) ) {
                    if( ImGui.Button( FontAwesomeIcon.Check.ToIconString() ) ) CommandManager.Add( new UldNodeDataCommand( this ) );
                }
                ImGui.SameLine();
                ImGui.Text( "组件类型" );
            }
            else Type.Draw();

            ImGui.SetCursorPosY( ImGui.GetCursorPosY() + 5 );

            using var tabBar = ImRaii.TabBar( "栏", ImGuiTabBarFlags.NoCloseWithMiddleMouseButton );
            if( !tabBar ) return;

            DrawParameters();
            DrawData();
        }

        private void DrawParameters() {
            using var tabItem = ImRaii.TabItem( "参数" );
            if( !tabItem ) return;

            using var _ = ImRaii.PushId( "Parameters" );
            using var child = ImRaii.Child( "Child" );

            TimelineId.Draw();
            ParentId.Draw();
            NextSiblingId.Draw();
            PrevSiblingId.Draw();
            ChildNodeId.Draw();

            Parsed.ForEach( x => x.Draw() );
        }

        private void DrawData() {
            if( Data == null ) return;

            using var tabItem = ImRaii.TabItem( "数据" );
            if( !tabItem ) return;

            using var _ = ImRaii.PushId( "Data" );
            using var child = ImRaii.Child( "Child" );

            Data.Draw();
        }

        public override string GetDefaultText() {
            var suffix = IsComponentNode ? ComponentTypeId.Value.ToString() : Type.Value.ToString();
            return $"节点 {GetIdx()} ({suffix})";
        }

        public override string GetWorkspaceId() => $"{Parent.GetWorkspaceId()}/Node{GetIdx()}";
    }
}
