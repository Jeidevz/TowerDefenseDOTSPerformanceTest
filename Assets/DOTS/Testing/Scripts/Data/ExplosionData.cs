using Unity.Physics.Authoring;

namespace Testing
{
    public struct ExplosionData
    {
        public float force;
        public float radius;
        public float upForce;
        public float upModifier;
        public PhysicsCategoryTags belongsTo;
        public PhysicsCategoryTags collidesWith;
    }
}


