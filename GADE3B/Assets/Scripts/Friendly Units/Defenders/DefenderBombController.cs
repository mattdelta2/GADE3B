using System;
using UnityEngine;
public class DefenderBombController : DefenderController
{
    public GameObject bombProjectilePrefab;  // The bomb projectile prefab
    public Transform shootPoint;             // Where the projectile is shot from
    public float bombExplosionRadius = 5f;   // Radius of the bomb explosion
    public float bombDamage = 50f;   
    public float speed = 10f;        // Damage dealt by the explosion

    protected override void Start()
    {
        base.Start();

        // Ensure shoot point is assigned or find it
        if (shootPoint == null)
        {
            shootPoint = transform.Find("ShootPoint");
        }
    }

    protected override void Update()
    {
        base.Update();  // Call base update to ensure base logic still works

        // Check if we have a target in range and it's time to shoot
        if (target != null && shootingTimer >= shootingInterval)
        {
            ShootBombAtTarget(target);
            shootingTimer = 0f;  // Reset shooting timer
        }
    }

    // Method to shoot a bomb projectile towards the target
    private void ShootBombAtTarget(Transform enemyTarget)
    {
        if (bombProjectilePrefab == null || shootPoint == null) return;

        // Instantiate the bomb projectile at the shoot point
        GameObject bombProjectile = Instantiate(bombProjectilePrefab, shootPoint.position, Quaternion.identity);

        // Get the projectile controller component and assign the target along with damage and speed
        BombProjectileController projectileController = bombProjectile.GetComponent<BombProjectileController>();
        if (projectileController != null)
        {
            // Pass the target, defender's damage, and speed to the projectile
            projectileController.SetTarget(enemyTarget, bombDamage, speed);
            projectileController.explosionRadius = bombExplosionRadius;  // Set explosion radius
        }
        else
        {
            Debug.LogError("No BombProjectileController found on the bomb projectile.");
        }
    }
}
