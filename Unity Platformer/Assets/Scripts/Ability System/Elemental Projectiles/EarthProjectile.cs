﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class EarthProjectile : ElementalProjectiles,IPooledProjectile {

	private Rigidbody _rb;
	private Vector3 _originalScale;
	private Plane _plane;				// Plane used for plane.raycast in the Shoot function
	private Vector3 _distanceFromCamera;    // Distance from camera that the plane is created at
    private AbilityInputHandler _inputHandler;

    [SerializeField] private float _baseDamage;
	[SerializeField] private float _boostedDamage;
	[SerializeField] private float _boostedKnockbackForce;
    [SerializeField] private GameObject _OnLandPE;

    // Rigidbody is set in awake
    private void Awake()
	{
        ProjectileElementalState = ElementalStates.Earth;
		_rb = GetComponent<Rigidbody>();
		_originalScale = transform.localScale;
        _inputHandler = GameObject.FindGameObjectWithTag("AbilityManager").GetComponent<AbilityInputHandler>();
    }

    public void Shoot()
	{
        _rb.useGravity = false;

		// Null check to ensure player variables are set
		if (PlayerTrans == null)
			LoadPlayerVariables();

		StartCoroutine(GravityDropOff(_rb));

        // Destroy the projectile after specified number of seconds
        Destroy(gameObject, TimeTillDestroy);

        _distanceFromCamera = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y, PlayerTrans.position.z);
		_plane = new Plane(Vector3.forward, _distanceFromCamera);

		if (AbilityManager.IsAimToShoot)
		{
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			float enter = 1000.0f;
			if (_plane.Raycast(ray, out enter))
			{
				// Set gameobjects velocity to returned value of AimToFireProjectileForce method
				_rb.velocity = AimToFireProjectileForce(ProjectileSpeed, ray, enter, PlayerTrans);
				// Debug ray to see raycast in viewport
				Debug.DrawRay(ray.origin, ray.direction * enter, Color.green, 2f);
			}
		}
	}

    private void OnCollisionEnter(Collision collision)
    {
        // Set gravity to false;
        _rb.useGravity = false;

        GameObject onLand = Instantiate(_OnLandPE, collision.contacts[0].point, Quaternion.identity);
        if (IsBoosted)
            onLand.transform.localScale *= 2.0f;

        Destroy(onLand, 1f);
        Collider col = collision.collider;
        if (col.CompareTag("Enemy"))
        {
            if (IsBoosted)
                KnockbackDamageToEnemy(_boostedDamage, _boostedKnockbackForce, transform, col);
            else
                FlatDamageToEnemy(_baseDamage, col);
        }
        // Deactive projectile and display particle effect
        // Set projectile back to normal once it hits something
        if (IsBoosted)
        {
            transform.localScale = _originalScale;
            IsBoosted = false;
        }
        Destroy(gameObject);
    }
}
