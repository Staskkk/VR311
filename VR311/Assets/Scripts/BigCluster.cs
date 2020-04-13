using Mapbox.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts
{
    public class BigCluster
    {
        public Dictionary<DateTime, List<Incident>> Incidents { get; set; }

        public Vector2d Location { get; set; }
    }
}
