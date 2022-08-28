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

	private int maxEntities = 5;

	[SerializeField]
	private bool isHeal = false;

	[SerializeField]
	private List<Modifier> attackModifiers;
	#endregion

	#region Private Variables.

	private Transform attackOwner;
	private HealthScript ownerHealth;
	private float damageMultiplier = 1.0f;
	#endregion

	#region Private Functions.
	// Start is called before the first frame update
	void Start() {

	}

	void Awake() {

	}

	// Update is called once per frame
	void Update() {
		if (isHeal && attackOwner != null)
		{
			gameObject.transform.position = attackOwner.position;
		}
	}

	private void OnValidate() {
		if (maxEntities <= 0) {
			maxEntities = 1;
		}
	}

	private void OnTriggerEnter2D(Collider2D other) {
		if (other.transform == null) {
			return;
		}

		if (other.transform == attackOwner) {
			return;
		}

		HealthScript otherHealthScript = other.transform.GetComponent<HealthScript>();
		if (otherHealthScript == null) {
			return;
		}

		if (otherHealthScript.GetAllignment() == EntityAllignment.TOWER) {
			return;
		}

		if (ownerHealth != null) {
			if (!(otherHealthScript.GetAllignment() == EntityAllignment.DEFENCE_POINT &&
				  ownerHealth.GetAllignment() == EntityAllignment.TOWER ||
				  otherHealthScript.GetAllignment() == EntityAllignment.DEFENCE_POINT &&
				  ownerHealth.GetAllignment() == EntityAllignment.PLAYER)) {
				if ((ownerHealth.GetAllignment() != otherHealthScript.GetAllignment() ||
					 ownerHealth.GetAllignment() == EntityAllignment.NEUTRAL) && !isHeal) {
					bool didKill = otherHealthScript.DamageEntity(attackDamage * damageMultiplier);
					if (didKill) {
						if (attackOwner.gameObject.tag == "Player" && otherHealthScript.gameObject.tag == "Enemy") {
							Progression progression = attackOwner.gameObject.GetComponent<Progression>();
							progression.IncreaseKills(1);
						}
					}
				} else if ((ownerHealth.GetAllignment() == otherHealthScript.GetAllignment() ||
							ownerHealth.GetAllignment() == EntityAllignment.NEUTRAL) && isHeal) {
					otherHealthScript.HealEntity(attackDamage * damageMultiplier);
					Debug.Log("HEALED CREATURE: " + other.gameObject.name);
				}

				EntityStatManagerScript otherStatManager = other.transform.GetComponent<EntityStatManagerScript>();
				for (int i = 0; i < attackModifiers.Count; i++) {
					ApplyModifier(otherStatManager, attackModifiers[i]);
				}
			}
		} else {
			if (otherHealthScript.GetAllignment() == EntityAllignment.ENEMY) {
				bool didKill = otherHealthScript.DamageEntity(attackDamage * damageMultiplier);
			}
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
		if (ownerHealth == null) {
			int a = 0;
		}
	}

	public void SetDamageMultiplier(float a_multiplier) {
		damageMultiplier = a_multiplier;
	}
	#endregion
}
