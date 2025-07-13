using VFXEditor.Formats.AvfxFormat.Curve;

namespace VfxEditor.AvfxFormat {
    public class AvfxEffectorDataCameraQuake : AvfxData {
        public readonly AvfxCurve1Axis Attenuation = new( "衰减", "Att" );
        public readonly AvfxCurve1Axis AttenuationRandom = new( "随机衰减", "AttR" );
        public readonly AvfxCurve1Axis RadiusOut = new( "外半径", "RdO" );
        public readonly AvfxCurve1Axis RadiusOutRandom = new( "随机外半径", "RdOR" );
        public readonly AvfxCurve1Axis RadiusIn = new( "内半径", "RdI" );
        public readonly AvfxCurve1Axis RadiusInRandom = new( "随机内半径", "RdIR" );
        public readonly AvfxCurve3Axis Rotation = new( "旋转", "Rot", CurveType.Angle );
        public readonly AvfxCurve3Axis Position = new( "位置", "Pos" );

        public AvfxEffectorDataCameraQuake() : base() {
            Parsed = [
                Attenuation,
                AttenuationRandom,
                RadiusOut,
                RadiusOutRandom,
                RadiusIn,
                RadiusInRandom,
                Rotation,
                Position
            ];

            Tabs.Add( Attenuation );
            Tabs.Add( AttenuationRandom );
            Tabs.Add( RadiusOut );
            Tabs.Add( RadiusOutRandom );
            Tabs.Add( RadiusIn );
            Tabs.Add( RadiusInRandom );
            Tabs.Add( Rotation );
            Tabs.Add( Position );
        }
    }
}
