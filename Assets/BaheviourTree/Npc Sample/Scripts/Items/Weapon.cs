using System;
using System.Threading.Tasks;
using UnityEngine;

namespace ValeryPopov.Common.StateTree.NpcSample
{
    public class Weapon : Item
    {
        [SerializeField] private DamageData _damageData;
        [SerializeField] private Bullet _bulletPrefab;
        [field: SerializeField] public Magazine ConnectedMagazine { get; private set; }
        [SerializeField] private Transform _connectedMagazinePoint;
        [SerializeField] private Transform _spawnPoint;
        [SerializeField] private float _fireDelay = .2f;
        [SerializeField] private float _reloadDuration = 1;

        public async Task Fire()
        {
            var bullet = Instantiate(_bulletPrefab, _spawnPoint.position, _spawnPoint.rotation);
            bullet.Init(_damageData);
            await Task.Delay(TimeSpan.FromSeconds(_fireDelay));
        }

        public async Task Reload(Magazine newMagazine)
        {
            newMagazine.CurrentCapacity = Mathf.Min(ConnectedMagazine.CurrentCapacity + newMagazine.CurrentCapacity, newMagazine.Capacity);

            ConnectedMagazine.DropFromInventory();
            ConnectedMagazine = newMagazine;

            newMagazine.transform.position = _connectedMagazinePoint.position;
            newMagazine.transform.rotation = _connectedMagazinePoint.rotation;

            await Task.Delay(TimeSpan.FromSeconds(_reloadDuration));
        }
    }
}
