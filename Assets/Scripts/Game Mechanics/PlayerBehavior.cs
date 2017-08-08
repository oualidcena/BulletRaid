﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class PlayerBehavior : Ship {

	public GameObject bulletPrefab;
	private float playerVelocity;
	private float bulletVelocity;
	private Camera cam;
	private Burst currentBurst = new Burst();
	private int cooldown = 1;
	private Boundary bounds;

	// Use this for initialization
	void Start () {
		playerVelocity = 2f;
		bulletVelocity = 3f;
		cam = Camera.main;
		SetBounds(new Boundary(
			cam.ScreenToWorldPoint(new Vector3(0,0,transform.position.z)),
			cam.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, transform.position.z)),
			0.1f
		));
		SetPosition(transform.position);
	}


	// Update is called once per frame
	void Update () {
		MovePlayer();
		HandleBurst();
		Shoot();
	}

	// Is triggered when player rigidbody collides with any other rigidbody
	void OnTriggerEnter2D(Collider2D col) {
		Destroy(gameObject);
	}

	// Helper Functions
	void Shoot() {
		DecrementCooldown();
		if (Input.GetButton("Fire1") && ReadyToFire()) {
			float camDis = cam.transform.position.z;
			Vector3 mouse = cam.ScreenToWorldPoint (new Vector3 (Input.mousePosition.x, Input.mousePosition.y, camDis));
			mouse = new Vector3(mouse.x, mouse.y, 0);
			FireBurst(mouse, bulletPrefab);
		}
	}

	void MovePlayer() {
		if (Input.GetKey(KeyCode.LeftShift)) {playerVelocity = 1f;}
		float yPos = (Input.GetAxis ("Vertical"));
		float xPos = (Input.GetAxis ("Horizontal"));
		Vector3 movementVector = (new Vector3 (xPos, yPos, 0));
		movementVector.Normalize();
		Move(movementVector * playerVelocity * Time.deltaTime);
		transform.position = GetPosition();

		playerVelocity = 2f;
	}

	void HandleBurst() {
		for (int i = 1; i < 10; i++) {
			if (Input.GetKeyDown(i.ToString())) {
				currentBurst = new Burst(i);
			}
		}
	}


	void OnDestroy() {
		SceneManager.LoadScene("Main Menu");
	}
}
