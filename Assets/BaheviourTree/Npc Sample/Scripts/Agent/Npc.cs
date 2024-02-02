using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

namespace ValeryPopov.Common.StateTree.NpcSample
{
    public class Npc : Agent<Npc>, IDamagable, IHealable
    {
        public Transform Transform => transform;
        [field: SerializeField] public Rigidbody Rigidbody { get; private set; }
        [field: SerializeField] public Health Health { get; private set; }
        [field: SerializeField] public Inventory Inventory { get; private set; }
        [field: SerializeField] public TeamTag TeamTag { get; private set; }
        [field: SerializeField] public Communication Communication { get; private set; }
        [field: SerializeField] public OrderSystem OrderSystem { get; private set; }
        [field: SerializeField] public Mover Mover { get; private set; }
        [field: SerializeField] public Transform GranadeThrowPoint { get; private set; }

        public Npc TargetEnemy
        {
            get
            {
                if (_targetEnemy && _targetEnemy.Health.IsAlive)
                    return _targetEnemy;
                else
                    return _targetEnemy = null;
            }
            set
            {
                if (value != null && value.Health.IsAlive)
                    _targetEnemy = value;
                else
                    _targetEnemy = null;
            }
        }
        private Npc _targetEnemy;
        public Target TargetWarning { get; set; } = new EmptyTarget();
        public Vector3 StartPosition { get; set; }



        private void Awake()
        {
            Inventory.Init();
            Rigidbody.isKinematic = true;
            Communication.Init(this);
            Mover.Init(destroyCancellationToken);

            StartPosition = transform.position;

            Health.OnDeath += Die;
        }

        private void OnDestroy()
        {
            Health.OnDeath -= Die;
        }

        private void Die()
        {
            Dispose();
            Inventory.Dispose();
            Rigidbody.isKinematic = false;
            Communication.Dispose();
            Mover.Dispose();
            OrderSystem.Dispose();

            Rigidbody.AddTorque(UnityEngine.Random.onUnitSphere * 20); // falling
        }

        protected override async Task<IStateResult<Npc>> ExecuteState(State<Npc> state)
        {
            return await state.Execute(this);
        }

        /// <summary>
        /// Get npc near this agent
        /// </summary>
        /// <param name="distance">find distance from agent</param>
        /// <param name="includeDeads">include dead npcs</param>
        /// <returns>alived nps (optional), exclude this agent</returns>
        public IEnumerable<Npc> OverlapNpcs(int distance, bool includeDeads = false)
        {
            return FindObjectsByType<Npc>(FindObjectsSortMode.None)
                .Where(npc =>
                npc != this
                && (includeDeads || !includeDeads && npc.Health.IsAlive)
                && Vector3.Distance(transform.position, npc.transform.position) < distance);
        }

        public void GetDamage(int damage)
        {
            Health.GetDamage(damage);
        }

        public void GetHeal(int heal)
        {
            Health.GetHeal(heal);
        }
    }
}