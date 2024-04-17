using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeC : MonoBehaviour
{
    public int maxHP = 3;
    public int currentHealth;
    public Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        currentHealth = maxHP;
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void Hit(int damageTaken)
    {
        Debug.Log("Hit Detected " + damageTaken);
        Debug.Log(currentHealth);
        currentHealth -= damageTaken;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Destroy(gameObject);
    }
}
