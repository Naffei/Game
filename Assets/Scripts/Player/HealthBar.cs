using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    public GameObject heartPrefab;
    public Player player;

    private List<Hearts> hearts = new List<Hearts>();

    void Start()
    {
        
    }

    public void PopulateHealthBar()
    {
        ClearBar();

        int maxHearts = Mathf.CeilToInt(player.maxHP / 2f);
        for ( int i = 0; i< maxHearts; i++ )
        {
            CreateEmpty();
        }

        UpdateHealthBar(player.currentHealth);
    }

    public void UpdateHealthBar(float currentHealth)
    {
        for (int i = 0; i < hearts.Count; i++)
        {
            int healthForThisHeart = Mathf.Clamp((int)currentHealth - (i * 2), 0, 2);
            hearts[i].SetHearts((HeartStatus)healthForThisHeart);
        }
    }

    public void CreateEmpty()
    {
        GameObject newHeartBar = Instantiate(heartPrefab);
        newHeartBar.transform.SetParent(transform);

        RectTransform newHeartTransform = newHeartBar.GetComponent<RectTransform>();
        float scaleFactor = 1f;
        newHeartTransform.localScale = Vector3.one * scaleFactor;

        Hearts heartComponent = newHeartBar.GetComponent<Hearts>();
        heartComponent.SetHearts(HeartStatus.Empty); ;
        hearts.Add(heartComponent);
    }

    public void ClearBar()
    {
        foreach(Transform i in transform)
        {
            Destroy(i.gameObject);
        }
    }
}
