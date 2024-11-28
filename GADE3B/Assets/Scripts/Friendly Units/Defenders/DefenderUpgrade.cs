using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefenderUpgrade : MonoBehaviour
{
    public DefenderController selectedDefender;

    public void UpgradeSelectedDefender()
    {
        if (selectedDefender != null)
        {
            selectedDefender.UpgradeDefender();
        }
        else
        {
            Debug.Log("No defender selected.");
        }
    }
}
