using System;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

namespace ValeryPopov.Common.StateTree.NpcSample
{
    public class Granade : Item
    {
        [SerializeField] private int _damage = 3;
        [SerializeField] private int _timer = 2;
        [SerializeField] private int _distance = 5;
        [SerializeField] private GameObject _spawnOnExplosion;

        public async void Throw(Vector3 point, Vector3 force)
        {
            transform.position = point;
            DropFromInventory();
            IsPickable = false;
            Rigidbody.AddForce(force, UnityEngine.ForceMode.Impulse);

            await Task.Delay(TimeSpan.FromSeconds(_timer));
            if (!Application.isPlaying) return;

            Instantiate(_spawnOnExplosion, transform.position, transform.rotation);

            var damagables = Physics.OverlapSphere(transform.position, _distance)
                .Select(collider => collider.GetComponentInParent<IDamagable>())
                .Where(damagable => damagable != null);

            foreach (var damagable in damagables)
                damagable.GetDamage(_damage);

            Destroy(gameObject);
        }
    }
}