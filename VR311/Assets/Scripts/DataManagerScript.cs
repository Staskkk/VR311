using Assets.Scripts;
using Mapbox.Json;
using Mapbox.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class DataManagerScript : MonoBehaviour
{
    void Start()
    {
        string filePath = Application.dataPath + "/Data/data.json";
        JArray array = JArray.Parse(File.ReadAllText(filePath));
        Dictionary<DateTime, List<Incident>> dicIncidents = new Dictionary<DateTime, List<Incident>>();
        foreach (var incident in array.ToObject<IEnumerable<Incident>>())
        {
            var incDate = incident.CreatedDate.Date;
            if (!dicIncidents.ContainsKey(incDate))
            {
                dicIncidents.Add(incDate, new List<Incident>(new Incident[] { incident }));
            }
            else
            {
                dicIncidents[incDate].Add(incident);
            }
        }

        var kMeansResults = KMeans.Cluster(dicIncidents.First().Value.ToArray(), 
            100, 
            30, 
            (p, c) => { return Haversine.Distance((float)p[0], (float)p[1], (float)c[0], (float)c[1]); });

        var clusters = new List<Cluster>();
        foreach (var data in kMeansResults.Clusters)
        {
            clusters.Add(new Cluster { Incidents = data.ToList() });
        }

        for (int i = 0; i < clusters.Count; ++i)
        {
            clusters[i].Location = new Location { Lat = (float)kMeansResults.Means[i][0], Lon = (float)kMeansResults.Means[i][1] };
        }

        Debug.Log("ok");
    }

    void Update()
    {
        
    }
}
