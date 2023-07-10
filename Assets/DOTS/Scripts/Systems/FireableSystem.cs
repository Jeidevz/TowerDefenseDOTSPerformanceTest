using System.Collections.Generic;
using System.Numerics;
using TowerDefense;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace TowerDefenseDOTS
{
    [UpdateAfter(typeof(TransformSystemGroup))]
    public class FireableSystem : SystemBase
    {
        EntityCommandBufferSystem barrier => World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();

        protected override void OnUpdate()
        {
            float dt = Time.DeltaTime;
            EntityCommandBuffer.ParallelWriter commandBuffer = barrier.CreateCommandBuffer().AsParallelWriter();

            Entities
                .ForEach((Entity entity, int entityInQueryIndex, ref Fireable fireable, in LocalToWorld transform) =>
                {
                    if (fireable.timeLeftTillNextFire <= 0)
                    {
                        Entity bullet = commandBuffer.Instantiate(entityInQueryIndex, fireable.ammoPrefab);
                        float3 forwardDir = math.normalizesafe(transform.Forward);
                        commandBuffer.SetComponent<Movable>(entityInQueryIndex, bullet, new Movable { direction = forwardDir, speed = fireable.firePower.value });
                        commandBuffer.SetComponent<Translation>(entityInQueryIndex, bullet, new Translation { Value = transform.Position });
                        //commandBuffer.AddComponent<Lifetime>(entityInQueryIndex, bullet, new Lifetime { value = fireable.ammoLifeTime.value });
                        //commandBuffer.AddComponent<BulletComponent>(entityInQueryIndex, bullet, new BulletComponent
                        //{
                        //    damage = 100,
                        //    colliderBelongsTo = fireable.ammoBelongsTo,
                        //    colliderCollidesWith = fireable.ammoCollidesWith,
                        //    previousPosition = transform.Position
                        //});

                        commandBuffer.AddComponent<DamageableProjectile>(entityInQueryIndex, bullet, new DamageableProjectile
                        {
                            damage = 100,
                            colliderBelongsTo = fireable.ammoBelongsTo,
                            colliderCollidesWith = fireable.ammoCollidesWith,
                            previousPosition = transform.Position
                        });

                        fireable.timeLeftTillNextFire = 1f / fireable.fireRate.value;

                        Entity sfxEntity = commandBuffer.CreateEntity(entityInQueryIndex);
                        commandBuffer.AddComponent<SFX>(entityInQueryIndex, sfxEntity, new SFX 
                        { 
                            position = transform.Position, 
                            clip = SoundLibraryScriptableObject.Clip.Bullet, 
                            parent = entity 
                        });

                        Entity vfxEntity = commandBuffer.CreateEntity(entityInQueryIndex);
                        commandBuffer.AddComponent<VFX>(entityInQueryIndex, vfxEntity, new VFX
                        {
                            normal = math.normalizesafe(transform.Forward),
                            position = transform.Position,
                            effect = VFXLibraryScriptableObject.Effect.MuzzleFlash,
                            parent = entity
                        });
                    }

                    fireable.timeLeftTillNextFire -= dt;
                }).ScheduleParallel();

            barrier.AddJobHandleForProducer(Dependency);
        }
    }
}
