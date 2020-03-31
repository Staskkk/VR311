namespace Mapbox.Examples
{
	using UnityEngine;
	using Mapbox.Utils;
	using Mapbox.Unity.Map;
	using Mapbox.Unity.MeshGeneration.Factories;
	using Mapbox.Unity.Utilities;
	using System.Collections.Generic;
    using Assets.Scripts;

    public class SpawnOnMap : MonoBehaviour
	{
		[SerializeField]
		AbstractMap _map;

		[SerializeField]
		GameObject _markerPrefab;

		[SerializeField]
		float _widthScale;

		[SerializeField]
		float _heightScale;

		public List<MarkerScript> _spawnedObjects = new List<MarkerScript>();

		public void AddMarkers(List<Cluster> clusters, Color color, float width, float maxHeight)
		{
			foreach (var cluster in clusters)
			{
				var instance = Instantiate(_markerPrefab).GetComponent<MarkerScript>();
				instance.cluster = cluster;
				instance.transform.localPosition = _map.GeoToWorldPosition(cluster.Location, true);
				instance.SetParams(color, width, cluster.Incidents.Count / maxHeight);
				ScaleMarkers(instance);
				FixPosition(instance);
				_spawnedObjects.Add(instance);
			}
		}

		private void Update()
		{
			foreach (var spawnedObject in _spawnedObjects)
			{
				var location = spawnedObject.cluster.Location;
				spawnedObject.transform.localPosition = _map.GeoToWorldPosition(location, true);
				ScaleMarkers(spawnedObject);
				FixPosition(spawnedObject);
			}
		}

		private void FixPosition(MarkerScript spawnedObject)
		{
			spawnedObject.transform.localPosition =
				new Vector3(spawnedObject.transform.localPosition.x,
				spawnedObject.transform.localPosition.y + spawnedObject.transform.localScale.y,
				spawnedObject.transform.localPosition.z);
		}

		private void ScaleMarkers(MarkerScript spawnedObject)
		{
			spawnedObject.transform.localScale = new Vector3(
				_markerPrefab.transform.localScale.x * spawnedObject.width * _widthScale,
				_markerPrefab.transform.localScale.y * spawnedObject.height * _heightScale,
				_markerPrefab.transform.localScale.z * spawnedObject.width * _widthScale);
		}
	}
}