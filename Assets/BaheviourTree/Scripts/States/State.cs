using System.Threading.Tasks;
using XNode;

namespace ValeryPopov.Common.StateTree
{
    public abstract class State<TAgent> : Node where TAgent : Agent
    {
        public abstract Task<StateResult<TAgent>> Execute(TAgent agent);
    }
}
