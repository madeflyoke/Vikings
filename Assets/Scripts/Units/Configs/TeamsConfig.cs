using System;
using System.Collections.Generic;
using System.Linq;
using Units.Enums;
using UnityEngine;

namespace Units.Configs
{
    [CreateAssetMenu(fileName = "TeamsConfig", menuName = "Teams/TeamsConfig")]
    public class TeamsConfig : ScriptableObject
    {
        [SerializeField] private List<TeamConfigData> _teamConfigDatas;

        public TeamConfigData GetTeamConfigData(Team team)
        {
            return _teamConfigDatas.FirstOrDefault(x => x.Team == team);
        }
    }

    [Serializable]
    public class TeamConfigData
    {
        public Team Team;
        public Color RelatedColor;
    }
}
