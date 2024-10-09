using UnityEngine;

public class BombProjectileController : MonoBehaviour
{
    private Transform target;              // Target to hit
    public float explosionRadius = 5f;     // Explosion radius
    public GameObject explosionEffect;     // Visual explosion effect
    private float speed;                   // Speed of the projectile
    private float damage;                  // Damage dealt by the explosion

    // Set the target, damage, and speed for the projectile
    public void SetTarget(Transform enemyTarget, float damageValue, float speedValue)
    {
        target = enemyTarget;
        damage = damageValue;
        speed = speedValue;
    }

    void Update()
    {
        if (target == null) return;

        // Move the projectile towards the target
        Vector3 direction = (target.position - transform.position).normalized;
        float distanceThisFrame = speed * Time.deltaTime;

        // Check if we are close enough to hit the target
        if (Vector3.Distance(transform.position, target.position) <= distanceThisFrame)
        {
            Explode();
        }
        else
        {
            // Move the projectile towards the target
            transform.Translate(direction * distanceThisFrame, Space.World);
        }
    }

    private void Explode()
    {
        // Optionally, instantiate explosion effect
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
                    enemy.TakeDamage(damage);
                }
            }
        }

        // Destroy the projectile after explosion
        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        // Draw explosion radius for visualization in the editor
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}
