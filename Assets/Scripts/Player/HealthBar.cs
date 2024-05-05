using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    public GameObject heartPrefab;
    public Player player;

    private List<Hearts> hearts = new List<Hearts>();


    // Add heart containers to bar
    public void PopulateHealthBar()
    {
        // Clear HP bar first
        ClearBar();

        // Find the max amount of health player can have
        int maxHearts = Mathf.CeilToInt(player.maxHP / 2f);
        for ( int i = 0; i< maxHearts; i++ )
        {
            CreateEmpty();
        }

        // Update health based on current health of player
        UpdateHealthBar(player.currentHealth);
    }

    // Updates current health
    public void UpdateHealthBar(float currentHealth)
    {
        for (int i = 0; i < hearts.Count; i++)
        {
            int healthForThisHeart = Mathf.Clamp((int)currentHealth - (i * 2), 0, 2);
            Debug.Log(healthForThisHeart);
            hearts[i].SetHearts(healthForThisHeart);
        }
    }

    // Create a empty hp bar first
    public void CreateEmpty()
    {
        GameObject newHeartBar = Instantiate(heartPrefab);
        newHeartBar.transform.SetParent(transform);

        RectTransform newHeartTransform = newHeartBar.GetComponent<RectTransform>();
        float scaleFactor = 1f;
        newHeartTransform.localScale = Vector3.one * scaleFactor;

        Hearts heartComponent = newHeartBar.GetComponent<Hearts>();
        heartComponent.SetHearts(0);
        hearts.Add(heartComponent);
    }

    // Clear health bar
    public void ClearBar()
    {
        Debug.Log("Clearing HP Bar");
        foreach(Transform i in transform)
        {
            Destroy(i.gameObject);
        }
    }
}
