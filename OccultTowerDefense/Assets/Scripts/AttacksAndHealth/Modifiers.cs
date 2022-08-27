using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Modifiers {
	public enum ModifierType
	{
		none = 0,
		healthMultiplier = 1,
		attackDamageMultiplier = 2,
		damageOverTime = 3,
	}

	[System.Serializable]
	public class Modifier {
		//Public Variables.
		public readonly int ID = 0;
		public ModifierType type = ModifierType.none;
		public float value = 0.0f;
		public float modifierTime = 1.0f;
		private string tag = "DEFAULT";

		//Private Variables.
		private static int modifierCount = 0;

		//Private Functions.

		//Public Functions.
		public Modifier(ModifierType a_type, float a_value, float a_modifierTime, string a_tag)
		{
			ID = modifierCount;
			modifierCount++;
			type = a_type;
			value = a_value;
			modifierTime = a_modifierTime;
			tag = a_tag;
		}

		public Modifier(Modifier a_copy)
		{
			ID = modifierCount;
			modifierCount++;
			type = a_copy.type;
			value = a_copy.value;
			modifierTime = a_copy.modifierTime;
			tag = a_copy.tag;
		}
	}
}