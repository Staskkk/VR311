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

    private class BarConfig
    {
        public Color Color { get; set; }

        public double Width { get; set; }
    }

    void Start()
    {
        var time1 = Time.realtimeSinceStartup;
        string filePath = Application.streamingAssetsPath + "/Data/data.json";
        // Load the data from the json file
        var array = JArray.Parse(File.ReadAllText(filePath));
        Debug.Log("Time1: " + (Time.realtimeSinceStartup - time1));
        var time2 = Time.realtimeSinceStartup;
        var incidents = array.ToObject<List<Incident>>();
        var sortedDates = new SortedSet<DateTime>();
        foreach (var incident in incidents)
        {
            sortedDates.Add(incident.CreatedDate.Date);
        }

        var barConfigs = new Dictionary<DateTime, BarConfig>(7);
        int confIndex = 0;
        foreach (var createdDate in sortedDates.Reverse())
        {
            if (!barConfigs.ContainsKey(createdDate))
            {
                barConfigs.Add(createdDate, new BarConfig() {
                    Color = markerColors[confIndex],
                    Width = 0.5f
                });
                confIndex++;
                if (confIndex >= markerColors.Length)
                {
                    confIndex = markerColors.Length - 1;
                }
            }
        }

        // Put data in big clusters
        var bigClusters = new Dictionary<int, BigCluster>(100);
        foreach (var incident in incidents)
        {
            if (!bigClusters.ContainsKey(incident.ZipCode))
            {
                bigClusters.Add(incident.ZipCode, new BigCluster() {
                    Incidents = new Dictionary<DateTime, List<Incident>>() { { incident.CreatedDate.Date, new List<Incident>() { incident } } },
                    Location = new Vector2d(incident.Lat, incident.Lon) });
            }
            else
            {
                var cluster = bigClusters[incident.ZipCode];
                var createdDate = incident.CreatedDate.Date;
                if (!cluster.Incidents.ContainsKey(createdDate))
                {
                    cluster.Incidents.Add(createdDate, new List<Incident>() { incident });
                }
                else
                {
                    cluster.Incidents[createdDate].Add(incident);
                }
                
                cluster.Location += new Vector2d(incident.Lat, incident.Lon);
            }
        }

        // Get max height of each clster for scaling
        float maxHeight = bigClusters.Values.Max(el => el.Incidents.Values.Max(el2 => el2.Count));

        // Get average location of each big cluster
        foreach (var bigCluster in bigClusters.Values)
        {
            long count = 0;
            foreach (var cluster in bigCluster.Incidents.Values)
            {
                count += cluster.Count;
            }

            bigCluster.Location /= count;
        }

        double radius = 0.002f;
        double radiusRatio = 1.35f;

        // Visualize all clusters in circles of big clusters
        foreach (var bigCluster in bigClusters.Values)
        {
            var index = 0;
            foreach (var cluster in bigCluster.Incidents)
            {
                var barConfig = barConfigs[cluster.Key];

                double latShift = radius * Math.Cos(index * 2 * Math.PI / bigCluster.Incidents.Count);
                double lonShift = radius * radiusRatio * Math.Sin(index * 2 * Math.PI / bigCluster.Incidents.Count);
                spawnManager.AddMarker(
                    new Cluster() {
                        Incidents = cluster.Value,
                        Location = new Vector2d(bigCluster.Location.x + latShift, bigCluster.Location.y + lonShift)
                    },
                    barConfig.Color,
                    0.5f,
                    maxHeight);
                index++;
            }
        }

        Debug.Log("Time2: " + (Time.realtimeSinceStartup - time2));


        //var kMeansResults = new List<KMeansResults<Incident>>();
        //foreach (var incidents in dicIncidents.Values)
        //{
        //    var kMeansResult = KMeans.Cluster(incidents.ToArray(),
        //        clusterCount,
        //        iterationCount,
        //        (p, c) => { return Haversine.Distance(p[0], p[1], c[0], c[1]); });
        //    kMeansResults.Add(kMeansResult);
        //    float localMax = kMeansResult.Clusters.Max(el => el.Count);
        //    if (maxHeight < localMax)
        //    {
        //        maxHeight = localMax;
        //    }
        //}

        //int index = 0;
        //foreach (var kMeansResult in kMeansResults)
        //{
        //    var clusters = new List<Cluster>();
        //    foreach (var data in kMeansResult.Clusters)
        //    {
        //        clusters.Add(new Cluster { Incidents = data });
        //    }

        //    for (int i = 0; i < clusters.Count; ++i)
        //    {
        //        clusters[i].Location = new Vector2d { x = kMeansResult.Means[i][0], y = kMeansResult.Means[i][1] };
        //    }

        //    spawnManager.AddMarkers(clusters, markerColors[index], markerWidths[index], maxHeight);
        //    index++;
        //}
    }
}
