using UnityEngine;
using UnityEngine.UI;

public class UpgradeManager : MonoBehaviour
{
    public Button upgradeButton; // Reference to the upgrade button
    private DefenderController selectedDefender; // Currently selected defender

    void Start()
    {
        // Hide the upgrade button initially
        upgradeButton.gameObject.SetActive(false);
        upgradeButton.onClick.AddListener(UpgradeSelectedDefender);
    }

    void Update()
    {
        // Check for mouse clicks to select a defender
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                DefenderController defender = hit.collider.GetComponent<DefenderController>();
                if (defender != null)
                {
                    selectedDefender = defender;
                    ShowUpgradeButton(defender);
                }
                else
                {
                    selectedDefender = null;
                    upgradeButton.gameObject.SetActive(false);
                }
            }
        }
    }

    private void ShowUpgradeButton(DefenderController defender)
    {
        // Only show the button if upgrades are available
        if (defender.upgradeLevel < defender.maxUpgrades)
        {
            upgradeButton.gameObject.SetActive(true);
        }
        else
        {
            upgradeButton.gameObject.SetActive(false);
        }
    }

    private void UpgradeSelectedDefender()
    {
        if (selectedDefender != null)
        {
            selectedDefender.UpgradeDefender();
            upgradeButton.gameObject.SetActive(false); // Hide button after upgrade
        }
    }
}
