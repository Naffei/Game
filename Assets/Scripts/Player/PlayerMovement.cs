using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 3f;
    public Rigidbody2D rb;

    Vector2 movement;
    SpriteRenderer spriteRenderer;
    Walk walkScript;

    public void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        walkScript = FindObjectOfType<Walk>();
    }


    // Update is called once per frame
    void Update()
    {
        // User Inputs
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        if (movement.x > 0)
        {
            spriteRenderer.flipX = false;
        }
        else if (movement.x < 0)
        {
            spriteRenderer.flipX = true;
        }

    }

    void FixedUpdate()
    {
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        // Check if there is a wall and make player stop
        if (collision.gameObject.CompareTag("Collidable"))
        {
            rb.velocity = Vector2.zero;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Door"))
        {
            Debug.Log("Door Detected");

            // Get the name of the door
            string doorName = other.name;

            // Set the offset based on the door name
            Vector2 offset = Vector2.zero;

            switch (doorName)
            {
                case "TopDoor":
                    offset = Vector2.up * 3.5f;
                    break;
                case "BotDoor":
                    offset = Vector2.down * 2f;
                    break;
                case "LeftDoor":
                    offset = Vector2.left * 3f;
                    break;
                case "RightDoor":
                    offset = Vector2.right * 2f;
                    break;
                default:
                    Debug.LogWarning("Unknown door name: " + doorName);
                    break;
            }

            // Move the player by adding the offset to their current position
            transform.position = (Vector2)transform.position + offset;

            Debug.Log("Player moved to: " + transform.position);
        }
    }
}