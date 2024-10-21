using System.Collections;
using UnityEngine;

public class BombProjectileController : MonoBehaviour
{
    public float speed = 10f;             // Speed of the projectile
    public Transform target;              // Target to hit
    public float explosionRadius = 5f;    // Explosion radius
    public float damage = 50f;            // Damage dealt by the explosion
    public GameObject explosionEffect;    // Visual explosion effect, in this case we use animation

    private EnemyController targetEnemy;  // Cache for the target's EnemyController component
    private Animation explosionAnimation; // Reference to the Animation component in the child

    public void SetTarget(Transform enemyTarget, float damageValue, float projectileSpeed)
    {
        target = enemyTarget;
        targetEnemy = target.GetComponent<EnemyController>();
        damage = damageValue;
        speed = projectileSpeed;

        // Find the animation component on the child object (assuming the child is where the animation is)
        explosionAnimation = GetComponentInChildren<Animation>();

        if (explosionAnimation == null)
        {
            Debug.LogError("Explosion animation not found on child object.");
        }
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
        // Play the explosion animation if available
        if (explosionAnimation != null)
        {
            if (explosionAnimation.GetClip("Scene") != null)
            {
                explosionAnimation.Play("Scene");
                Debug.Log("Playing explosion animation 'Scene'.");
            }
            else
            {
                Debug.LogError("Explosion animation 'Scene' not found in the Animation component.");
            }
        }
        else
        {
            Debug.LogError("Explosion animation component is missing.");
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

        // Destroy the projectile after the explosion animation ends
        float animationLength = explosionAnimation.GetClip("Scene") != null
            ? explosionAnimation.GetClip("Scene").length
            : 0f;

        Destroy(gameObject, animationLength);
    }

    private void OnDrawGizmosSelected()
    {
        // Draw explosion radius for visualization
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}
