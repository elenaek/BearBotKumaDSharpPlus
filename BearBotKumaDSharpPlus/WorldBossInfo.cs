using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BearBotKumaDSharpPlus
{
    public class WorldBossInfo
    {
        public string EventId { get; set; }
        public string Name { get; set; }
        public string Waypoint { get; set; } 


        public WorldBossInfo(string eventId, string name, string waypoint)
            {
            this.EventId = eventId;
            this.Name = name;
            this.Waypoint = waypoint;
            }
    }
}
