using UnityEngine;

public class Item : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip spawnClip;
    public AudioClip pickUp;

    private void Start()
    {
        audioSource.clip = spawnClip;
        audioSource.Play();
    }
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
            Player player = other.GetComponent<Player>();

            if (player != null)
            {
                switch (itemType)
                {
                    case ItemType.Sword:
                        player.IncreaseDamage();
                        break;
                    case ItemType.HealthPotion:
                        player.AddHealthPotion();
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