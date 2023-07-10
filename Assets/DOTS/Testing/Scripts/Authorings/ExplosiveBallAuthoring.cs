using Unity.Entities;
using UnityEngine;
using Unity.Physics.Authoring;

namespace Testing
{
    [DisallowMultipleComponent]
    [RequiresEntityConversion]
    public class ExplosiveBallAuthoring : MonoBehaviour, IConvertGameObjectToEntity
    {
        [SerializeField] float _explosionRadius = 10f;
        [SerializeField] float _explosionForce = 100f;
        [SerializeField] float _upForce = 3f;
        [SerializeField] float _upwardModifier = 3f;
        [SerializeField] float _explodeTimer = 10f;
        [SerializeField] PhysicsCategoryTags _belongsTo;
        [SerializeField] PhysicsCategoryTags _collidesWith;

        [Header("Automatic random timer settings")]
        [Tooltip("Enabling this overrides timer setting")]
        [SerializeField] bool _useRandomTimer = false;
        [SerializeField] float _randomTimerMin = 2f;
        [SerializeField] float _randomTimerMax = 10f;

        public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
        {
            //To able to find on how many are there spawned.
            float timer = (_useRandomTimer) ? UnityEngine.Random.Range(_randomTimerMin, _randomTimerMax) : _explodeTimer;

            dstManager.AddComponentData(entity, new Tag_ExplosiveBall { });
            dstManager.AddComponentData(entity, new ExplosiveIntervalComponent {
                explosionData = new ExplosionData {
                    force = _explosionForce,
                    radius = _explosionRadius,
                    upForce = _upForce,
                    upModifier = _upwardModifier,
                    belongsTo = _belongsTo,
                    collidesWith = _collidesWith
                },
                timer = timer,
                defaultInterval = timer
            });
        }
    }
}