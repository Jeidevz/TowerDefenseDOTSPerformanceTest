using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;

namespace TowerDefenseDOTS
{
    [DisableAutoCreation]
    //[UpdateInGroup(typeof(PhysicChangesSystemGroup))]
    //[UpdateAfter(typeof(BulletRaycastSystem))]
    public class BulletSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            Entities.ForEach((ref BulletComponent bullet, in Translation translation) =>
            {
                //if(!bullet.previousPosition.Equals(translation.Value))
                    bullet.previousPosition = translation.Value;

            }).Schedule();
        }
    }
}