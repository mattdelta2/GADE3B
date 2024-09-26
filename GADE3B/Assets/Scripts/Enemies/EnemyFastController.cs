using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFastController : EnemyController
{

    public float fastSpeed = 10f;  // Speed of the fast enemy, adjust this as needed

    protected override void Start()
    {
        base.Start();
        // Increase the speed of the NavMeshAgent for fast enemy
        if (agent != null)
        {
            agent.speed = fastSpeed;
        }
    }

}
