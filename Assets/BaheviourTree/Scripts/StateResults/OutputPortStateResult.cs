﻿using XNode;

namespace ValeryPopov.Common.StateTree
{
    public class OutputPortStateResult<TAgent> : IStateResult<TAgent> where TAgent : Agent
    {
        private NodePort _outputPort;

        public OutputPortStateResult(NodePort outputPort)
        {
            _outputPort = outputPort;
        }

        public State<TAgent> Complete(Agent<TAgent> agent)
        {
            if (_outputPort.Connection == null)
                return agent.StartState;
            else
                return _outputPort.Connection.node as State<TAgent>;
        }
    }
}
