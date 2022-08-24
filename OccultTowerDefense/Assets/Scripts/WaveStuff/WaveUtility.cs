using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WaveUtility {
	//Classes.
	[System.Serializable]
	public class Wave {
		//Private Variables.
		[SerializeField]
		private List<WaveInfo> possibleEnemies = new List<WaveInfo>();

		//Private Functions.
		private bool ValidateEnemy(WaveInfo enemyInfo) {
			//Get the prefab.
			GameObject prefab = enemyInfo.enemyPrefab;

			//Check the prefab is valid.
			if (prefab != null) {

				//Check the tag is correct.
				if (prefab.tag != "Enemy") {
					Debug.LogError(
						"Enemy prefab is not tagged as Enemy.\nPlease fix this before attempting to add this enemy to the wave.");
					return false;
				}

				//Check the enemy has the correct components.
				if (prefab.GetComponent<EnemyMovement>() == null) {
					Debug.LogError(
						"Enemy prefab does not have 'EnemyMovement' script attached.\nPlease attach this script to the prefab before adding it to the wave.");
					return false;
				}

				if (prefab.GetComponent<Collider2D>() == null) {
					Debug.LogError(
						"Enemy prefab does not have 'Collider2D' attached.\nPlease attach this component to the prefab before adding it to the wave.");
					return false;
				}

				if (prefab.GetComponent<HealthScript>() == null) {
					Debug.LogError(
						"Enemy prefab does not have 'HealthScript' script attached.\nPlease attach this script to the prefab before adding it to the wave.");
					return false;
				}

				if (prefab.GetComponent<EnemyAttackScript>() == null) {
					Debug.LogError(
						"Enemy prefab does not have 'EnemyAttackScript' script attached.\nPlease attach this script to the prefab before adding it to the wave.");
					return false;
				}
			}

			//Only gets to this point if the enemy is actually valid.
			return true;
		}

		//Public Access Functions.
		public Wave() {

		}

		/// <summary>
		/// Returns true if the wave is valid.
		/// Returns false if the wave is not valid.
		/// </summary>
		/// <param name="a_wave"></param>
		/// <returns></returns>
		public void ValidateWave() {
			//Fill up the queue to be checked..
			Queue<WaveInfo> validationQueue = new Queue<WaveInfo>();
			for (int i = 0; i < possibleEnemies.Count; i++) {
				validationQueue.Enqueue(possibleEnemies[i]);
			}

			//Loop through the wave info for this wave and make sure each wave info is valid.
			for (int i = 0; i < validationQueue.Count; i++) {
				WaveInfo info = validationQueue.Dequeue();
				//Check the enemy prefab to make sure it is valid.
				//If it is not valid remove it from the list and throw an error.
				if (!ValidateEnemy(info)) {
					possibleEnemies.Remove(info);
					continue;
				}

				//Check the enemy count.
				if (info.enemyCount <= 0) {
					//Don't need to remove for this just set it to a minimum of 1.
					WaveInfo newInfo = possibleEnemies[i];
					newInfo.enemyCount = 1;
					possibleEnemies[i] = newInfo;
				}
			}
		}
	}

	//Structs.
	[System.Serializable]
	public struct WaveInfo {
		public int enemyCount;
		public GameObject enemyPrefab;
	}
}