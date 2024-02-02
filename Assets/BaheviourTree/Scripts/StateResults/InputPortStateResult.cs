using XNode;

namespace ValeryPopov.Common.StateTree
{
    public class InputPortStateResult<TAgent> : IStateResult<TAgent> where TAgent : Agent
    {
        private NodePort _inputPort;

        public InputPortStateResult(NodePort nextStateInputPort)
        {
            _inputPort = nextStateInputPort;
        }

        public State<TAgent> Complete(Agent<TAgent> agent)
        {
            if (_inputPort == null)
                return agent.StartState;
            else
                return _inputPort.node as State<TAgent>;
        }
    }
}
