using UnityEngine;

public class EnemyProjectileController : MonoBehaviour
{
    public float speed = 8f; // Speed of the projectile
    private Transform target;
    public float damage = 15f; // Damage dealt by the projectile

    private DefenderController targetDefender;  // Cache for the DefenderController of the target
    private MainTowerController targetTower; // Cache for the MainTowerController of the target

    // Set the target for the projectile
    public void SetTarget(Transform targetTransform)
    {
        target = targetTransform;

        // Check if the target is a defender or the main tower
        if (target != null)
        {
            targetDefender = target.GetComponent<DefenderController>();
            targetTower = target.GetComponent<MainTowerController>();
        }
    }

    private void Update()
    {
        // If the target is null, or if the defender/tower is dead, destroy the projectile
        if (target == null || (targetDefender != null && targetDefender.health <= 0) || (targetTower != null && targetTower.IsDead()))
        {
            Destroy(gameObject);
            return;
        }

        // Move towards the target if it's still valid
        Vector3 direction = (target.position - transform.position).normalized;
        transform.position += direction * speed * Time.deltaTime;
        transform.rotation = Quaternion.LookRotation(direction);

        // Check if the projectile has reached the target
        if (Vector3.Distance(transform.position, target.position) < 0.1f)
        {
            HitTarget();  // Handle hitting the target
        }
    }

    private void HitTarget()
    {
        Debug.Log("Projectile hit the target: " + target.name);

        // If the target is a defender, apply damage
        if (targetDefender != null)
        {
            Debug.Log("Enemy is shooting at a defender!");
            targetDefender.TakeDamage(damage);
        }

        // If the target is the main tower, apply damage
        if (targetTower != null)
        {
            Debug.Log("Enemy is shooting at the main tower!");
            targetTower.TakeDamage(damage);
        }

        // Destroy the projectile after dealing damage
        Debug.Log("Projectile destroyed.");
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        // Handle collisions with defenders
        if (other.CompareTag("Defender"))
        {
            DefenderController defender = other.GetComponent<DefenderController>();
            if (defender != null)
            {
                defender.TakeDamage(damage);
                Destroy(gameObject);  // Destroy the projectile after damaging the defender
            }
        }

        // Handle collisions with the main tower
        if (other.CompareTag("Tower"))
        {
            MainTowerController tower = other.GetComponent<MainTowerController>();
            if (tower != null)
            {
                tower.TakeDamage(damage);
                Destroy(gameObject);  // Destroy the projectile after damaging the tower
            }
        }
    }
}
