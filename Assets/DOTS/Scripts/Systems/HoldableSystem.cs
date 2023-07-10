using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;

namespace TowerDefenseDOTS
{
    [DisableAutoCreation]
    public class HoldableSystem : SystemBase
    {
        protected override void OnUpdate()
        {

            float dt = Time.DeltaTime;



            Entities.ForEach((ref Translation translation, ref DynamicBuffer<EntityCommandBuffer> buffer, ref Holdable holdable, in Rotation rotation) =>
            {
                if (buffer.Length > 0)
                {
                    if(buffer.Length == 1)
                    { }
                    if (holdable.itemPositionUpdateDelay <= 0 && holdable.itemPositionUpdateDelay > -1)
                    {
                        float3 startPosition =
                        //Update positions


                        //Prevent updating position everyframe.
                        holdable.itemPositionUpdateDelay = -2;
                    }
                    else if (holdable.itemPositionUpdateDelay > 0)
                    {
                        holdable.itemPositionUpdateDelay -= dt;
                    }
                }
            }).Schedule();
        }
    }
}