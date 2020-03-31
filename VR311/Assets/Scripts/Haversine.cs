using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts
{
    public static class Haversine
    {
        public static float Distance(Location l1, Location l2)
        {
            return Distance(l1.Lat, l1.Lon, l2.Lat, l2.Lon);
        }

        public static float Distance(float lat1, float lon1, float lat2, float lon2)
        {
            var R = 6372.8f; // In kilometers
            var dLat = ToRadians(lat2 - lat1);
            var dLon = ToRadians(lon2 - lon1);
            lat1 = ToRadians(lat1);
            lat2 = ToRadians(lat2);

            var a = Mathf.Sin(dLat / 2) * Mathf.Sin(dLat / 2) + Mathf.Sin(dLon / 2) * Mathf.Sin(dLon / 2) * Mathf.Cos(lat1) * Mathf.Cos(lat2);
            var c = 2 * Mathf.Asin(Mathf.Sqrt(a));
            return R * 2 * Mathf.Asin(Mathf.Sqrt(a));
        }

        private static float ToRadians(float angle)
        {
            return angle * Mathf.Deg2Rad;
        }
    }
}