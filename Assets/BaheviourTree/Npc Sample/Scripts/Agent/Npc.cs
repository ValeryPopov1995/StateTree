using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using XNode;

namespace ValeryPopov.Common.StateTree.NpcSample
{
    public class Npc : Agent<Npc>
    {
        public Transform Transform => transform;
        [field: SerializeField] public Rigidbody Rigidbody { get; private set; }
        [field: SerializeField] public Health Health { get; private set; }
        [field: SerializeField] public Inventory Inventory { get; private set; }
        [field: SerializeField] public TeamTag TeamTag { get; private set; }
        [field: SerializeField] public Communication Communication { get; private set; }
        [field: SerializeField] public Mover Mover { get; private set; }
        public Npc TargetEnemy { get; set; }
        public Target TargetWarning { get; set; }
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
            Destroy(gameObject);
        }

        protected override async Task<NodePort> ExecuteState(State<Npc> state)
        {
            await Task.Yield(); // NOTEBENE good practice, can help avoid infinite loop and find error
            return await state.Execute(this);
        }

        public IEnumerable<Npc> OverlapNpcs(int distance)
        {
            return FindObjectsByType<Npc>(FindObjectsSortMode.None)
                .Where(charcter => this != charcter &&
                Vector3.Distance(transform.position, charcter.transform.position) < distance);
        }

        internal Cover FindCover()
        {
            var covers = Physics.OverlapSphere(transform.position, 15)
                .Select(collider => collider.GetComponentInParent<Cover>())
                .Where(cover => cover)
                .OrderBy(cover => Vector3.Distance(transform.position, cover.transform.position))
                .Distinct();

            if (covers.Count() > 1)
                return covers.ElementAt(1);

            return covers.LastOrDefault();
        }
    }
}