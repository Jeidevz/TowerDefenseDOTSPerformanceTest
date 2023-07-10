using Unity.Entities;
using Unity.Jobs;
using Unity.Transforms;
using UnityEngine;

namespace Testing
{
    public class TestMovableSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            float dt = Time.DeltaTime;

            Entities.ForEach((int entityInQueryIndex, ref Translation translation, in TestMovableComponent movableComponent) =>
            {
                translation.Value += movableComponent.moveDirection * movableComponent.moveSpeed * dt;

            }).Schedule();
        }
    }
}




