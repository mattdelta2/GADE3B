using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefenderManager : MonoBehaviour
{
    public List<GameObject> defenders = new List<GameObject>(); // List to store all active defenders
    public float defenderCheckRadius = 5f; // Radius around defenders to avoid enemy spawning

    // Dictionary to track the number of defenders by type
    private Dictionary<string, int> defenderCounts = new Dictionary<string, int>();

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

            // Add defender to defenderCounts based on its type
            DefenderController defenderController = defender.GetComponent<DefenderController>();
            if (defenderController != null)
            {
                string defenderType = defenderController.defenderType;

                if (defenderCounts.ContainsKey(defenderType))
                {
                    defenderCounts[defenderType]++;
                }
                else
                {
                    defenderCounts[defenderType] = 1;
                }

                Debug.Log($"Defender of type {defenderType} added. Total of this type: {defenderCounts[defenderType]}");
            }
            else
            {
                Debug.LogError("The defender does not have a DefenderController script.");
            }
        }
    }

    // Method to remove a defender from the manager (e.g., if destroyed)
    public void RemoveDefender(GameObject defender)
    {
        if (defenders.Contains(defender))
        {
            defenders.Remove(defender);

            // Remove defender from defenderCounts based on its type
            DefenderController defenderController = defender.GetComponent<DefenderController>();
            if (defenderController != null)
            {
                string defenderType = defenderController.defenderType;

                if (defenderCounts.ContainsKey(defenderType))
                {
                    defenderCounts[defenderType]--;
                    if (defenderCounts[defenderType] <= 0)
                    {
                        defenderCounts.Remove(defenderType); // Remove the type if no defenders of this type are left
                    }

                    Debug.Log($"Defender of type {defenderType} removed. Remaining of this type: {defenderCounts[defenderType]}");
                }
            }
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

    // Method to get the most common defender type
    public string GetMostCommonDefenderType()
    {
        string mostCommonType = "Normal"; // Default to "Normal" if no defenders are placed
        int maxCount = 0;

        foreach (var defenderType in defenderCounts)
        {
            if (defenderType.Value > maxCount)
            {
                mostCommonType = defenderType.Key;
                maxCount = defenderType.Value;
            }
        }

        return mostCommonType;
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
