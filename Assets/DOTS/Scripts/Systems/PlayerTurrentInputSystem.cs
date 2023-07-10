using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Physics.Authoring;
using Unity.Physics.Systems;
using Unity.Transforms;
using UnityEngine;

namespace TowerDefenseDOTS
{
    [UpdateBefore(typeof(MovableSystem))]
    public class PlayerTurrentInputSystem : SystemBase
    {
        EndSimulationEntityCommandBufferSystem endSimEntCBS;
        BeginSimulationEntityCommandBufferSystem beginSimCommandBufferSystem;
        BuildPhysicsWorld buildPhysicsWorld;

        protected override void OnCreate()
        {
            base.OnCreate();
            endSimEntCBS = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
            buildPhysicsWorld = World.GetOrCreateSystem<BuildPhysicsWorld>();
            beginSimCommandBufferSystem = World.GetOrCreateSystem<BeginSimulationEntityCommandBufferSystem>();
        }

        protected override void OnUpdate()
        {
            EntityCommandBuffer ecbConcurrent = endSimEntCBS.CreateCommandBuffer();
            EntityCommandBuffer beginSimCommandBuffer = beginSimCommandBufferSystem.CreateCommandBuffer();
            CollisionWorld collisionWorld = buildPhysicsWorld.PhysicsWorld.CollisionWorld;
            Transform camera = Camera.main.transform;

            Entities
                .ForEach((ref Translation translation, ref Rotation rotation, ref Movable movable, ref LocalToWorld transform,
                in TurrentWeapons turrentGuns, in Tag_Player tag, in PlayerTurrentInputComponent input) =>
                {
                    float forward = 0f;
                    float right = 0f;

                    forward += (Input.GetKey(input.moveForward)) ? 10f : 0f;
                    forward -= (Input.GetKey(input.moveBackward)) ? 10f : 0f;
                    right += (Input.GetKey(input.moveRight)) ? 10f : 0f;
                    right -= (Input.GetKey(input.moveLeft)) ? 10f : 0f;

                    movable.direction = math.mul(rotation.Value, math.normalizesafe(new float3(right, 0f, forward)));

                    rotation.Value = quaternion.LookRotation(new float3(camera.forward.x, transform.Forward.y, camera.forward.z), math.up());

                    float3 forwardDir = camera.forward;
                    RotateArmToLookDirection(ref ecbConcurrent, in turrentGuns.leftArm, in forwardDir);
                    RotateArmToLookDirection(ref ecbConcurrent, in turrentGuns.rightArm, in forwardDir);

                    if (Input.GetKeyDown(input.fireGuns))
                    {
                        SetupGuns(ref ecbConcurrent, in turrentGuns.leftBottomGunBarrel, turrentGuns.ammoPrefab, in turrentGuns.ammoBelongsTo, in turrentGuns.ammoCollidesWith, 1f / 100f / 2f);
                        SetupGuns(ref ecbConcurrent, in turrentGuns.rightBottomGunBarrel, turrentGuns.ammoPrefab, in turrentGuns.ammoBelongsTo, in turrentGuns.ammoCollidesWith, 0f);
                    }
                    else if (Input.GetKeyUp(input.fireGuns))
                    {

                        if (World.DefaultGameObjectInjectionWorld.EntityManager.HasComponent<Fireable>(turrentGuns.leftBottomGunBarrel))
                            ecbConcurrent.RemoveComponent<Fireable>(turrentGuns.leftBottomGunBarrel);

                        if (World.DefaultGameObjectInjectionWorld.EntityManager.HasComponent<Fireable>(turrentGuns.rightBottomGunBarrel))
                            ecbConcurrent.RemoveComponent<Fireable>(turrentGuns.rightBottomGunBarrel);
                    }

                    if(Input.GetKeyDown(input.fireMissile))
                    {
                        FireMissile(ref beginSimCommandBuffer, in turrentGuns, in collisionWorld, in camera);
                    }
                }).WithoutBurst().Run();

            endSimEntCBS.AddJobHandleForProducer(Dependency);
            beginSimCommandBufferSystem.AddJobHandleForProducer(Dependency);
        }
        public static void SetupGuns(ref EntityCommandBuffer ecb, in Entity barrel, in Entity ammoPrefab, in PhysicsCategoryTags belongsTo, in PhysicsCategoryTags collidesWith, float firstBulletDelay)
        {
            if (World.DefaultGameObjectInjectionWorld.EntityManager.HasComponent<Fireable>(barrel))
                return;

            ecb.AddComponent<Fireable>(barrel, new Fireable
            {
                ammoPrefab = ammoPrefab,
                ammoLifeTime = new Lifetime { value = 10f },
                ammoBelongsTo = belongsTo,
                ammoCollidesWith = collidesWith,
                firePower = new FirePower { value = 100f },
                fireRate = new FireRate { value = 50f },
                timeLeftTillNextFire = firstBulletDelay,
                clip = SoundLibraryScriptableObject.Clip.Bullet
            });
        }

        public void RotateArmToLookDirection(ref EntityCommandBuffer ecb, in Entity arm, in float3 direction)
        {
            ecb.SetComponent<LookDirectionVertically>(arm, new LookDirectionVertically { direction = direction });
        }

        public void FireMissile(ref EntityCommandBuffer commandBuffer, in TurrentWeapons turrentGuns, in CollisionWorld collisionWorld, in Transform targeter)
        {
            Entity missileEntity = commandBuffer.Instantiate(turrentGuns.missilePrefab);
            LocalToWorld missileHolderTransform = EntityManager.GetComponentData<LocalToWorld>(turrentGuns.missileHolder);
            ExplosiveComponent missileExlosiveComponent = EntityManager.GetComponentData<ExplosiveComponent>(turrentGuns.missilePrefab);
            quaternion forwarDir = quaternion.LookRotationSafe(missileHolderTransform.Forward, math.up());
            commandBuffer.SetComponent<Rotation>(missileEntity, new Rotation { Value = forwarDir });
            commandBuffer.SetComponent<Translation>(missileEntity, new Translation { Value = missileHolderTransform.Position });

            RaycastInput missileRaycastInput = new RaycastInput
            {
                Start = targeter.position,
                End = targeter.position + targeter.forward * 1000,
                Filter = new CollisionFilter
                {
                    BelongsTo = missileExlosiveComponent.colliderBelongsTo.Value,
                    CollidesWith = missileExlosiveComponent.colliderCollidesWith.Value,
                    GroupIndex = 0
                }
            };

            Unity.Physics.RaycastHit hit;
            float3 missileTargetPosition;
            if (collisionWorld.CastRay(missileRaycastInput, out hit))
            {
                missileTargetPosition = hit.Position;
            }
            else
                missileTargetPosition = targeter.position + targeter.forward * 1000;


            //commandBuffer.AddComponent<PhysicsRotateTowardPositionDelayed>(missileEntity, new PhysicsRotateTowardPositionDelayed
            //{
            //    physicsRotate = new PhysicsRotateTowardPosition
            //    {
            //        position = missileTargetPosition,
            //        speed = 720f
            //    },
            //    delay = .5f
            //});

            commandBuffer.AddComponent<HomingProjectile>(missileEntity, new HomingProjectile
            {
                delay = .5f,
                speed = 10f,
                stopHomingDistance = 10f,
                targetPosition = missileTargetPosition
            });
        }
    }
}