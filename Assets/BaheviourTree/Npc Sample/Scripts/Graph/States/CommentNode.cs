using UnityEngine;
using XNode;

namespace ValeryPopov.Common.StateTree.NpcSample
{
    [CreateNodeMenu("StateTree/Npc Sample/Base/Comment"), NodeWidth(400), NodeTint(150, 0, 0)]
    public class CommentNode : Node
    {
        [SerializeField, TextArea(5, 9)] private string comment;
    }
}