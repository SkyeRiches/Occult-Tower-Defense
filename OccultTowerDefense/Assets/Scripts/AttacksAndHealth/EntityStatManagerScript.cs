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
	private static List<Modifier> globalApplyModifiers = null;
	private static List<Modifier> globalStorageModifier = null;
	private static Queue<Modifier> globalRemovalList = null;
	private List<Modifier> applyModifiers = null;
	private List<Modifier> storageModifier = null;
	private Queue<Modifier> removalList = null;
	private int lastModifierCount = 0;
	private int lastGlobalCount = 0;
	#endregion

	#region Private Functions.
	// Start is called before the first frame update
	void Start() {
		//Set up modifier utility variables.
		globalRemovalList = new Queue<Modifier>();
		globalApplyModifiers = new List<Modifier>();
		globalStorageModifier = new List<Modifier>();
		applyModifiers = new List<Modifier>();
		storageModifier = new List<Modifier>();
		removalList = new Queue<Modifier>();
		lastModifierCount = applyModifiers.Count;
		lastGlobalCount = globalApplyModifiers.Count;

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

		if (globalApplyModifiers != null) {
			if (globalApplyModifiers.Count != lastGlobalCount) {
				ApplyGlobalModifiers();
			}
		}

		if (removalList != null) {
			if (removalList.Count > 0) {
				HandleRemovals();
			}
		}

		if (globalRemovalList != null) {
			if (globalRemovalList.Count > 0) {
				HandleGlobalRemovals();
			}
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

	private void ApplyGlobalModifiers() {
		for (int i = 0; i < globalApplyModifiers.Count; i++) {
			if (globalApplyModifiers[i] == null) {
				continue;
			}

			ApplyModifier(globalApplyModifiers[i]);
			globalStorageModifier.Add(globalApplyModifiers[i]);
			globalApplyModifiers.Remove(globalApplyModifiers[i]);
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

			default: {
					break;
				}
		}
	}

	private void HandleRemovals() {
		for (int i = 0; i < removalList.Count; i++)
		{
			Modifier removalModifier = removalList.Dequeue();
			if (removalModifier == null)
			{
				continue;
			}

			UnApplyModifier(removalModifier);
			
		}
	}

	private void HandleGlobalRemovals() {
		for (int i = 0; i < globalRemovalList.Count; i++) {
			Modifier removalModifier = globalRemovalList.Dequeue();
			if (removalModifier == null) {
				continue;
			}

			UnApplyModifier(removalModifier);
		}
	}
	#endregion

	#region Public Access Functions (Getters and Setters).

	public void AddModifier(Modifier a_modifier) {
		if (applyModifiers == null) {
			applyModifiers = new List<Modifier>();
		}
		applyModifiers.Add(a_modifier);
	}

	public void RemoveModifier(Modifier a_modifier) {
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

	public static void AddGlobal(Modifier a_modifier) {
		if (globalApplyModifiers == null) {
			globalApplyModifiers = new List<Modifier>();
		}
		globalApplyModifiers.Add(a_modifier);
	}

	public static void RemoveGlobalModifier(Modifier a_modifier) {
		if (globalRemovalList == null) {
			globalRemovalList = new Queue<Modifier>();
		}
		if (globalStorageModifier == null) {
			globalStorageModifier = new List<Modifier>();
		}
		for (int i = 0; i < globalStorageModifier.Count; i++) {
			Modifier modifier = globalStorageModifier[i];
			if (modifier == null) {
				globalStorageModifier.Remove(globalStorageModifier[i]);
				continue;
			}

			if (modifier.ID == a_modifier.ID) {
				globalRemovalList.Enqueue(globalStorageModifier[i]);
				globalStorageModifier.Remove(globalStorageModifier[i]);
			}
		}
	}
	#endregion
}
