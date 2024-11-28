using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GoldManager : MonoBehaviour
{
    [Header("Gold Settings")]
    public int startingGold = 20;  // Initial gold amount
    public int currentGold;       // Current amount of gold
    public TextMeshProUGUI goldDisplay; // UI Text to display gold

    [Header("Passive Gold Gain")]
    public float goldGainInterval = 5f; // Time in seconds to gain gold
    public int goldGainAmount = 1;      // Amount of gold gained per interval

    private float timer;

    void Start()
    {
        currentGold = startingGold;
        UpdateGoldDisplay();
    }

    void Update()
    {
        // Passive gold gain logic
        timer += Time.deltaTime;
        if (timer >= goldGainInterval)
        {
            EarnGold(goldGainAmount);
            timer = 0f;
        }
    }

    /// <summary>
    /// Adds gold to the player's total.
    /// </summary>
    /// <param name="amount">The amount of gold to add.</param>
    public void EarnGold(int amount)
    {
        currentGold += amount;
        UpdateGoldDisplay();
        Debug.Log($"Earned {amount} gold. Current total: {currentGold}");
    }

    /// <summary>
    /// Attempts to spend the specified amount of gold.
    /// </summary>
    /// <param name="amount">The amount of gold to spend.</param>
    /// <returns>True if the gold was successfully spent, false otherwise.</returns>
    public bool SpendGold(int amount)
    {
        if (currentGold >= amount)
        {
            currentGold -= amount;
            UpdateGoldDisplay();
            Debug.Log($"Spent {amount} gold. Remaining total: {currentGold}");
            return true;
        }
        else
        {
            Debug.LogWarning($"Not enough gold to spend {amount}. Current total: {currentGold}");
            return false;
        }
    }

    /// <summary>
    /// Updates the gold display in the UI.
    /// </summary>
    private void UpdateGoldDisplay()
    {
        if (goldDisplay != null)
        {
            goldDisplay.text = $"Gold: {currentGold}";
        }
        else
        {
            Debug.LogWarning("Gold display UI is not assigned.");
        }
    }

    /// <summary>
    /// Checks if the player has enough gold for an action.
    /// </summary>
    /// <param name="amount">The amount of gold required.</param>
    /// <returns>True if the player has enough gold, false otherwise.</returns>
    public bool CanAfford(int amount)
    {
        return currentGold >= amount;
    }


    public int GetGold()
{
    return currentGold;
}
}
