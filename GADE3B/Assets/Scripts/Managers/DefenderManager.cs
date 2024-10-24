using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefenderManager : MonoBehaviour
{
    public List<GameObject> defenders = new List<GameObject>(); // List to store all active defenders
    public float defenderCheckRadius = 5f; // Radius around defenders to avoid enemy spawning

    private void Start()
    {
        // Optionally, if there are pre-placed defenders, find and add them to the list
        GameObject[] existingDefenders = GameObject.FindGameObjectsWithTag("Defender");
        foreach (GameObject defender in existingDefenders)
        {
            AddDefender(defender);
        }
    }

    // Method to add a defender to the manager
    public void AddDefender(GameObject defender)
    {
        if (!defenders.Contains(defender))
        {
            defenders.Add(defender);
            Debug.Log($"Defender added: {defender.name} at position {defender.transform.position}");
        }
    }

    // Method to remove a defender from the manager (e.g., if destroyed)
    public void RemoveDefender(GameObject defender)
    {
        if (defenders.Contains(defender))
        {
            defenders.Remove(defender);
            Debug.Log($"Defender removed: {defender.name}");
        }
    }

    // Method to check if a given position is near a defender
    public bool IsNearDefender(Vector3 position, float checkRadius)
    {
        foreach (GameObject defender in defenders)
        {
            if (Vector3.Distance(position, defender.transform.position) <= checkRadius)
            {
                return true; // Position is near a defender
            }
        }
        return false; // No defenders nearby
    }

    // Optional: Method to visualize defender areas in the editor
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        foreach (GameObject defender in defenders)
        {
            Gizmos.DrawWireSphere(defender.transform.position, defenderCheckRadius);
        }
    }
}
