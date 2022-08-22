using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackScript : MonoBehaviour {
	#region Variables to assign via the unity inspector (SerialiseFields).
	[SerializeField]
	[Range(0.0f, 100.0f)]
	private float attackDamage = 10.0f;

	[SerializeField]
	[Range(1, 100)]
	private int maxEntities = 5;
	#endregion

	#region Private Variables.

	private Transform attackOwner;
	private HealthScript ownerHealth;
	#endregion

	#region Private Functions.
	// Start is called before the first frame update
	void Start() {

	}

	// Update is called once per frame
	void Update() {

	}

	private void OnTriggerEnter2D(Collider2D other) {
		if (other.transform == attackOwner) {
			return;
		}

		HealthScript otherHealthScript = other.transform.GetComponent<HealthScript>();
		if (otherHealthScript == null) {
			return;
		}

		if (ownerHealth.GetAllignment() != otherHealthScript.GetAllignment() || ownerHealth.GetAllignment() == EntityAllignment.NEUTRAL) {
			otherHealthScript.DamageEntity(attackDamage);
		}

		maxEntities--;
		if (maxEntities <= 0)
		{
			this.gameObject.SetActive(false);
		}

	}
	#endregion

	#region Public Access Functions (Getters and Setters).

	public void SetOwner(Transform a_owner) {
		attackOwner = a_owner;
		ownerHealth = attackOwner.GetComponent<HealthScript>();
	}
	#endregion
}
