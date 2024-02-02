using UnityEngine;

namespace ValeryPopov.Common.StateTree.NpcSample
{
    public class Medpack : Item
    {
        [SerializeField, Min(1)] private int _healPoints = 10;
        [SerializeField] private GameObject _visual;
        [SerializeField] private GameObject _usedVisual;

        protected override void Awake()
        {
            base.Awake();
            _usedVisual.SetActive(false);
        }

        public void Heal(Npc npc)
        {
            _visual.SetActive(false);
            _usedVisual.SetActive(true);

            npc.GetHeal(_healPoints);

            DropFromInventory();
            IsPickable = false;
        }
    }
}
