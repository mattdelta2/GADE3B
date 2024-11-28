using UnityEngine;
using UnityEngine.UI;


public class UpgradeManager : MonoBehaviour
{
    public GameObject selectedDefender;  // The defender to upgrade
    public GoldManager goldManager;      // Reference to the GoldManager
    public int upgradeCost = 10;         // Cost to upgrade a defender

    public LayerMask defenderLayer;      // LayerMask to ensure only defenders are selected

    public void Update()
    {
        if (Input.GetMouseButtonDown(0)) // Left-click to select a defender
        {
            SelectDefender();
        }
    }

    /// <summary>
    /// Selects a defender based on player input (mouse click).
    /// </summary>
    private void SelectDefender()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        // Draw debug line in the Scene view
        Debug.DrawRay(ray.origin, ray.direction * 100f, Color.red, 2f);

        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, defenderLayer))
        {
            DefenderController defender = hit.collider.GetComponent<DefenderController>();
            if (defender != null)
            {
                selectedDefender = hit.collider.gameObject;
                Debug.Log($"Defender selected: {selectedDefender.name}");
            }
            else
            {
                Debug.LogWarning("The selected object is not a defender.");
            }
        }
        else
        {
            Debug.LogWarning("No valid defender selected.");
            selectedDefender = null;
        }
    }

    /// <summary>
    /// Attempts to upgrade the selected defender.
    /// </summary>
    public void TryUpgradeDefender()
    {
        if (selectedDefender == null)
        {
            Debug.LogWarning("No defender selected for upgrade.");
            return;
        }

        DefenderController defender = selectedDefender.GetComponent<DefenderController>();
        if (defender == null)
        {
            Debug.LogWarning("Selected object does not have a DefenderController.");
            return;
        }

        if (goldManager == null)
        {
            Debug.LogError("GoldManager is not assigned.");
            return;
        }

        if (goldManager.GetGold() >= upgradeCost && defender.upgradeLevel < defender.maxUpgrades)
        {
            goldManager.SpendGold(upgradeCost);
            defender.UpgradeDefender();
            Debug.Log("Defender upgraded successfully.");
        }
        else if (defender.upgradeLevel >= defender.maxUpgrades)
        {
            Debug.LogWarning("Defender has already reached the maximum upgrade level.");
        }
        else
        {
            Debug.LogWarning("Not enough gold to upgrade the defender.");
        }
    }

    /// <summary>
    /// Clear the currently selected defender (e.g., when exiting upgrade mode).
    /// </summary>
    public void ClearSelection()
    {
        selectedDefender = null;
    }
}
