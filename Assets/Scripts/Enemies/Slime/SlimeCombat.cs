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

    private void onTriggerEnter2D(Collider2D other)
    {
        //Detect player attack and push away
        if(other.GameObject.name == "MeleeHitbox")
        {
        Vector2. pushbackDir = (transform.position-other.transform.postiion).normalized;
        rb.AddForce(pushbackDirection * BackBlast, ForceMode.2D.Impluse)     
        }
    }

}
