using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTauntController : EnemyController
{
    public float tauntCooldown = 5f;     // Cooldown for the taunt ability
    public float tauntDuration = 1.5f;   // Duration of the taunt effect
    private bool canTaunt = true;        // Whether the enemy can taunt
    private float tauntRadius = 25f;

    protected override void Update()
    {
        base.Update();

        // Check if any defenders are within range and attempt to taunt them
        if (canTaunt && IsAnyDefenderInRange())
        {
            StartCoroutine(TauntDefenders());
        }
    }

    private bool IsAnyDefenderInRange()
    {
        Collider[] hitDefenders = Physics.OverlapSphere(transform.position, range);
        foreach (Collider defenderCollider in hitDefenders)
        {
            if (defenderCollider.CompareTag("Defender"))
            {
                return true;
            }
        }
        return false;
    }

    private IEnumerator TauntDefenders()
    {
        canTaunt = false;
        Debug.Log("Taunting defenders!");

        // Notify defenders to target this enemy
        TauntNearbyDefenders();

        // Wait for the taunt duration
        yield return new WaitForSeconds(tauntDuration);

        Debug.Log("Taunt ended.");
        yield return new WaitForSeconds(tauntCooldown);
        canTaunt = true;
    }

    private void TauntNearbyDefenders()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, tauntRadius);

        foreach (var collider in hitColliders)
        {
            DefenderController defender = collider.GetComponent<DefenderController>();
            if (defender != null)
            {
                defender.TargetEnemy(this.transform);
            }
        }
    }

    // Override the ScaleAttributes method to adjust taunt-specific properties
    public override void ScaleAttributes(float healthScale, float damageScale, float speedScale)
    {
        base.ScaleAttributes(healthScale, damageScale, speedScale);

        // Adjust taunt-specific properties
        tauntCooldown /= speedScale;  // Reduce cooldown for increased difficulty
        tauntDuration *= speedScale;  // Increase taunt duration for higher difficulty
    }
}
