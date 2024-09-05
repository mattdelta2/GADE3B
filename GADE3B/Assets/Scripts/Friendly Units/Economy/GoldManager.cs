using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.UI;
using TMPro;

public class GoldManager : MonoBehaviour
{
    public int startingGold = 20;
    public int currentGold;
    public TextMeshProUGUI goldDisplay;
    public float goldGainInterval = 5f;  // Time in seconds to gain gold
    public int goldGainAmount = 1;  // Amount of gold gained per interval

    private float timer;

    void Start()
    {
        currentGold = startingGold;
        UpdateGoldDisplay();
    }

    void Update()
    {
        // Gold gain over time
        timer += Time.deltaTime;
        if (timer >= goldGainInterval)
        {
            EarnGold(goldGainAmount);
            timer = 0f;
        }
    }

    public void EarnGold(int amount)
    {
        currentGold += amount;
        UpdateGoldDisplay();
    }

    public bool SpendGold(int amount)
    {
        if (currentGold >= amount)
        {
            currentGold -= amount;
            UpdateGoldDisplay();
            return true;
        }
        return false;
    }

    void UpdateGoldDisplay()
    {
        if (goldDisplay != null)
        {
            goldDisplay.text = $"Gold: {currentGold}";
        }
    }
}
