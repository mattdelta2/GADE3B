using UnityEngine;

public class DefenderBombController : DefenderController
{
    public float explosionRadius = 5f;
    public GameObject explosionEffect;

    protected override void Die()
    {
        // Override to explode when dying
        Explode();
        base.Die();  // Call the base Die method to handle health bar destruction
    }

    private void Explode()
    {
        // Optionally instantiate an explosion effect
        if (explosionEffect != null)
        {
            Instantiate(explosionEffect, transform.position, Quaternion.identity);
        }

        // Damage enemies in a circular radius
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
    }

    private void OnDrawGizmosSelected()
    {
        // Draw explosion radius for visualization
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}
