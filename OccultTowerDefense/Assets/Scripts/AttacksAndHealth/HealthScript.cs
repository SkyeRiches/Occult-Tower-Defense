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
		if (gameObject.name == "Base") {
			UIChangeInt change = GameObject.FindGameObjectsWithTag("BaseHealthText")[0].GetComponent<UIChangeInt>();
			if (change != null) {
				change.ChangeInt(Mathf.RoundToInt(currentHealth));
			}
		}
	}

	// Update is called once per frame
	void Update() {
		if (currentHealth <= 0.0f) {
			if (gameObject.tag == "Player" && gameObject.name != "Base") {
				GameObject.FindGameObjectWithTag("Managers").GetComponent<Respawn>().BeginRespawn();
				return;
			} else if (gameObject.tag == "Player" && gameObject.name == "Base") {
				//PUT GAME OVER STUFF HERE
				Debug.Log("GAME OVER!!!");
				GameObject.FindGameObjectsWithTag("Managers")[0].GetComponent<GameManager>().FailGame();
				gameObject.SetActive(false);
				return;
			} else {
				DestroyEntity();
			}
		}
	}

	private void UpdateBaseHealth() {
		baseHealth = currentHealth / currentHealthMuliplier;
		baseHealth = Mathf.Clamp(baseHealth, 0, maxBaseHealth);
		if (gameObject.name == "Base")
		{
			UIChangeInt change = GameObject.FindGameObjectsWithTag("BaseHealthText")[0].GetComponent<UIChangeInt>();
			if (change != null)
			{
				change.ChangeInt(Mathf.RoundToInt(currentHealth));
			}
		}
	}

	private void UpdateCurrentHealth() {
		currentHealth = baseHealth * currentHealthMuliplier;
		currentHealth = Mathf.Clamp(currentHealth, 0, maxBaseHealth * currentHealthMuliplier);
	}

	protected virtual void DestroyEntity() {
		Destroy(this.gameObject);
	}
	#endregion

	#region Public Access Functions (Getters and Setters)

	public void HealEntity(float a_health) {
		currentHealth += a_health;
		currentHealth = Mathf.Clamp(currentHealth, 0, maxBaseHealth * currentHealthMuliplier);
		UpdateBaseHealth();
	}

	public void HealEntityToMax() {
		currentHealth = maxBaseHealth * currentHealthMuliplier;
		UpdateBaseHealth();
	}

	/// <summary>
	/// Increases the max base health by the value passed in.
	/// </summary>
	/// <param name="a_increase"></param>
	public void IncreaseMaxHealth(float a_increase) {
		maxBaseHealth += a_increase;
		UpdateBaseHealth();
		UpdateCurrentHealth();
	}


	/// <summary>
	/// Damages the entity by the points passed in as a parameter.
	/// </summary>
	/// <param name="a_damage"></param>
	/// <returns>Returns true if the entity was killed by the attack. Returns false if the entity was not killed.</returns>
	public bool DamageEntity(float a_damage) {
		currentHealth -= a_damage;
		currentHealth = Mathf.Clamp(currentHealth, 0, maxBaseHealth * currentHealthMuliplier);
		UpdateBaseHealth();
		if (currentHealth <= 0.0f) {
			return true;

		}

		//Only returns false if the entity isn't killed.
		return false;
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

	public EntityAllignment GetAllignment() {
		return creatureAllignment;
	}
	#endregion
}

#region Enums.
public enum EntityAllignment {
	NEUTRAL = 0,
	PLAYER = 1,
	ENEMY = 2,
	TOWER = 3,
	DEFENCE_POINT = 4,
}
#endregion