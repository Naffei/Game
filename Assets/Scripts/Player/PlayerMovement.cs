using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 3f;
    public Rigidbody2D rb;

    Vector2 movement;
    //SpriteRenderer spriteRenderer;

    public Animator animator;
    public float swordDmg = 1f;
    public float knockbackForce = 1000f;

    public GameObject MeleeHitbox;

    private int potionCount = 0;

    public void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        //spriteRenderer = GetComponent<SpriteRenderer>();
    }


    // Update is called once per frame
    void Update()
    {
        // User Inputs
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        if (movement.x > 0)
        {
            transform.localScale = new Vector2(1, 1);
        }
        else if (movement.x < 0)
        {
            transform.localScale = new Vector2(-1, 1); 
        }

        // Attack
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Attack();
            Debug.Log("Input detected");
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

        IAttackable attackAble = other.GetComponent<IAttackable>();

        if (attackAble != null)
        {
            Vector3 swordPos = transform.position;
            Vector2 direction = (Vector2)(swordPos - other.gameObject.transform.position).normalized;

            Vector2 knockback = direction * knockbackForce;

            Debug.Log("Applying knockback: " + knockback);

            attackAble.Hit(swordDmg, knockback);
        }
    }

    void Attack()
    {
        Debug.Log("Attack Called: ");
        animator.SetTrigger("swingAtk");

        MeleeHitbox.SetActive(true);
    }

    public void IncreaseDamage()
    {
        swordDmg += 1f;
    }

    public void HealthPotion()
    {
        potionCount++;
    }

}