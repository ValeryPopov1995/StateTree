using System;
using System.Threading.Tasks;
using UnityEngine;

namespace ValeryPopov.Common.StateTree.NpcSample
{
    public class Weapon : Item
    {
        public Magazine ConnectedMagazine { get; private set; }
        [SerializeField] private DamageData _damageData;
        [SerializeField] private Bullet _bulletPrefab;
        [SerializeField] private Transform _connectedMagazinePoint;
        [SerializeField] private Transform _spawnPoint;
        [SerializeField] private Inventory _inventory;
        [SerializeField] private float _fireDelay = .2f;
        [SerializeField] private float _reloadDuration = 1;

        protected override void Awake()
        {
            base.Awake();

            _inventory.Init();

            if (_inventory.Items.Count > 1)
                throw new Exception("many magazines");

            ConnectedMagazine = _inventory.TryGetItem<Magazine>();
            SetMagazine(ConnectedMagazine);
        }

        public async Task Fire()
        {
            // TODO exception if destroy weapon on fire state

            if (ConnectedMagazine == null || ConnectedMagazine.CurrentCapacity <= 0) return;

            ConnectedMagazine.CurrentCapacity--;
            var bullet = Instantiate(_bulletPrefab, _spawnPoint.position, _spawnPoint.rotation);
            bullet.Init(_damageData);
            await Task.Delay(TimeSpan.FromSeconds(_fireDelay));
        }

        public async Task Reload(Magazine newMagazine)
        {
            newMagazine.CurrentCapacity = Mathf.Min(ConnectedMagazine.CurrentCapacity + newMagazine.CurrentCapacity, newMagazine.Capacity);

            ConnectedMagazine.DropFromInventory();

            SetMagazine(newMagazine);

            await Task.Delay(TimeSpan.FromSeconds(_reloadDuration));
        }

        private void SetMagazine(Magazine magazine)
        {
            ConnectedMagazine = magazine;
            ConnectedMagazine.PickUpByInventory(_inventory);
            magazine.transform.position = _connectedMagazinePoint.position;
            magazine.transform.rotation = _connectedMagazinePoint.rotation;
        }
    }
}
