using UnityEngine;

namespace Testing
{
    public class ExplosiveBall : MonoBehaviour
    {
        [SerializeField] float _explosionRadius = 10f;
        [SerializeField] float _explosionForce = 100f;
        [SerializeField] float _upwardModifier = 3f;
        [SerializeField] float _explodeTimer = 10f;
        [Header("Automatic random timer settings")]
        [Tooltip("Enabling this overrides timer setting")]
        [SerializeField] bool _useRandomTimer = false;
        [SerializeField] float _randomTimerMin = 2f;
        [SerializeField] float _randomTimerMax = 10f;

        private float _defaultTimer = 10f;

        void Start()
        {
            if (_useRandomTimer)
                SetRandomInterval(_randomTimerMin, _randomTimerMax);
            else
                _defaultTimer = _explodeTimer;
        }

        // Update is called once per frame
        void Update()
        {
            if (_explodeTimer < 0)
            {
                Explode(transform.position, _explosionRadius, _explosionForce, _upwardModifier);
                ResetTimer();
            }
            else
                _explodeTimer -= Time.deltaTime;
        }

        private void SetRandomInterval(float randomIntervalMin, float randomIntervalMax)
        {
            float interval = Random.Range(randomIntervalMin, randomIntervalMax);
            _explodeTimer = interval;
            _defaultTimer = interval;
        }

        private void Explode(in Vector3 position, float radius, float power, float upwardModifier)
        {
            Collider[] colliders = Physics.OverlapSphere(position, _explosionRadius);
            foreach (Collider hit in colliders)
            {
                Rigidbody rb = hit.GetComponent<Rigidbody>();

                if (rb != null)
                    rb.AddExplosionForce(power, position, radius, upwardModifier);
            }
        }

        private void ResetTimer()
        {
            _explodeTimer = _defaultTimer;
        }
    }
}