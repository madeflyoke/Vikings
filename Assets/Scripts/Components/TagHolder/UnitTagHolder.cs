using Components.TagHolder.Base;
using Units.Enums;

namespace Units.Components
{
    public class UnitTagHolder : TagHolder
    {
        public Team Team { get; }

        public UnitTagHolder(Team team)
        {
            Team = team;
        }
    }
}
