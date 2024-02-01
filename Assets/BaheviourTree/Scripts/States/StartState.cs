using UnityEngine;

namespace ValeryPopov.Common.StateTree
{
    public abstract class StartState<TAgent> : State<TAgent> where TAgent : Agent
    {
        [field: SerializeField, Output]
        protected State<TAgent> _firstState;
    }
}
