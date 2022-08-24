using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WaveUtility;

public class WaveManagerScript : MonoBehaviour {
	#region Variables to assign via the unity inspector (SerializeFields).
	[SerializeField]
	private Transform spawnPoint = null;

	[SerializeField]
	private List<Wave> gameWaves = null;
	#endregion

	#region Private Variables.

	private bool waveOver = false;
	#endregion

	#region Private Functions.
	// Start is called before the first frame update
	void Start() {

	}

	// Update is called once per frame
	void Update() {

	}
	#endregion

	#region Public Access Functions (Getters and Setters).

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
