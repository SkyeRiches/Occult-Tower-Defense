using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Modifiers {
	public enum ModifierType
	{
		none = 0,
		healthMultiplier = 1,
		attackDamageMultiplier = 2,
	}

	[System.Serializable]
	public class Modifier {
		//Public Variables.
		public readonly int ID = 0;
		public ModifierType type = ModifierType.none;
		public float value = 0.0f;
		public float modifierTime = 1.0f;
		public bool useTimer = true;

		//Private Variables.
		private static int modifierCount = 0;

		//Private Functions.

		//Public Functions.
		public Modifier(ModifierType a_type, float a_value, float a_modifierTime, bool a_useModifierTime)
		{
			ID = modifierCount;
			modifierCount++;
			type = a_type;
			value = a_value;
			modifierTime = a_modifierTime;
			useTimer = a_useModifierTime;
		}

		public Modifier(Modifier a_copy)
		{
			ID = modifierCount;
			modifierCount++;
			type = a_copy.type;
			value = a_copy.value;
			modifierTime = a_copy.modifierTime;
			useTimer = a_copy.useTimer;
		}
	}
}