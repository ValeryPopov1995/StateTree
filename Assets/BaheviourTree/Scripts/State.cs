using System.Threading.Tasks;
using XNode;

namespace ValeryPopov.Common.StateTree
{
    public abstract class State<TAgent> : Node where TAgent : Agent
    {
        public abstract Task<NodePort> Execute(TAgent agent);

        /// <summary>
        /// Use it for custom selection of next State node.
        /// </summary>
        /// <returns>Input of next node</returns>
        public virtual NodePort GetCustomConnection() { return null; }
    }
}
