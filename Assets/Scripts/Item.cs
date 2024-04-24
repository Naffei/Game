using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public enum ItemType
    {
        Sword,
        HealthPotion
    }

    public ItemType itemType;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerMovement player = other.GetComponent<PlayerMovement>();

            if (player != null)
            {
                switch (itemType)
                {
                    case ItemType.Sword:
                        player.IncreaseDamage();
                        break;
                    case ItemType.HealthPotion:
                        player.HealthPotion();
                        break;
                }

                Destroy(gameObject);
            }
            else
            {
                Debug.LogWarning("Error no player found");
            }
        }
    }
}
