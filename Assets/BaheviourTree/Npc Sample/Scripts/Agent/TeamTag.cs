using System;

namespace ValeryPopov.Common.StateTree.NpcSample
{
    [Serializable]
    public class TeamTag
    {
        public byte TeamId;

        public static bool operator ==(TeamTag left, TeamTag right)
        {
            return left.TeamId == right.TeamId;
        }

        public static bool operator !=(TeamTag left, TeamTag right)
        {
            return left.TeamId != right.TeamId;
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return TeamId;
        }

        internal Npc GetNearestTeammate(Npc npc)
        {
            throw new NotImplementedException();
        }
    }
}
