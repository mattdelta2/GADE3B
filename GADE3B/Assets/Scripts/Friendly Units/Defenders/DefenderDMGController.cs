using UnityEngine;

public class DefenderDMGController : DefenderController
{
    
    protected override void Start()
    {
        base.Start();
        damage = 10f;  // Override the base damage
        range = 12f;   // Adjust the range for this defender
        shootingInterval = 2f; // Slower shooting speed
    }
}
