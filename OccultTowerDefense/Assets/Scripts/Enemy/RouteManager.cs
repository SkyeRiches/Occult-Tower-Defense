using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RouteManager : MonoBehaviour {
	[SerializeField]
	private GameObject waypointPrefab = null;
	public Vector2[] destinations;
	public static Vector2[] destinationsForEnemies;

	private GameObject[] waypoints;

	void Awake() {
		destinationsForEnemies = destinations;
		waypoints = new GameObject[destinations.Length];
		if (waypointPrefab != null) {
			for (int i = 0; i < waypoints.Length; i++) {
				if (i >= waypoints.Length - 1) {
					break;
				}
				waypoints[i] = Instantiate(waypointPrefab, new Vector3(destinations[i].x, destinations[i].y, 0.0f), Quaternion.identity, this.transform);
				waypoints[i].name = "Waypoint[" + i + "]";
			}
		}
	}

	// Start is called before the first frame update
	void Start() {

	}

	// Update is called once per frame
	void Update() {

	}

	private void OnDrawGizmos() {
		//Draw destination gizmos.
		if (destinations.Length > 0) {
			Gizmos.color = Color.red;
			foreach (Vector2 pos in destinations) {
				Gizmos.DrawSphere(new Vector3(pos.x, pos.y, 0.0f), 0.1f);
			}
		}
	}
}
