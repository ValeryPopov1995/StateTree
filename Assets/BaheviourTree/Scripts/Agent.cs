using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using XNode;

namespace ValeryPopov.Common.StateTree
{
    public abstract class Agent : MonoBehaviour { }

    public abstract class Agent<TAgent> : Agent where TAgent : Agent
    {
        [SerializeField] private StateGraph<TAgent> _graph;
        private State<TAgent> _startState;
        private State<TAgent> _currentState;
        private int _currentStateIndex;

        protected virtual void Start()
        {
            StartCircle();
        }

        private async void StartCircle()
        {
            _startState = _graph.nodes.FirstOrDefault(node => node is StartState<TAgent>) as State<TAgent>;

            if (_startState == null)
                throw new System.NullReferenceException("no start state");


            _currentState = _startState;
            while (!destroyCancellationToken.IsCancellationRequested)
            {
                NodePort port = await ExecuteState(_currentState);

                var customConnection = _currentState.GetCustomConnection();
                if (customConnection != null)
                {
                    _currentState = customConnection.node as State<TAgent>;
                    _currentStateIndex = _graph.nodes.IndexOf(_currentState);
                    continue;
                }

                if (port.Connection == null)
                    _currentState = _startState;
                else
                    _currentState = port.Connection.node as State<TAgent>;

                _currentStateIndex = _graph.nodes.IndexOf(_currentState);
            }
        }

        /// <summary>
        /// Override to this <code>return await state.Execute(this);</code>
        /// </summary>
        /// <returns>Output port</returns>
        protected abstract Task<NodePort> ExecuteState(State<TAgent> state);
    }
}