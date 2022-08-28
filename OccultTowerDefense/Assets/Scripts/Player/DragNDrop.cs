using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragNDrop : MonoBehaviour {
	public GameObject towerPrefab;
	private GameObject objectToDrag;

	private bool hasTowerInst;
	private bool canPlace;
	private bool placedTower;
	private bool empowerTower;
	private GameObject manager;
	private Currency currencyManager;
	private TowerManager towerManager;
	private SpriteRenderer towerSprite;

	private void Start() {
		manager = GameObject.FindGameObjectWithTag("Managers");
		towerManager = manager.GetComponent<TowerManager>();
		currencyManager = manager.GetComponent<Currency>();
	}

	private void OnMouseDrag() {
		if (!CheckCost()) {
			return;
		}

		if (!hasTowerInst) {
			objectToDrag = Instantiate(towerPrefab, GetMousePos(), Quaternion.identity);
			towerSprite = objectToDrag.GetComponentInChildren<SpriteRenderer>();
			hasTowerInst = true;
			placedTower = false;
		}
		objectToDrag.transform.position = GetMousePos();
	}


	private void Update() {
		if (Input.GetKeyUp(KeyCode.Mouse0) && canPlace && !placedTower) {
			hasTowerInst = false;
			placedTower = true;

			towerManager.towers.Add(objectToDrag);

			if (objectToDrag != null && objectToDrag.GetComponent<TowerBehaviour>() != null) {
				objectToDrag.GetComponent<TowerBehaviour>().PlaceTower();

				if (empowerTower) {
					objectToDrag.GetComponent<TowerBehaviour>().EmpowerTower();
				}
			}
			else if(objectToDrag != null)
			{
				manager.GetComponent<Currency>().DecreaseSouls(50.0f);
			}

			objectToDrag = null;
			towerSprite = null;
		}

		if (objectToDrag && !placedTower) {
			objectToDrag.transform.position = GetMousePos();
		}

		CheckPos();

		ChangeTowerColor();
	}


	private bool IsPlacementvalid() {
		RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

		if (hit.collider == null) {
			return false;
		}

		if (hit.collider.gameObject.tag != "Obstacle") {
			return false;
		}

		//Only gets here if the placement is valid.
		return true;
	}

	private bool CheckCost() {
		float cost = 50.0f;
		if (towerPrefab.GetComponent<TowerBehaviour>() != null) {
			cost = towerPrefab.GetComponent<TowerBehaviour>().GetCost();
		}
		if (currencyManager.GetNumSouls() < cost) {
			return false;
		} else {
			return true;
		}
	}

	private void CheckPos() {
		// Check if the tower is too close to another tower
		if (towerManager.towers.Count > 0) {
			foreach (GameObject go in towerManager.towers) {
				if (objectToDrag != null && go != null) {
					float distance = Vector2.Distance(objectToDrag.transform.position, go.transform.position);
					float placeRad = 1.0f;
					if (go.GetComponent<TowerBehaviour>() != null) {
						placeRad = go.GetComponent<TowerBehaviour>().GetPlacementRadius();
					}
					if (distance < placeRad) {
						canPlace = false;
						break;
					} else {
						canPlace = true;
					}
				}
			}
		} else {
			canPlace = true;
		}

		if (!IsPlacementvalid())
		{
			canPlace = false;
		}

		// Check if the tower is being placed on a pool of power
		if (towerManager.powerPools.Count > 0) {
			foreach (GameObject go in towerManager.powerPools) {
				if (objectToDrag != null && go != null) {
					float distance = Vector2.Distance(objectToDrag.transform.position, go.transform.position);
					if (distance < go.GetComponent<PowerPool>().placementRadius) {
						empowerTower = true;
						break;
					} else {
						empowerTower = false;
					}
				}
			}
		} else {
			empowerTower = false;
		}
	}

	void ChangeTowerColor() {
		if (towerSprite != null) {
			if (canPlace) {
				towerSprite.color = Color.white;
			} else if (!canPlace) {
				towerSprite.color = Color.red;
			}

			if (empowerTower && canPlace) {
				towerSprite.color = Color.magenta;
			}
		}
	}

	Vector3 GetMousePos() {
		var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		mousePos.z = 0;
		return mousePos;
	}
}
