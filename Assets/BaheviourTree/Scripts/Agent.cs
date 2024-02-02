using System;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

namespace ValeryPopov.Common.StateTree
{
    public abstract class Agent : MonoBehaviour { }

    public abstract class Agent<TAgent> : Agent, IDisposable where TAgent : Agent
    {
        public StartState<TAgent> StartState { get; protected set; }

        [SerializeField] protected StateGraph<TAgent> _graph;
        protected float _minStateDuration = .1f;
        private float _lastStateExecuteTime;
        protected State<TAgent> _currentState;
#if UNITY_EDITOR
        [SerializeField] private string _logStateIndex = "";
#endif
        private bool _circleInWork;



        protected virtual void Start()
        {
            ExecuteCircle();
        }

        private async void ExecuteCircle()
        {
            _circleInWork = true;
            _lastStateExecuteTime = -_minStateDuration;
            StartState = _graph.nodes.FirstOrDefault(node => node is StartState<TAgent>) as StartState<TAgent>;
            if (StartState == null)
                throw new System.NullReferenceException("no start state");

            _currentState = StartState;
            while (!destroyCancellationToken.IsCancellationRequested && _circleInWork)
            {
#if UNITY_EDITOR
                _logStateIndex += _graph.nodes.IndexOf(_currentState) + " ";
#endif
                if (Time.time < _lastStateExecuteTime + _minStateDuration)
                {
                    //Debug.LogWarning("min state duration linit during " + _currentState.name);
                    await Task.Delay(TimeSpan.FromSeconds(_minStateDuration));
                }
                _lastStateExecuteTime = Time.time;
                var result = await ExecuteState(_currentState);
                _currentState = result.Complete(this);
            }
        }

        /// <summary>
        /// Override to this <code>return await state.Execute(this);</code>
        /// </summary>
        /// <returns>Output port</returns>
        protected abstract Task<IStateResult<TAgent>> ExecuteState(State<TAgent> state);

        public void Dispose()
        {
            _circleInWork = false;
        }
    }
}