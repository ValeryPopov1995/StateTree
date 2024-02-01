using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

namespace ValeryPopov.Common.StateTree
{
    public abstract class Agent : MonoBehaviour { }

    public abstract class Agent<TAgent> : Agent where TAgent : Agent
    {
        public Interruption<TAgent> UnhandledInterrupt { get; internal set; }
        public StartState<TAgent> StartState { get; private set; }
        public StartInterruptState<TAgent> InterruptionState { get; private set; }

        [SerializeField] private StateGraph<TAgent> _graph;
        private State<TAgent> _currentState;
#if UNITY_EDITOR
        private int _currentStateIndex; // for debug
#endif



        protected virtual void Start()
        {
            ExecuteCircle();
        }

        private async void ExecuteCircle()
        {
            StartState = _graph.nodes.FirstOrDefault(node => node is StartState<TAgent>) as StartState<TAgent>;
            if (StartState == null)
                throw new System.NullReferenceException("no start state");

            InterruptionState = _graph.nodes.FirstOrDefault(node => node is StartInterruptState<TAgent>) as StartInterruptState<TAgent>;
            if (InterruptionState == null)
                throw new System.NullReferenceException("no interruption state");

            _currentState = StartState;
            while (!destroyCancellationToken.IsCancellationRequested)
            {
                var result = await ExecuteState(_currentState);
                _currentState = result.Complete(this);
            }
        }

        /// <summary>
        /// Override to this <code>return await state.Execute(this);</code>
        /// </summary>
        /// <returns>Output port</returns>
        protected abstract Task<StateResult<TAgent>> ExecuteState(State<TAgent> state);
    }
}