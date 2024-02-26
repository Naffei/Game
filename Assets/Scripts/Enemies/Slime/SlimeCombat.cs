using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeC : MonoBehaviour
{
//     public int hp = 100;
    public Rigidbody2D rb;
    public float pushback = 5f;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
                
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Detect player attack and push away
        if (collision.gameObject.name == "MeleeHitbox")
        {
            Debug.Log("Hit Detected");
            Vector2 pushbackDir = (transform.position - collision.transform.position).normalized;
            rb.AddForce(pushbackDir * pushback, ForceMode2D.Impulse);  
        }
    }
}
