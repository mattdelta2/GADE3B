using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerPlacement : MonoBehaviour
{
    public LayerMask pathLayer;  // Layer for the path

    public bool CanPlaceTower(Vector3 position)
    {
        // Check if the position is on the path
        Collider[] colliders = Physics.OverlapSphere(position, 0.5f, pathLayer);
        return colliders.Length == 0;  // Return true if no colliders are found
    }
}
