using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using TowerDefenseDOTS;
using Unity.Physics.Authoring;
using Unity.Transforms;
using Unity.Mathematics;

namespace TowerDefenseDOTS
{
    public class TurrentAuthoring : MonoBehaviour, IConvertGameObjectToEntity
    {
        [Header("Guns")]
        //public Entity leftArm;
        //public Entity rightArm;
        //public Entity leftTopGunBarrel;
        //public Entity leftBottomGunBarrel;
        //public Entity rightTopGunBarrel;
        //public Entity rightBottomGunBarrel;
        //Transform of where bullets come out.
        public GameObject[] barrels;
        public SoundLibraryScriptableObject.Clip fireSFXClip;
        public VFXLibraryScriptableObject.Effect fireVFXEffect;

        [Header("Ammo")]
        public GameObject ammoPrefab;
        public float lifetime;
        public PhysicsCategoryTags colliderBelongsTo;
        public PhysicsCategoryTags colliderCollidesWith;
        public SoundLibraryScriptableObject.Clip bulletHitSFXClip;
        public VFXLibraryScriptableObject.Effect bulletHitVFXEffect;

        // Start is called before the first frame update
        void Start()
        {

        }
        public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
        {
            //dstManager.AddComponentData<Translation>(entity, new Translation { Value = new float3(0, 0, 0) });
            //DynamicBuffer<GunBarrelsBuffer> dynamicGunBarrelsBuffer = dstManager.AddBuffer<GunBarrelsBuffer>(entity);
            //for (int i = 0; i < barrels.Length; i++)
            //{
            //    Transform trans = barrels[i].transform;
            //    Entity barrelEnt = dstManager.CreateEntity();
            //    dstManager.AddComponentData<PreviousParent>(barrelEnt, new PreviousParent { Value = })
            //    dynamicGunBarrelsBuffer.Add(new TurrentGunBarrelBufferData { barrel = barrels[i] });
            //}
        }
    }
}
