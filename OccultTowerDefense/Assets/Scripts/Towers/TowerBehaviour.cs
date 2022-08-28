using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class TowerBehaviour : MonoBehaviour {

	public enum TOWER_STATE {
		PLACED,
		UNPLACED
	}

	public enum TOWER_TYPE {
		BASIC,
		FIRE,
		HEAL,
		BURST,
		RAILGUN,
	}

	#region Variables to assign via the unity inspector (SerializeFields).
	[SerializeField]
	private TOWER_TYPE type = TOWER_TYPE.BASIC;

	[SerializeField]
	private float placementRadius = 3f;

	[SerializeField]
	private float attackSpeed = 5.0f;

	//[SerializeField]
	//private TargetType targetType = TargetType.CLOSEST;

	[SerializeField]
	private float attackRange = 10.0f;

	[SerializeField]
	private float cooldownTime = 1.0f;

	[SerializeField]
	private float cost = 50f;

	[SerializeField]
	private GameObject rangeIndicator;
	#endregion

	#region Private Variables.

	private bool isPlaced = false;

	private bool isHealer = false;
	private string attackType;

	private bool canFire = true;
	private string targetTag = "Player";

	private float damageMultiplier = 1.0f;

	//Targeting list.
	private List<GameObject> targets = new List<GameObject>();

	private GameObject manager;

	#endregion

	#region Private Functions.

	private void Start() {
		manager = GameObject.FindGameObjectWithTag("Managers");
	}

	private void Update() {
		if (!isPlaced) {
			float range = (attackRange * 2) * 1.0f;
			rangeIndicator.transform.localScale = new Vector3(range, range, range);
			Color rangeCol = Color.grey;
			rangeCol.a = 0.25f;
			rangeIndicator.GetComponent<SpriteRenderer>().color = rangeCol;
		}
		if (canFire && isPlaced) {
			UpdateTargetsList();
			if (type == TOWER_TYPE.HEAL) {
				if (targets.Count > 0) {
					Vector2 healpos = new Vector2(gameObject.transform.position.x, gameObject.transform.position.y);
					FireStationary(healpos);
					canFire = false;
					StartCoroutine(FireCooldown());
				}
			} else if (type == TOWER_TYPE.BURST) {
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
		if (type == TOWER_TYPE.HEAL) {
			isHealer = true;
		} else {
			isHealer = false;
		}

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

		//Look at the target.
		LookAtPos(new Vector2(closest.transform.position.x, closest.transform.position.y));

		canFire = false;
		Vector2 direction = (new Vector2(closest.transform.position.x, closest.transform.position.y) - new Vector2(gameObject.transform.position.x, gameObject.transform.position.y)).normalized;
		Fire(direction);
		StartCoroutine(FireCooldown());
	}

	private void Fire(Vector2 a_dir) {
		GameObject.FindGameObjectsWithTag("Managers")[0].GetComponent<AttacksManagerScript>().SpawnAttack(attackType, a_dir.normalized * attackSpeed, this.gameObject.transform.position, this.gameObject.transform, damageMultiplier, attackRange * 1.5f);
	}

	private void FireStationary(Vector2 a_pos) {
		GameObject.FindGameObjectsWithTag("Managers")[0].GetComponent<AttacksManagerScript>().SpawnAttack(attackType, Vector2.zero, new Vector3(a_pos.x, a_pos.y, 0.0f), this.gameObject.transform, damageMultiplier, cooldownTime * 0.75f);
	}

	private void LookAtPos(Vector2 a_pos) {
		//Get vector from tower to target.
		//Vector2 dir = a_pos - new Vector2(gameObject.transform.position.x, gameObject.transform.position.y);
		//dir.Normalize();

		////Convert vector to angle.
		//float angle = 0.0f;
		//if (dir.y > 0.0f) {
		//	if (dir.x > 0.0f) {
		//		angle = Mathf.Asin(dir.x / 1.0f) * Mathf.Rad2Deg;
		//	} else {
		//		angle = 360.0f - (Mathf.Asin((0 - dir.x) / 1.0f) * Mathf.Rad2Deg);
		//	}
		//} else {
		//	if (dir.x > 0.0f) {
		//		angle = 90 + (Mathf.Asin((0 - dir.y) / 1.0f) * Mathf.Rad2Deg);
		//	} else {
		//		angle = 180 + (Mathf.Asin((0 - dir.x) / 1.0f) * Mathf.Rad2Deg);
		//	}
		//}

		////Set angle of the transform rotation z to the angle calculated.
		//Vector3 currentAngle = this.transform.rotation.eulerAngles;
		//currentAngle.z = angle;
		//this.transform.rotation = Quaternion.Euler(currentAngle * Mathf.Rad2Deg);

		//What is the difference in position?
		Vector3 diff = ((new Vector3(a_pos.x, a_pos.y, 0.0f)) - transform.position);

		//We use aTan2 since it handles negative numbers and division by zero errors. 
		float angle = Mathf.Atan2(diff.y, diff.x);

		//Now we set our new rotation. 
		transform.rotation = Quaternion.Euler(0f, 0f, angle * Mathf.Rad2Deg - 90.0f);
	}

	private IEnumerator FireCooldown() {
		yield return new WaitForSeconds(cooldownTime);
		canFire = true;
	}
	#endregion

	#region Public Access Functions.
	public void PlaceTower() {
		isPlaced = true;
		rangeIndicator.SetActive(false);

		switch (type) {
			case TOWER_TYPE.BASIC: {
					isHealer = false;
					attackType = "TowerCrystal";
					gameObject.GetComponentInChildren<SpriteRenderer>().color = Color.white;
					break;
				}
			case TOWER_TYPE.FIRE: {
					isHealer = false;
					attackType = "FireBullet";
					gameObject.GetComponentInChildren<SpriteRenderer>().color = Color.white;
					break;
				}
			case TOWER_TYPE.HEAL: {
					attackType = "HealBullet";
					isHealer = true;
					gameObject.GetComponentInChildren<SpriteRenderer>().color = Color.white;
					break;
				}
			case TOWER_TYPE.BURST: {
					attackType = "BurstBullet";
					isHealer = false;
					gameObject.GetComponentInChildren<SpriteRenderer>().color = Color.white;
					break;
				}
			case TOWER_TYPE.RAILGUN: {
					attackType = "RailgunBullet";
					isHealer = false;
					gameObject.GetComponentInChildren<SpriteRenderer>().color = Color.white;
					break;
				}
			default: {
					break;
				}
		}

		if (isHealer) {
			targetTag = "Player";
		} else {
			targetTag = "Enemy";
		}

		manager.GetComponent<Currency>().DecreaseSouls(cost);
	}

	public void EmpowerTower() {
		// increase the tower stats 
	}

	public bool GetIsPlaced() {
		return isPlaced;
	}

	public void SetIsPlaced(bool a_state) {
		isPlaced = a_state;
	}

	public TOWER_TYPE GetTowerType() {
		return type;
	}

	public float GetPlacementRadius() {
		return placementRadius;
	}

	public float GetCost() {
		return cost;
	}

	public float GetRange() {
		return attackRange;
	}
	#endregion
}
