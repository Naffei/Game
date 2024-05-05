using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PotionCounter : MonoBehaviour
{
    public Text potionText;
    public Player player;

    private void Start()
    {
        UpdatePotionCount();
    }

    private void Update()
    {
        UpdatePotionCount();
    }

    // Count potions
    public void UpdatePotionCount()
    {
        if (potionText != null && player != null)
        {
            potionText.text = "x" + player.potionCount.ToString();
        }
    }
}
