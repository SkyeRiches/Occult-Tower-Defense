using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Modifiers;

public class EntityStatManagerScript : MonoBehaviour {
	#region Variables to assign via the unity inspector (SerialiseField).

	#endregion

	#region Private Variables.

	private static List<Modifier> globalModifiers = null;
	private static Queue<Modifier> globalRemovalList = null;
	private List<Modifier> currentModifiers = null;
	private Queue<Modifier> removalList = null;
	private int lastModifierCount = 0;
	private int lastGlobalCount = 0;
	#endregion

	#region Private Functions.
	// Start is called before the first frame update
	void Start() {
		lastModifierCount = currentModifiers.Count;
		lastGlobalCount = globalModifiers.Count;
	}

	// Update is called once per frame
	void Update() {
		if (currentModifiers != null) {
			if (currentModifiers.Count != lastModifierCount) {
				UpdateModifiers();
			}
		}

		if (globalModifiers != null) {
			if (globalModifiers.Count != lastGlobalCount) {
				UpdateGlobalModifiers();
			}
		}

		if (removalList != null)
		{
			if (removalList.Count > 0)
			{
				HandleRemovals();
			}
		}

		if (globalRemovalList != null)
		{
			if (globalRemovalList.Count > 0)
			{
				HandleGlobalRemovals();
			}
		}
	}

	private void UpdateModifiers() {

	}

	private void UpdateGlobalModifiers() {

	}

	private void HandleRemovals()
	{

	}

	private void HandleGlobalRemovals()
	{

	}
	#endregion

	#region Public Access Functions (Getters and Setters).

	public void AddModifier(Modifier a_modifier) {
		if (currentModifiers == null) {
			currentModifiers = new List<Modifier>();
		}
		currentModifiers.Add(a_modifier);
	}

	public void RemoveModifier(Modifier a_modifier) {
		if (removalList == null) {
			removalList = new Queue<Modifier>();
		}
		if (currentModifiers == null) {
			currentModifiers = new List<Modifier>();
		}
		for (int i = 0; i < currentModifiers.Count; i++) {
			Modifier modifier = currentModifiers[i];
			if (modifier == null) {
				currentModifiers.Remove(currentModifiers[i]);
				continue;
			}

			if (modifier.ID == a_modifier.ID) {
				removalList.Enqueue(currentModifiers[i]);
				currentModifiers.Remove(currentModifiers[i]);
			}
		}
	}

	public static void AddGlobal(Modifier a_modifier) {
		if (globalModifiers == null) {
			globalModifiers = new List<Modifier>();
		}
		globalModifiers.Add(a_modifier);
	}

	public static void RemoveGlobalModifier(Modifier a_modifier) {
		if (globalRemovalList == null) {
			globalRemovalList = new Queue<Modifier>();
		}
		if (globalModifiers == null) {
			globalModifiers = new List<Modifier>();
		}
		for (int i = 0; i < globalModifiers.Count; i++) {
			Modifier modifier = globalModifiers[i];
			if (modifier == null) {
				globalModifiers.Remove(globalModifiers[i]);
				continue;
			}

			if (modifier.ID == a_modifier.ID) {
				globalRemovalList.Enqueue(globalModifiers[i]);
				globalModifiers.Remove(globalModifiers[i]);
			}
		}
	}
	#endregion
}
