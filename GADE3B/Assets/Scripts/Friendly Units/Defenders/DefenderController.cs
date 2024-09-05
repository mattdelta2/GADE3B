using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefenderController : MonoBehaviour
{
    public float range = 15f; // Range at which the defender can shoot
    public float damage = 20f; // Damage dealt by the defender's projectiles
    public float shootingInterval = 1.5f; // Time between shots
    public GameObject projectilePrefab; // Projectile prefab to be shot at enemies

    private float shootingTimer = 0f;
    public Transform target; // Current enemy target

    private void Update()
    {
        shootingTimer += Time.deltaTime;
        if (shootingTimer >= shootingInterval)
        {
            ShootAtEnemy();
            shootingTimer = 0f; // Reset the timer after shooting
        }

        FindClosestEnemy(); // Continuously search for enemies
    }

    private void ShootAtEnemy()
    {
        if (target != null && projectilePrefab != null)
        {
            // Create the projectile and launch it at the enemy
            GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
            ProjectileController projectileController = projectile.GetComponent<ProjectileController>();
            if (projectileController != null)
            {
                projectileController.SetTarget(target);
            }
        }
    }

    private void FindClosestEnemy()
    {
        Collider[] hitEnemies = Physics.OverlapSphere(transform.position, range);
        float closestDistance = Mathf.Infinity;
        Transform closestEnemy = null;

        foreach (Collider enemyCollider in hitEnemies)
        {
            if (enemyCollider.CompareTag("Enemy"))
            {
                float distanceToEnemy = Vector3.Distance(transform.position, enemyCollider.transform.position);
                if (distanceToEnemy < closestDistance)
                {
                    closestDistance = distanceToEnemy;
                    closestEnemy = enemyCollider.transform;
                }
            }
        }

        target = closestEnemy;
    }
}



