namespace VfxEditor.AvfxFormat {
    public class AvfxParticleDataLightModel : AvfxDataWithParameters {
        public readonly AvfxInt ModelIdx = new( "模型索引", "MNO", size: 1 );
        public readonly AvfxNodeSelect<AvfxModel> ModelSelect;

        public AvfxParticleDataLightModel( AvfxParticle particle ) : base() {
            Parsed = [
                ModelIdx
            ];

            ParameterTab.Add( ModelSelect = new AvfxNodeSelect<AvfxModel>( particle, "模型", particle.NodeGroups.Models, ModelIdx ) );
        }

        public override void Enable() => ModelSelect.Enable();

        public override void Disable() => ModelSelect.Disable();
    }
}
