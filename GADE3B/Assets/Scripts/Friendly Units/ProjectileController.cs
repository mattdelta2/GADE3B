using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    public float speed = 10f;
    private Transform target;
    public float damage = 10f;  // Damage dealt by the projectile

    public void SetTarget(Transform targetTransform)
    {
        target = targetTransform;
    }

    private void Update()
    {
        if (target != null)
        {
            Vector3 direction = (target.position - transform.position).normalized;
            transform.position += direction * speed * Time.deltaTime;

            if (Vector3.Distance(transform.position, target.position) < 0.1f)
            {
                // Handle collision with target or destroy projectile
                // This will be handled by OnTriggerEnter

                Debug.Log("Projectile reached the target.");
                Destroy(gameObject);
            }
        }
    }
    //private bool hasDamaged = false;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))  // Check if the object has the "Enemy" tag
        {
            EnemyController enemy = other.GetComponent<EnemyController>();
            if (enemy != null)
            {
                Debug.Log("EnemyController found on: " + other.gameObject.name);
                enemy.TakeDamage(damage);
                Destroy(gameObject);  // Destroy the projectile
            }
            else
            {
                Debug.LogError("EnemyController not found on the hit object: " + other.gameObject.name);
            }
        }
    }
}
