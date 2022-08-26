using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WaveUtility;

public class WaveManagerScript : MonoBehaviour {
	#region Variables to assign via the unity inspector (SerializeFields).
	[SerializeField]
	private Transform spawnPoint = null;

	[SerializeField]
	private float spawnRateLowerRange = 0.1f;

	[SerializeField]
	private float spawnRateUpperRange = 1.0f;

	[SerializeField]
	private List<Wave> gameWaves = null;
	#endregion

	#region Private Variables.

	private bool waveStarted = false;

	private Wave currentWave = null;
	private bool waveOver = false;
	private bool spawnCooldown = false;
	#endregion

	#region Private Functions.
	// Start is called before the first frame update
	void Start() {
		//StartNextWave();
	}

	// Update is called once per frame
	void Update() {
		if (gameWaves != null) {
			if (gameWaves.Count <= 0 && GameObject.FindGameObjectsWithTag("Enemy").Length <= 0) {
				//GAME IS WON, PUT GAME WON CODE HERE
				Debug.Log("GAME WON!!!");
				return;

			}
		}
		if (waveStarted && currentWave != null) {
			if (!spawnCooldown) {
				SpawnEnemy(currentWave.GetNextEnemey());
				spawnCooldown = true;
				StartCoroutine(SpawnCooldown());
			}

			if (currentWave.IsWaveOver()) {
				//Finish the wave.
				currentWave = null;
				waveStarted = false;
			}
		}
	}

	private void SpawnEnemy(WaveInfo a_enemy) {
		if (a_enemy.enemyCount <= 0) {
			return;
		}

		if (a_enemy.enemyPrefab == null) {
			return;
		}

		GameObject enemyToSpawn = Instantiate(a_enemy.enemyPrefab, spawnPoint.position, spawnPoint.rotation, spawnPoint);

	}

	private IEnumerator SpawnCooldown() {
		float cooldownTime = Random.Range(spawnRateLowerRange, spawnRateUpperRange);
		yield return new WaitForSeconds(cooldownTime);
		spawnCooldown = false;
	}
	#endregion

	#region Public Access Functions (Getters and Setters).

	public void StartNextWave() {
		if (gameWaves == null) {
			Debug.LogError("No valid waves available.");
			return;
		}

		if (gameWaves.Count <= 0) {
			return;
		}

		if (!waveStarted) {
			currentWave = gameWaves[0];
			gameWaves.Remove(currentWave);
			waveStarted = true;
		}
	}

	/// <summary>
	/// Returns true if there's a wave in progress.
	/// </summary>
	/// <returns></returns>
	public bool IsWaveInProgress() {
		return waveStarted;
	}
	#endregion

	#region Validation
	private void OnValidate() {
		ValidateWaves();
	}

	/// <summary>
	/// Loops through all the game waves and removes a wave
	/// from the list and throws and error if it is not valid.
	/// </summary>
	private void ValidateWaves() {
		//Check each item in the queue and remove it from the actual list
		//if it's not valid and throw and error.
		for (int i = 0; i < gameWaves.Count; i++) {
			//Check the wave to see if it's valid.
			gameWaves[i].ValidateWave();
		}
	}
	#endregion
}
