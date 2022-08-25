using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour {
	[SerializeField] private float fMoveSpeed = 2.0f;
	[SerializeField] private Rigidbody2D rb;
	Vector2 movement;
	Vector2 lookDir;

	PlayerControls controls;

	[SerializeField] private Transform firePoint;

	private bool doFire = true; // true when the fire cooldown has ended
	[SerializeField] private float fFireCooldown = 1f;
	private bool canFire = true; // true when the stick is being held beyond the deadzone
	[SerializeField] private float maxDamageMult = 3.0f;
	private float damageMultiplier = 1.0f;

	public void ReduceFireCooldown(float amount) {
		fFireCooldown -= amount;
	}

	public void IncreaseDamageMultiplier(float a_increase) {
		damageMultiplier += a_increase;
		damageMultiplier = Mathf.Clamp(damageMultiplier, 0.0f, maxDamageMult);
	}

	private void Awake() {
		controls = new PlayerControls();
		controls.Gameplay.Movement.performed += ctx => movement = ctx.ReadValue<Vector2>();
		controls.Gameplay.Movement.canceled += ctx => movement = Vector2.zero;
		controls.Gameplay.Aim.performed += ctx => lookDir = ctx.ReadValue<Vector2>();
		controls.Gameplay.Aim.canceled += ctx => { canFire = false; lookDir = Vector2.zero; };
	}

	// Update is called once per frame
	void Update() {
		if (lookDir != Vector2.zero) {
			float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg;
			angle -= 90; //basically a magic number lol

			transform.rotation = Quaternion.Euler(0, 0, angle);

			if (lookDir.magnitude < 0.25f) {
				canFire = false;
			} else {
				canFire = true;
				Fire();
			}
		}
	}

	void Fire() {
		if (doFire && canFire) {
			GameObject.FindGameObjectsWithTag("Managers")[0].GetComponent<AttacksManagerScript>().SpawnAttack("Bullet", lookDir.normalized * 5.0f, firePoint.position, this.gameObject.transform, damageMultiplier, 0.5f);
			doFire = false;
			StartCoroutine(FireCooldown());
		}
	}

	private IEnumerator FireCooldown() {
		yield return new WaitForSeconds(fFireCooldown);
		doFire = true;
	}

	private void FixedUpdate() {
		rb.MovePosition(rb.position + movement * fMoveSpeed * Time.deltaTime);
	}

	private void OnEnable() {
		controls.Gameplay.Enable();
	}
	private void OnDisable() {
		controls.Gameplay.Disable();
	}
}
