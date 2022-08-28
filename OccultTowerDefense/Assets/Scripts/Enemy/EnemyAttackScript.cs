using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackScript : MonoBehaviour {
	#region Variables to assign via the unity inspector(SerializeFields).
	[SerializeField]
	[Header("Check AttackManager on the 'managers' object\nfor list of possible attacks.")]
	private string attackName = string.Empty;

	[SerializeField]
	private float attackSpeed = 5.0f;

	//[SerializeField]
	//private TargetType targetType = TargetType.CLOSEST;

	[SerializeField]
	private float attackRange = 10.0f;

	[SerializeField]
	private float cooldownTime = 1.0f;

	[SerializeField]
	private bool isHealer = false;
	#endregion

	#region Private Variables.

	private bool canFire = true;
	private string targetTag = "Player";
	private float damageMultiplier = 1.0f;

	//Targeting list.
	private List<GameObject> targets = new List<GameObject>();
	#endregion

	#region Private Functions.
	// Start is called before the first frame update
	void Start() {
		if (isHealer) {
			targetTag = "Enemy";
		} else {
			targetTag = "Player";
		}
	}

	// Update is called once per frame
	void Update() {
		if (canFire) {
			UpdateTargetsList();

			if (attackName == "Melee_Basic" || attackName == "Melee_Boss") {
				if (targets.Count > 0) {
					AimAndMelee();
				}
			} else if (attackName == "HealBullet") {
				if (targets.Count > 0) {
					Vector2 pos = new Vector2(gameObject.transform.position.x, gameObject.transform.position.y);
					FireStationary(pos);
					canFire = false;
					StartCoroutine(FireCooldown());
				}
			} else {
				AimAndFire();
			}
		}
	}

	private void OnValidate() {
		//Make sure all variables passed in via the inspector are correct.
		if (attackSpeed <= 0.0f) {
			attackSpeed = 0.1f;
		}

		if (attackRange <= 0.0f) {
			attackRange = 0.01f;
		}

		if (cooldownTime <= 0.0f) {
			cooldownTime = 0.01f;
		}
	}

	private void OnDrawGizmos() {
		Gizmos.color = Color.black;
		Gizmos.DrawWireSphere(gameObject.transform.position, attackRange);
	}

	private void UpdateTargetsList() {
		//Clear the previous list.
		targets.Clear();

		//Find all the targets in range and add them to the list.
		GameObject[] entities = GameObject.FindGameObjectsWithTag(targetTag);
		for (int i = 0; i < entities.Length; i++) {
			GameObject currentEntity = entities[i];
			if (currentEntity == this.gameObject) {
				continue;
			}

			if ((currentEntity.transform.position - gameObject.transform.position).magnitude <= attackRange) {
				targets.Add(currentEntity);
			}
		}
	}

	private void AimAndMelee()
	{
		//Get target to fire at.
		GameObject closest = null;
		for (int i = 0; i < targets.Count; i++) {
			if (closest == null) {
				closest = targets[i];
				continue;
			}

			if ((targets[i].transform.position - gameObject.transform.position).magnitude < (closest.transform.position - gameObject.transform.position).magnitude) {
				closest = targets[i];
			}
		}

		//Fire at the target if there is one.
		if (closest == null) {
			return;
		}

		canFire = false;
		Vector2 direction = (new Vector2(closest.transform.position.x, closest.transform.position.y) - new Vector2(gameObject.transform.position.x, gameObject.transform.position.y)).normalized * 1.0f;
		Vector3 pos = new Vector3(gameObject.transform.position.x + direction.x, gameObject.transform.position.y + direction.y, gameObject.transform.position.z);
		GameObject.FindGameObjectsWithTag("Managers")[0].GetComponent<AttacksManagerScript>().SpawnAttack(attackName, Vector2.zero, pos, this.gameObject.transform, damageMultiplier, 0.25f);
		StartCoroutine(FireCooldown());
	}

	private void AimAndFire() {
		//Get target to fire at.
		GameObject closest = null;
		for (int i = 0; i < targets.Count; i++) {
			if (closest == null) {
				closest = targets[i];
				continue;
			}

			if ((targets[i].transform.position - gameObject.transform.position).magnitude < (closest.transform.position - gameObject.transform.position).magnitude) {
				closest = targets[i];
			}
		}

		//Fire at the target if there is one.
		if (closest == null) {
			return;
		}

		canFire = false;
		Vector2 direction = (new Vector2(closest.transform.position.x, closest.transform.position.y) - new Vector2(gameObject.transform.position.x, gameObject.transform.position.y));
		Fire(direction);
		StartCoroutine(FireCooldown());
	}

	private void Fire(Vector2 a_dir) {
		GameObject.FindGameObjectsWithTag("Managers")[0].GetComponent<AttacksManagerScript>().SpawnAttack(attackName, a_dir.normalized * attackSpeed, this.gameObject.transform.position, this.gameObject.transform, damageMultiplier, 5.0f);
	}

	private void FireStationary(Vector2 a_pos) {
		GameObject.FindGameObjectsWithTag("Managers")[0].GetComponent<AttacksManagerScript>().SpawnAttack(attackName, Vector2.zero, new Vector3(a_pos.x, a_pos.y, 0.0f), this.gameObject.transform, damageMultiplier, cooldownTime * 0.75f);
	}

	private IEnumerator FireCooldown() {
		yield return new WaitForSeconds(cooldownTime);
		canFire = true;
	}
	#endregion

	#region Public Access Functions (Getters and Setters).

	#endregion


}
#region Enums and Structs.
public enum TargetType {
	CLOSEST = 0,
	FURTHEST = 1,
	LOWEST_HEALTH = 2,
	HIGHEST_HEALTH = 3,
}
#endregion