using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    public float speed = 10f;
    private Transform target;
    private float damage;  // Damage that will be inherited from the defender

    private EnemyController targetEnemy;  // Cache for the EnemyController of the target

    // Set the target and damage for the projectile
    public void SetTarget(Transform targetTransform, float damageValue)
    {
        target = targetTransform;
        damage = damageValue;  // Set the damage from the defender
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

        // Move towards the target using Time.deltaTime for consistent speed
        Vector3 direction = (target.position - transform.position).normalized;
        transform.position += direction * speed * Time.deltaTime;

        // Rotate projectile to face the target if necessary
        if (direction != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(direction);
        }

        // Check if the projectile has reached the target
        if (Vector3.Distance(transform.position, target.position) < 0.1f)
        {
            HitTarget();  // Handle hitting the target
        }
    }

    void OnTriggerEnter(Collider other)
    {
        // Ensure that the projectile damages enemies upon collision
        if (other.CompareTag("Enemy"))
        {
            EnemyController enemy = other.GetComponent<EnemyController>();
            if (enemy != null)
            {
                HitTarget();  // Apply damage and destroy the projectile
            }
        }
    }

    private void HitTarget()
    {
        // Apply damage to the target
        if (targetEnemy != null)
        {
            targetEnemy.TakeDamage(damage);  // Use inherited damage value
        }

        // Optionally add explosion effect here if needed

        Destroy(gameObject);  // Destroy the projectile after hitting the target
    }
}
