using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    public float speed = 10f;
    private Transform target;
    public float damage = 10f;  // Damage dealt by the projectile

    private EnemyController targetEnemy;  // Cache for the EnemyController of the target

    // Set the target for the projectile
    public void SetTarget(Transform targetTransform)
    {
        target = targetTransform;
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
        transform.position += direction * speed * Time.deltaTime;

        // Check if the projectile has reached the target
        if (Vector3.Distance(transform.position, target.position) < 0.1f)
        {
            Destroy(gameObject);  // Destroy the projectile when it reaches the target
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            EnemyController enemy = other.GetComponent<EnemyController>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
                Destroy(gameObject);  // Destroy the projectile after damaging the enemy
            }
        }
    }
}
