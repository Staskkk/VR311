using Assets.Scripts;
using Mapbox.Examples;
using Mapbox.Json;
using Mapbox.Json.Linq;
using Mapbox.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class DataManagerScript : MonoBehaviour
{
    public SpawnOnMap spawnManager;

    public Color[] markerColors;

    public float[] markerWidths;

    public int clusterCount;

    public int iterationCount;

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

        float maxHeight = -1;
        var kMeansResults = new List<KMeansResults<Incident>>();
        foreach (var incidents in dicIncidents.Values)
        {
            var kMeansResult = KMeans.Cluster(incidents.ToArray(),
                clusterCount,
                iterationCount,
                (p, c) => { return Haversine.Distance(p[0], p[1], c[0], c[1]); });
            kMeansResults.Add(kMeansResult);
            float localMax = kMeansResult.Clusters.Max(el => el.Count);
            if (maxHeight < localMax)
            {
                maxHeight = localMax;
            }
        }

        int index = 0;
        foreach (var kMeansResult in kMeansResults)
        {
            var clusters = new List<Cluster>();
            foreach (var data in kMeansResult.Clusters)
            {
                clusters.Add(new Cluster { Incidents = data });
            }

            for (int i = 0; i < clusters.Count; ++i)
            {
                clusters[i].Location = new Vector2d { x = kMeansResult.Means[i][0], y = kMeansResult.Means[i][1] };
            }

            spawnManager.AddMarkers(clusters, markerColors[index], markerWidths[index], maxHeight);
            index++;
        }
    }
}
