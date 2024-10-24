using UnityEngine;

public class EnemyProjectileController : MonoBehaviour
{
    public float speed = 8f; // Speed of the projectile
    private Transform target;
    public float damage = 15f; // Damage dealt by the projectile

    private DefenderController targetDefender;  
    private MainTowerController targetTower; 

    public void SetTarget(Transform targetTransform)
    {
        target = targetTransform;

        if (target != null)
        {
            targetDefender = target.GetComponent<DefenderController>();
            targetTower = target.GetComponent<MainTowerController>();
        }
    }

    private void Update()
    {
        if (target == null || (targetDefender != null && targetDefender.health <= 0) || (targetTower != null && targetTower.IsDead()))
        {
            Destroy(gameObject);
            return;
        }

        // Move towards the target
        Vector3 direction = (target.position - transform.position).normalized;
        float distanceThisFrame = speed * Time.deltaTime;
        
        transform.Translate(direction * distanceThisFrame, Space.World);
        transform.LookAt(target);

        // Check if the projectile has reached the target
        if (Vector3.Distance(transform.position, target.position) <= distanceThisFrame)
        {
            HitTarget();
        }
    }

    private void HitTarget()
    {
        if (targetDefender != null)
        {
            targetDefender.TakeDamage(damage);
        }

        if (targetTower != null)
        {
            targetTower.TakeDamage(damage);
        }

        Destroy(gameObject);
    }
}
