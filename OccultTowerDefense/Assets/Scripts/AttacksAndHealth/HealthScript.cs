using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthScript : MonoBehaviour
{
	#region Variables to assign via the unity inspector (SerialiseFields).
	[SerializeField]
	private EntityAllignment creatureAllignment = EntityAllignment.NEUTRAL;
	#endregion

	#region Private Variables.

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

	#region Enums.
	public enum EntityAllignment
	{
		NEUTRAL = 0,
		PLAYER = 1,
		ENEMY = 2
	}
#endregion
}
