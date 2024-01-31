using System.Threading.Tasks;
using UnityEngine;

namespace ValeryPopov.Common.StateTree.NpcSample
{
    public class Bullet : MonoBehaviour, IInitializable
    {
        public bool Initialized { get; private set; }

        private DamageData _data;

        private void Awake()
        {
            if (TryGetComponent<Collider>(out var collider))
            {
                if (!collider.isTrigger)
                {
                    Debug.LogWarning("bullet not trigger");
                    collider.isTrigger = true;
                }
            }
            else
                Debug.LogWarning("bullet no collider");
        }

        public async void Init(DamageData data)
        {
            _data = data;
            float startTime = Time.time;
            bool outTime() => Time.time > startTime + 3;

            while (!destroyCancellationToken.IsCancellationRequested && !outTime())
            {
                transform.Translate(Vector3.forward * Time.deltaTime * _data.Speed);
                await Task.Yield();
            }

            if (Application.isPlaying && !destroyCancellationToken.IsCancellationRequested)
                Destroy(gameObject);
            else
                DestroyImmediate(gameObject);
        }

        private void OnTriggerEnter(Collider other)
        {
            var npc = other.GetComponentInParent<Npc>();
            if (npc)
                npc.Health.Damage(_data.Damage);

            Destroy(gameObject);
        }
    }
}
