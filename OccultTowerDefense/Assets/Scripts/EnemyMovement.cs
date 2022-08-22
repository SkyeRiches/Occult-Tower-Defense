using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public Vector2[] destinationArray;

    private int locationInArray;

    public Vector2 enemyVelocity;

    public float neighbourhoodSize;
    public float collisionAvoidNeighbourhood;

    public Vector2 nextDestination;

    public List<GameObject> enemiesInRange = new List<GameObject>();
    public List<GameObject> obstaclesInRange = new List<GameObject>();
    public bool playerInRange;

    // velocity multipliers
    public float enemyCohesion;
    public float wallRepulsion;
    public float playerAttraction;
    public float nextDestinationAttraction;

    public float speed;

    // Start is called before the first frame update
    void Start()
    {
        locationInArray = 0;
        destinationArray = RouteManager.destinationsForEnemies;
        nextDestination = destinationArray[0];
        playerInRange = false;
    }


    void Update()
    {
        enemyVelocity.x = 0;
        enemyVelocity.y = 0;
        enemiesInRange.Clear();
        obstaclesInRange.Clear();
        Collider2D[] entitiesInRange = Physics2D.OverlapCircleAll(transform.position, neighbourhoodSize);
        foreach(Collider2D entity in entitiesInRange) {
            if (entity.gameObject.tag == "Enemy") {
                enemiesInRange.Add(entity.gameObject);
            }
            else if (entity.gameObject.tag == "Obstacle") {
                obstaclesInRange.Add(entity.gameObject);
            }
            else if (entity.gameObject.tag == "waypoint") {
                if ((entity.transform.position - transform.position).magnitude < transform.localScale.magnitude)
                if (entity.transform.position == (Vector3)destinationArray[locationInArray]) {
                    locationInArray++;
                    if (locationInArray <= destinationArray.Length) {
                        nextDestination = destinationArray[locationInArray];
                    }
                }
            }
            
        }


        enemyVelocity = (enemyVelocity + CalculateCohesionForce(enemiesInRange) * enemyCohesion);
        enemyVelocity = (enemyVelocity - CalculateFleeForce(enemiesInRange) * enemyCohesion * 2);
        enemyVelocity = (enemyVelocity + CalculateWallRepulsionForce() * wallRepulsion) ;
        enemyVelocity = (enemyVelocity + ((nextDestination - (Vector2)transform.position).normalized) * nextDestinationAttraction);

        transform.position = transform.position + ((Vector3)enemyVelocity * speed * 0.01f);



    }

    Vector2 CalculateCohesionForce(List<GameObject> enemiesWithinRange) {
        Vector3 velocity = new Vector3(0f,0f,0f);
        foreach (GameObject enemy in enemiesWithinRange) {
            Vector3 targetDirection = (enemy.transform.position - transform.position);
            if (targetDirection.magnitude > 0.0f) {
                targetDirection = targetDirection.normalized;
            }
            velocity = velocity + targetDirection;

        }
        return velocity;

    }

    Vector2 CalculateFleeForce(List<GameObject> enemiesWithinRange) {
        Vector3 velocity = new Vector3(0f, 0f, 0f);
        foreach (GameObject enemy in enemiesWithinRange) {
            Vector3 targetDirection = (enemy.transform.position - transform.position);
            if (targetDirection.magnitude > 0.0f && targetDirection.magnitude < collisionAvoidNeighbourhood) {
                targetDirection = targetDirection.normalized;
            }
            velocity = velocity + targetDirection;

        }
        return velocity;

    }

    Vector2 CalculateWallRepulsionForce() {
        // dummy float, -1 if something below, 1 if something above, 0 otherwise
        bool up = false;
        bool down = false;
        bool left = false;
        bool right = false;

        RaycastHit2D[] upTest = Physics2D.RaycastAll(transform.position, new Vector2(0,1), collisionAvoidNeighbourhood);
        RaycastHit2D[] downTest = Physics2D.RaycastAll(transform.position, new Vector2(0, -1), collisionAvoidNeighbourhood);
        RaycastHit2D[] leftTest = Physics2D.RaycastAll(transform.position, new Vector2(-1, 0), collisionAvoidNeighbourhood);
        RaycastHit2D[] rightTest = Physics2D.RaycastAll(transform.position, new Vector2(1, 0), collisionAvoidNeighbourhood);

        foreach (RaycastHit2D hit in upTest) {
            if (hit.transform.gameObject.tag == "Obstacle") {
                up = true;
            }
        }
        foreach (RaycastHit2D hit in downTest) {
            if (hit.transform.gameObject.tag == "Obstacle") {
                down = true;
            }
        }
        foreach (RaycastHit2D hit in leftTest) {
            if (hit.transform.gameObject.tag == "Obstacle") {
                left = true;
            }
        }
        foreach (RaycastHit2D hit in rightTest) {
            if (hit.transform.gameObject.tag == "Obstacle") {
                right = true;
            }
        }

        Vector2 direction = new Vector2(0, 0);
        if (up) {
            direction += new Vector2(0, -1);
        }
        if (down) {
            direction += new Vector2(0, 1);
        }
        if (left) {
            direction += new Vector2(1,0);
        }
        if (right) {
            direction += new Vector2(-1, 0);
        }

        return direction;

    }

}
