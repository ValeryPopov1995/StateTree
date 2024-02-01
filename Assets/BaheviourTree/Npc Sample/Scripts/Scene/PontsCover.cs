using System.Linq;
using UnityEngine;

namespace ValeryPopov.Common.StateTree.NpcSample
{
    public class PontsCover : Cover
    {
        [SerializeField] private Transform[] _points;

        public override Target GetCoverTarget(Npc npc)
        {
            Vector3 lookDirection = npc.transform.forward;

            if (npc.TargetEnemy != null)
                lookDirection = npc.TargetEnemy.Transform.position - npc.transform.position;

            // points behind cover
            Transform coverPoint = _points
                .FirstOrDefault(point => Vector3.Dot(lookDirection, transform.position - point.position) > 0);

            if (coverPoint == null)
                return null;
            else
                return new PointTarget(coverPoint.position);
        }
    }
}