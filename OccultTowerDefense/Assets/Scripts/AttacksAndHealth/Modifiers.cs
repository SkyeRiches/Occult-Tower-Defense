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
	public class Modifier {
		//Public Variables.
		public int ID = 0;
		public ModifierType type = ModifierType.none;
		public float value = 0.0f;

		//Private Variables.
		private static int modifierCount = 0;

		//Private Functions.

		//Public Functions.
		public Modifier(ModifierType a_type, float a_value)
		{
			ID = modifierCount;
			modifierCount++;
			type = a_type;
			value = a_value;
		}
	}
}