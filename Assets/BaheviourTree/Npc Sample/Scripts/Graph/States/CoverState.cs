using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

namespace ValeryPopov.Common.StateTree.NpcSample
{
    [CreateNodeMenu("StateTree/Npc Sample/Cover")]
    public class CoverState : NpcState
    {
        private enum CoverType
        {
            first, second, last
        }

        private enum FindCoverZoneType
        {
            forwardHemisphere, backwardHemisphere, all
        }

        [SerializeField] private int _maxMoveDuration = 5;
        [SerializeField] private int _busyCoverDistance = 2;
        [SerializeField] private CoverType _coverType = CoverType.last;
        [SerializeField] private FindCoverZoneType _findCoverZoneType = FindCoverZoneType.backwardHemisphere;

        [field: SerializeField, Output(connectionType = ConnectionType.Override, typeConstraint = TypeConstraint.Strict)]
        private NpcState _covered, _noCover;

        public override async Task<StateResult<Npc>> Execute(Npc agent)
        {
            float time = Time.time;
            var covers = FindCovers(agent)
                .Where(cvr => !IsBusyPoint(cvr.transform.position));
            covers = GetCoversByZone(agent, covers);
            Cover cover = GetCoverByCoverType(covers);



            if (cover)
            {
                agent.Mover.MoveTo(cover.GetCoverTarget(agent));
                bool maxDuration() => time > Time.time + _maxMoveDuration;
                await UniTask.WaitWhile(() => agent.Mover.IsMove || maxDuration());

                if (maxDuration())
                    return new OutputPortStateResult<Npc>(GetOutputPort(nameof(_noCover)));

                return new OutputPortStateResult<Npc>(GetOutputPort(nameof(_covered)));
            }

            return new OutputPortStateResult<Npc>(GetOutputPort(nameof(_noCover)));
        }

        private IEnumerable<Cover> GetCoversByZone(Npc agent, IEnumerable<Cover> covers)
        {
            switch (_findCoverZoneType)
            {
                case FindCoverZoneType.forwardHemisphere:
                    return covers.Where(cover => Vector3.Dot(agent.transform.forward, cover.transform.position - agent.transform.position) > 0);
                case FindCoverZoneType.backwardHemisphere:
                    return covers.Where(cover => Vector3.Dot(agent.transform.forward, cover.transform.position - agent.transform.position) < 0);
                case FindCoverZoneType.all:
                    return covers;
                default:
                    return covers;
            }
        }

        private Cover GetCoverByCoverType(IEnumerable<Cover> covers)
        {
            switch (_coverType)
            {
                case CoverType.first:
                    return covers.FirstOrDefault();
                case CoverType.second:
                    if (covers.Count() > 1)
                        return covers.ElementAt(1);
                    else
                        return covers.FirstOrDefault();
                case CoverType.last:
                    return covers.LastOrDefault();

                default:
                    return covers.FirstOrDefault();
            }
        }

        private IEnumerable<Cover> FindCovers(Npc agent)
        {
            return Physics.OverlapSphere(agent.transform.position, 15)
                .Select(collider => collider.GetComponentInParent<Cover>())
                .Where(cover => cover)
                .OrderBy(cover => Vector3.Distance(agent.transform.position, cover.transform.position))
                .Distinct();
        }

        bool IsBusyPoint(Vector3 point)
        {
            return Physics.OverlapSphere(point, _busyCoverDistance)
                .Select(collider => collider.GetComponentInParent<Npc>())
                .Where(npc => npc && npc.Health.IsAlive)
                .Any();
        }
    }
}