using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float fMoveSpeed = 2.0f;
    [SerializeField] private Rigidbody2D rb;
    Vector2 movement;
    Vector2 lookDir;

    PlayerControls controls;

    [SerializeField] private Transform firePoint;
    private bool doFire = false;

    private float fFireCooldown = 1f;

    private void Awake()
    {
        controls = new PlayerControls();
        controls.Gameplay.Movement.performed += ctx => movement = ctx.ReadValue<Vector2>();
        controls.Gameplay.Movement.canceled += ctx => movement = Vector2.zero;
        controls.Gameplay.Aim.performed += ctx => lookDir = ctx.ReadValue<Vector2>();
        controls.Gameplay.Aim.started += ctx => { doFire = true; };
        controls.Gameplay.Aim.canceled += ctx => { doFire = false; StopCoroutine(FireCooldown()); lookDir = Vector2.zero; };
    }

    // Update is called once per frame
    void Update()
    {
        if (lookDir != Vector2.zero)
        {
            float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg;
            angle -= 90; //basically a magic number lol

            transform.rotation = Quaternion.Euler(0,0,angle);

            if (lookDir.magnitude < 0.25f)
            {
                doFire = false;
            }
            else if (!doFire)
            {
                doFire = true;
                Fire();
            }
        }
    }

    void Fire()
    {
        if (doFire)
        {
	        GameObject.FindGameObjectsWithTag("Managers")[0].GetComponent<AttacksManagerScript>().SpawnAttack("Bullet", lookDir.normalized * 5.0f, firePoint.position, this.gameObject.transform, 0.5f);

	        StartCoroutine(FireCooldown());
        }
    }
    
    private IEnumerator FireCooldown()
    {
        yield return new WaitForSeconds(fFireCooldown);
        Fire();
    }

    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + movement * fMoveSpeed * Time.deltaTime);
    }

    private void OnEnable()
    {
        controls.Gameplay.Enable();
    }
    private void OnDisable()
    {
        controls.Gameplay.Disable();
    }
}
