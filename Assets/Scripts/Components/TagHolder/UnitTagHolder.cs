using Units.Enums;

namespace Components.TagHolder
{
    public class UnitTagHolder : Base.TagHolder
    {
        public Team Team { get; }

        public UnitTagHolder(Team team)
        {
            Team = team;
        }
    }
}
