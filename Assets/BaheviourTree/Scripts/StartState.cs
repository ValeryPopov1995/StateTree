using System.Threading.Tasks;
using UnityEngine;
using XNode;

namespace ValeryPopov.Common.StateTree
{
    public abstract class StartState<TAgent> : State<TAgent> where TAgent : Agent
    {
        [field: SerializeField, Output]
        private State<TAgent> _firstState;

        public override async Task<NodePort> Execute(TAgent agent)
        {
            await Task.Yield();
            return GetOutputPort(nameof(_firstState));
        }
    }
}
