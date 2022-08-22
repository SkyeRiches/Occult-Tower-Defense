using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Modifiers;
using Unity.Mathematics;

public class HealthScript : MonoBehaviour {
	#region Variables to assign via the unity inspector (SerialiseFields).
	[SerializeField]
	private float maxBaseHealth = 100.0f;

	[SerializeField]
	private EntityAllignment creatureAllignment = EntityAllignment.NEUTRAL;
	#endregion

	#region Private Variables.
	private float baseHealth = 0.0f;
	private float currentHealth = 0.0f;
	private float currentHealthMuliplier = 1.0f;
	#endregion

	#region Private Functions.
	// Start is called before the first frame update
	void Start() {
		baseHealth = maxBaseHealth;
		currentHealth = baseHealth * currentHealthMuliplier;
	}

	// Update is called once per frame
	void Update() {
		if (currentHealth <= 0.0f)
		{
			DestroyEntity();
		}
	}

	private void UpdateBaseHealth() {
		baseHealth = currentHealth / currentHealthMuliplier;
		baseHealth = Mathf.Clamp(baseHealth, 0, maxBaseHealth);
	}

	private void UpdateCurrentHealth() {
		currentHealth = baseHealth * currentHealthMuliplier;
		currentHealth = Mathf.Clamp(currentHealth, 0, maxBaseHealth * currentHealthMuliplier);
	}

	protected virtual void DestroyEntity()
	{
		Destroy(this.gameObject);
	}
	#endregion

	#region Public Access Functions (Getters and Setters)

	public void HealEntity(float a_health) {
		currentHealth += a_health;
		currentHealth = Mathf.Clamp(currentHealth, 0, maxBaseHealth * currentHealthMuliplier);
		UpdateBaseHealth();
	}

	public void HealEntityToMax()
	{
		currentHealth = maxBaseHealth * currentHealthMuliplier;
		UpdateBaseHealth();
	}
	
	public void DamageEntity(float a_damage) {
		currentHealth -= a_damage;
		currentHealth = Mathf.Clamp(currentHealth, 0, maxBaseHealth * currentHealthMuliplier);
		UpdateBaseHealth();
	}

	public void ApplyHealthMultiplier(float a_mult) {
		if (a_mult < 0.0f) {
			a_mult = 0.0f;
		}

		currentHealthMuliplier *= a_mult;
		UpdateCurrentHealth();
	}

	public void ResetHealthMultiplier() {
		currentHealthMuliplier = 1.0f;
		UpdateCurrentHealth();
	}

	public EntityAllignment GetAllignment()
	{
		return creatureAllignment;
	}
	#endregion
}

#region Enums.
public enum EntityAllignment {
	NEUTRAL = 0,
	PLAYER = 1,
	ENEMY = 2,
	Tower = 3,
}
#endregion