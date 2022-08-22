using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Modifiers;

public class AttackScript : MonoBehaviour {
	#region Variables to assign via the unity inspector (SerialiseFields).
	[SerializeField]
	[Range(0.0f, 100.0f)]
	private float attackDamage = 10.0f;

	[SerializeField]
	[Range(1, 100)]
	private int maxEntities = 5;

	[SerializeField]
	private List<Modifier> attackModifiers;
	#endregion

	#region Private Variables.

	private Transform attackOwner;
	private HealthScript ownerHealth;
	#endregion

	#region Private Functions.
	// Start is called before the first frame update
	void Start()
	{
		ownerHealth = new HealthScript();
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

		if (otherHealthScript.GetAllignment() == EntityAllignment.Tower)
		{
			return;
		}

		if (ownerHealth.GetAllignment() != otherHealthScript.GetAllignment() || ownerHealth.GetAllignment() == EntityAllignment.NEUTRAL) {
			otherHealthScript.DamageEntity(attackDamage);
		}

		EntityStatManagerScript otherStatManager = other.transform.GetComponent<EntityStatManagerScript>();
		for (int i = 0; i < attackModifiers.Count; i++) {
			ApplyModifier(otherStatManager, attackModifiers[i]);
		}

		maxEntities--;
		if (maxEntities <= 0) {
			this.gameObject.SetActive(false);
		}

	}

	private void ApplyModifier(EntityStatManagerScript statManager, Modifier a_modifier) {
		if (statManager == null) {
			return;
		}

		if (a_modifier == null) {
			return;
		}

		statManager.AddModifier(new Modifier(a_modifier));
	}
	#endregion

	#region Public Access Functions (Getters and Setters).

	public void SetOwner(Transform a_owner) {
		attackOwner = a_owner;
		ownerHealth = attackOwner.GetComponent<HealthScript>();
	}
	#endregion
}
