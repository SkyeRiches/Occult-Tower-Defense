using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttacksManagerScript : MonoBehaviour {
	#region Variables to assign via the unity inspector (SerialiseFields).
	[SerializeField]
	private List<Attacks> attackPrefabs = null;
	#endregion

	#region Private Variables.

	private List<Transform> attacks = null;
	#endregion

	#region Private Functions.
	// Start is called before the first frame update
	void Start()
	{
		attacks = new List<Transform>();
	}

	// Update is called once per frame
	void Update() {
		if (Input.GetMouseButton(0))
		{
			Debug.Log("pog");
		}
	}

	private Attacks GetAttackOfName(string name)
	{
		for (int i = 0; i < attackPrefabs.Count; i++)
		{
			if (attackPrefabs[i].name == name)
			{
				return attackPrefabs[i];
			}
		}

		//Only returns null if not valid attack found.
		Attacks notValid;
		notValid.prefab = null;
		notValid.name = string.Empty;
		return notValid;
	}

	private IEnumerator AttackCooldown(float time, Transform objectToDestroy)
	{
		yield return new WaitForSecondsRealtime(time);
		Destroy(objectToDestroy.gameObject);
	}
	#endregion

	#region Public Access Functions (Getters and Setters).

	public void SpawnAttack(string attackName, Vector2 v2Velocity, Vector3 firePoint, Transform owner, float attackCooldownTime) {
		if (owner == null)
		{
			Debug.LogError("Owner of attack is null in SpawnAttack() function of 'AttackManagerScript.cs'");
			return;
		}

		Attacks attack = GetAttackOfName(attackName);
		if (attack.prefab == null)
		{
			Debug.LogError("Attack attempted to be spawned does not exist in AttackManagerScript.cs");
			return;
		}

		if (attack.prefab.GetComponent<Rigidbody2D>() == null)
		{
			Debug.LogError("ERROR: RigidBody2D not attached to prefab = " + attack.prefab.name);
			return;
		}

		//Spawn the attack and make it move in the direction passed.
		Transform attackObject = Instantiate(attack.prefab, firePoint, Quaternion.identity);
		attackObject.parent = this.transform;
		Rigidbody2D rb = attackObject.GetComponent<Rigidbody2D>();
		rb.velocity = v2Velocity;

		//Set the owner of this attack to the transform.
		attackObject.GetComponent<AttackScript>().SetOwner(owner);

		//Start Destruction cooldown.
		StartCoroutine(AttackCooldown(attackCooldownTime, attackObject));
	}
	#endregion

	#region Structs.
	[System.Serializable]
	private struct Attacks {
		public Transform prefab;
		public string name;
	}

	#endregion

	#region Unity On Functions.

	private void OnValidate() {
		//Ensure all attacks are valid.
		for (int i = 0; i < attackPrefabs.Count; i++) {
			if (attackPrefabs[i].prefab == null) {
				continue;
			}

			if (attackPrefabs[i].prefab.gameObject.GetComponent<AttackScript>() == null) {
				Debug.LogError("ERROR: AttackScript not attached to prefab = " + attackPrefabs[i].prefab.name);
				attackPrefabs.Remove(attackPrefabs[i]);
				continue;
			}

			if (attackPrefabs[i].prefab.gameObject.GetComponent<Rigidbody2D>() == null) {
				Debug.LogError("ERROR: RigidBody2D not attached to prefab = " + attackPrefabs[i].prefab.name);
				attackPrefabs.Remove(attackPrefabs[i]);
				continue;
			}

			if (attackPrefabs[i].name == string.Empty) {
				Attacks attack = attackPrefabs[i];
				attack.name = attack.prefab.name;
				attackPrefabs[i] = attack;
			}
		}
	}
	#endregion
}
