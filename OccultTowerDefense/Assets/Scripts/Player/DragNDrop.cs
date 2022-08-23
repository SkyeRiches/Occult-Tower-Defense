using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragNDrop : MonoBehaviour
{
    public GameObject towerPrefab;
    private GameObject objectToDrag;

    private bool hasTowerInst;
    private bool canPlace;
    private bool placedTower;
    private TowerManager towerManager;
    private SpriteRenderer towerSprite;

    private void Start()
    {
        towerManager = GameObject.FindGameObjectWithTag("Managers").GetComponent<TowerManager>();
    }

    private void OnMouseDrag()
    {
        if (!hasTowerInst)
        {
            objectToDrag = Instantiate(towerPrefab, GetMousePos(), Quaternion.identity);
            towerSprite = objectToDrag.GetComponentInChildren<SpriteRenderer>();
            hasTowerInst = true;
            placedTower = false;
        }
        objectToDrag.transform.position = GetMousePos(); 
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Mouse0) && canPlace && !placedTower)
        {
            hasTowerInst = false;
            placedTower = true;
            towerManager.towers.Add(objectToDrag);
            objectToDrag = null;
            towerSprite = null;
        }

        if (Input.GetKey(KeyCode.Mouse0) && objectToDrag && !placedTower)
        {
            objectToDrag.transform.position = GetMousePos();
        }

        CheckPos();

        ChangeTowerColor();
    }

    private void CheckPos()
    {
        if (towerManager.towers.Count > 0)
        {
            foreach (GameObject go in towerManager.towers)
            {
                if (objectToDrag != null)
                {
                    float distance = Vector2.Distance(objectToDrag.transform.position, go.transform.position);
                    if (distance < objectToDrag.GetComponent<TowerBehaviour>().placementRadius)
                    {
                        canPlace = false;
                        break;
                    }
                    else
                    {
                        canPlace = true;
                    }
                }
            }
        }
        else
        {
            canPlace = true;
        }
    }

    void ChangeTowerColor()
    {
        if (towerSprite != null)
        {
            if (canPlace)
            {
                towerSprite.color = Color.white;
            }
            else if (!canPlace)
            {
                towerSprite.color = Color.red;
            }
        }
    }

    Vector3 GetMousePos()
    {
        var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;
        return mousePos;
    }
}
