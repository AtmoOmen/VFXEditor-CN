using VFXEditor.Formats.AvfxFormat.Curve;

namespace VfxEditor.AvfxFormat {
    public class AvfxEffectorDataDirectionalLight : AvfxData {
        public readonly AvfxCurveColor Ambient = new( "环境", "Amb" );
        public readonly AvfxCurveColor Color = new( "颜色" );
        public readonly AvfxCurve1Axis Power = new( "强度", "Pow" );
        public readonly AvfxCurve1Axis PowerRandom = new( "随机强度", "PowR" );
        public readonly AvfxCurve3Axis Rotation = new( "旋转", "Rot", CurveType.Angle );

        public AvfxEffectorDataDirectionalLight() : base() {
            Parsed = [
                Ambient,
                Color,
                Power,
                PowerRandom,
                Rotation
            ];

            Tabs.Add( Ambient );
            Tabs.Add( Color );
            Tabs.Add( Power );
            Tabs.Add( PowerRandom );
            Tabs.Add( Rotation );
        }
    }
}
