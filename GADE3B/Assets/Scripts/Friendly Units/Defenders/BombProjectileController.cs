using System.Collections;
using UnityEngine;

public class BombProjectileController : MonoBehaviour
{
    public float speed = 10f;             // Speed of the projectile
    public Transform target;              // Target to hit
    public float explosionRadius = 5f;    // Explosion radius
    public float damage = 50f;            // Damage dealt by the explosion
    public GameObject explosionEffect;    // Visual explosion effect

    private EnemyController targetEnemy;  // Cache for the target's EnemyController component

    public void SetTarget(Transform enemyTarget, float damageValue, float projectileSpeed)
    {
        target = enemyTarget;
        targetEnemy = target.GetComponent<EnemyController>();
        damage = damageValue;
        speed = projectileSpeed;
    }

    void Update()
    {
        // If the target is null or the enemy is dead, destroy the projectile
        if (target == null || (targetEnemy != null && targetEnemy.IsDead()))
        {
            Destroy(gameObject);  // Destroy the projectile if the target is gone or dead
            return;
        }

        // Move towards the target
        Vector3 direction = (target.position - transform.position).normalized;
        float distanceThisFrame = speed * Time.deltaTime;

        // Check if we are close enough to hit the target
        if (Vector3.Distance(transform.position, target.position) <= distanceThisFrame)
        {
            Explode();  // Call the explosion when the projectile reaches the target
        }
        else
        {
            // Move the projectile towards the target
            transform.Translate(direction * distanceThisFrame, Space.World);
        }
    }

    private void Explode()
    {
        // Instantiate explosion effect if available
        if (explosionEffect != null)
        {
            Instantiate(explosionEffect, transform.position, Quaternion.identity);
        }

        // Damage all enemies within the explosion radius
        Collider[] hitEnemies = Physics.OverlapSphere(transform.position, explosionRadius);
        foreach (Collider enemyCollider in hitEnemies)
        {
            if (enemyCollider.CompareTag("Enemy"))
            {
                EnemyController enemy = enemyCollider.GetComponent<EnemyController>();
                if (enemy != null)
                {
                    enemy.TakeDamage(damage);  // Apply damage to enemies within range
                }
            }
        }

        // Destroy the projectile after the explosion
        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        // Draw explosion radius for visualization
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}
