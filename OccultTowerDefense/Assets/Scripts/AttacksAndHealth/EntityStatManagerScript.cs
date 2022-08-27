using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Modifiers;

public class EntityStatManagerScript : MonoBehaviour {
	#region Variables to assign via the unity inspector (SerialiseField).

	#endregion

	#region Private Variables.
	//Entity script references.
	private HealthScript healthScript = null;

	//Modifier utility variables.
	private List<Modifier> applyModifiers = null;
	private List<Modifier> storageModifier = null;
	private Queue<Modifier> removalList = null;
	private int lastModifierCount = 0;
	private float damagerOverTime = 0.0f;
	#endregion

	#region Private Functions.
	// Start is called before the first frame update
	void Start() {
		//Set up modifier utility variables.
		applyModifiers = new List<Modifier>();
		storageModifier = new List<Modifier>();
		removalList = new Queue<Modifier>();
		lastModifierCount = applyModifiers.Count;

		//Get entity script references.
		healthScript = this.gameObject.GetComponent<HealthScript>();
	}

	// Update is called once per frame
	void Update() {
		if (applyModifiers != null) {
			if (applyModifiers.Count != lastModifierCount) {
				ApplyModifiers();
			}
		}

		if (removalList != null) {
			if (removalList.Count > 0) {
				HandleRemovals();
			}
		}

		if (healthScript != null) {
			healthScript.DamageEntity(damagerOverTime * Time.deltaTime);
		}
	}

	/// <summary>
	/// Actually applies the modifier to the scripts.
	/// </summary>
	private void ApplyModifier(Modifier a_modifier) {
		switch (a_modifier.type) {
			case ModifierType.none: {
					break;
				}

			case ModifierType.attackDamageMultiplier: {
					break;
				}

			case ModifierType.healthMultiplier: {
					if (healthScript != null) {
						healthScript.ApplyHealthMultiplier(a_modifier.value);
					}
					break;
				}
			case ModifierType.damageOverTime: {
					damagerOverTime += a_modifier.value;
					break;
				}
			default: {
					break;
				}
		}
	}

	/// <summary>
	/// Handles Lists.
	/// </summary>
	private void ApplyModifiers() {
		for (int i = 0; i < applyModifiers.Count; i++) {
			if (applyModifiers[i] == null) {
				continue;
			}

			ApplyModifier(applyModifiers[i]);
			storageModifier.Add(applyModifiers[i]);
			applyModifiers.Remove(applyModifiers[i]);
		}
	}

	private void UnApplyModifier(Modifier a_modifier) {
		switch (a_modifier.type) {
			case ModifierType.none: {
					break;
				}

			case ModifierType.attackDamageMultiplier: {
					break;
				}

			case ModifierType.healthMultiplier: {
					if (healthScript != null) {
						healthScript.ApplyHealthMultiplier(1 / a_modifier.value);
					}
					break;
				}
			case ModifierType.damageOverTime: {
					damagerOverTime -= a_modifier.value;
					if (damagerOverTime < 0.0f)
					{
						damagerOverTime = 0.0f;
					}
					break;
				}
			default: {
					break;
				}
		}
	}

	private void HandleRemovals() {
		for (int i = 0; i < removalList.Count; i++) {
			Modifier removalModifier = removalList.Dequeue();
			if (removalModifier == null) {
				continue;
			}

			UnApplyModifier(removalModifier);

		}
	}

	private IEnumerator RemoveTimer(float time, Modifier a_modifier) {
		yield return new WaitForSeconds(time);
		RemoveModifier(a_modifier);
	}

	private void RemoveModifier(Modifier a_modifier) {
		if (removalList == null) {
			removalList = new Queue<Modifier>();
		}
		if (storageModifier == null) {
			storageModifier = new List<Modifier>();
		}
		for (int i = 0; i < storageModifier.Count; i++) {
			Modifier modifier = storageModifier[i];
			if (modifier == null) {
				storageModifier.Remove(storageModifier[i]);
				continue;
			}

			if (modifier.ID == a_modifier.ID) {
				removalList.Enqueue(storageModifier[i]);
				storageModifier.Remove(storageModifier[i]);
			}
		}
	}

	#endregion

	#region Public Access Functions (Getters and Setters).

	public void AddModifier(Modifier a_modifier) {
		if (applyModifiers == null) {
			applyModifiers = new List<Modifier>();
		}
		applyModifiers.Add(a_modifier);
		StartCoroutine(RemoveTimer(a_modifier.modifierTime, a_modifier));
	}
	#endregion
}
