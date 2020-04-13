using Mapbox.Json;
using Mapbox.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts
{
    public class Incident
    {
        [JsonProperty("created_date")]
        public DateTime CreatedDate { get; set; }

        [JsonProperty("latitude")]
        [KMeansValue]
        public double Lat { get; set; }

        [JsonProperty("longitude")]
        [KMeansValue]
        public double Lon { get; set; }

        [JsonProperty("incident_zip")]
        [KMeansValue]
        public int ZipCode { get; set; }
    }
}
