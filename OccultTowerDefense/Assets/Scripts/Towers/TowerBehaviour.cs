using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerBehaviour : MonoBehaviour {

	public enum TOWER_STATE {
		PLACED,
		UNPLACED
	}

	public enum TOWER_TYPE {
		BASIC,
		FIRE,
		HEAL
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
	#endregion

	#region Private Variables.

	private bool isPlaced = false;

	private bool isHealer = false;
	private string attackType;

	private bool canFire = true;
	private string targetTag = "Player";

	//Targeting list.
	private List<GameObject> targets = new List<GameObject>();

	private GameObject manager;

	#endregion

	#region Private Functions.

	private void Start() 
	{
		manager = GameObject.FindGameObjectWithTag("Managers");
	}

	private void Update() {

		if (canFire && isPlaced) {
			UpdateTargetsList();
			AimAndFire();
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

		canFire = false;
		Vector2 direction = (new Vector2(closest.transform.position.x, closest.transform.position.y) - new Vector2(gameObject.transform.position.x, gameObject.transform.position.y)).normalized;
		Fire(direction);
		StartCoroutine(FireCooldown());
	}

	private void Fire(Vector2 a_dir) {
		GameObject.FindGameObjectsWithTag("Managers")[0].GetComponent<AttacksManagerScript>().SpawnAttack(attackType, a_dir.normalized * attackSpeed, this.gameObject.transform.position, this.gameObject.transform, 5.0f);
	}

	private IEnumerator FireCooldown() {
		yield return new WaitForSeconds(cooldownTime);
		canFire = true;
	}
	#endregion

	#region Public Access Functions.
	public void PlaceTower() {
		isPlaced = true;

		switch (type) {
			case TOWER_TYPE.BASIC: {
					isHealer = false;
					attackType = "Bullet";
					gameObject.GetComponentInChildren<SpriteRenderer>().color = Color.white;
					break;
				}
			case TOWER_TYPE.FIRE: {
					isHealer = false;
					attackType = string.Empty;
					gameObject.GetComponentInChildren<SpriteRenderer>().color = Color.Lerp(Color.yellow, Color.red, 0.5f);
					break;
				}
			case TOWER_TYPE.HEAL: {
					attackType = "HealBullet";
					isHealer = true;
					gameObject.GetComponentInChildren<SpriteRenderer>().color = Color.red;
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

	public float GetCost()
    {
		return cost;
    }
	#endregion
}
