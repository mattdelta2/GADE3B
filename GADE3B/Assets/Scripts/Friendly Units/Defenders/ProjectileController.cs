using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    public float speed = 10f;
    private Transform target;
    public float damage = 10f;  // Damage dealt by the projectile
    public Transform bulletTip; // Reference to the "tip" of the bullet (empty GameObject)

    private EnemyController targetEnemy;  // Cache for the EnemyController of the target

    // Set the target for the projectile
    public void SetTarget(Transform targetTransform, float damageValue)
    {
        target = targetTransform;
        damage = damageValue;
        if (target != null)
        {
            targetEnemy = target.GetComponent<EnemyController>();
        }
    }

    private void Update()
    {
        // If the target is null or the enemy is dead, destroy the projectile
        if (target == null || (targetEnemy != null && targetEnemy.IsDead()))
        {
            Destroy(gameObject);
            return;
        }

        // Move towards the target if it's still valid
        Vector3 direction = (target.position - transform.position).normalized;
        float distanceThisFrame = speed * Time.deltaTime;

        // Rotate the bullet tip to face the target
        bulletTip.LookAt(target);  // Ensure that the bulletTip (empty) always faces the target

        // Move the projectile towards the target
        transform.Translate(direction * distanceThisFrame, Space.World);

        // Check if the projectile has reached the target
        if (Vector3.Distance(transform.position, target.position) <= distanceThisFrame)
        {
            HitTarget(); // Handle what happens when the projectile reaches the target
        }
    }

    private void HitTarget()
    {
        if (target != null)
        {
            // Apply damage to the target
            EnemyController enemy = target.GetComponent<EnemyController>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
            }
        }

        // Destroy the projectile after hitting the target
        Destroy(gameObject);
    }
}
