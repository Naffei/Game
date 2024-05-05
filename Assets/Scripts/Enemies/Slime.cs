using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions.Must;

public class Slime : MonoBehaviour, IAttackable
{
    public Rigidbody2D rb;
    public float moveSpeed = 1f;
    public float maxHP = 3;
    public float currentHealth;

    public float atkDmg = 1f;
    public float knockbackForce = 1000f;

    public Animator animator;

    public GameObject roomBoundary;
    private Room room;

    public AudioSource audioSource;
    public AudioClip atkClip;

    private Transform player;
    private Vector3 lastPlayerPos;
    private List<Vector3> playerPositions = new List<Vector3>();
    public float maxPositionHistory = 5f;
    private bool isPushed = false;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        currentHealth = maxHP;
        room = transform.GetComponentInParent<Room>();
        if (room != null)
        {
            room.AddEnemy(gameObject);
        }
        else
        {
            Debug.LogError("Room not found for enemy: " + gameObject.name);
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // Move to Player
        if (!isPushed && IsPlayerInRoom(player.position))
        {
            // Move to position from list
            if (Vector3.Distance(player.position, lastPlayerPos) > 0.2f)
            {
                TrackPlayerMovement(player.position);
                MoveToPlayer();
            }
        }

    }

    void MoveToPlayer()
    {
        if (playerPositions.Count > 0)
        {
            for (int i = 0; i < playerPositions.Count - 1; i++)
            {
                Vector2 direction = ((Vector2)playerPositions[i] - rb.position).normalized;
                rb.MovePosition(rb.position + direction * moveSpeed * Time.fixedDeltaTime);
            }
        }
    }

    // Ddd old player positions to list
    void TrackPlayerMovement(Vector3 newPosition)
    {
        playerPositions.Add(newPosition);

        // Keep only the last few positions
        int maxPositions = 2;
        if (playerPositions.Count > maxPositions)
        {
            playerPositions.RemoveAt(0);
        }
    }

    // Check if player is in room
    private bool IsPlayerInRoom(Vector2 position)
    {
        Bounds roomBounds = roomBoundary.GetComponent<BoxCollider2D>().bounds;
        return roomBounds.Contains(position);
    }

    // Check if hit by player melee hitbox specfically
    private bool IsHitByMeleeHitbox()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            Transform meleeHitboxTransform = player.transform.Find("MeleeHitbox");
            if (meleeHitboxTransform != null)
            {
                Collider2D hitboxCollider = meleeHitboxTransform.GetComponent<Collider2D>();
                if (hitboxCollider != null && hitboxCollider.IsTouching(GetComponent<Collider2D>()))
                {
                    return true;
                }
            }
        }
        return false;
    }

    // Attack player by colliding with them
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Vector2 direction = (transform.position - collision.transform.position).normalized;
            Vector2 knockback = knockbackForce * direction;
            if (collision.transform.position.x > transform.position.x)
            {
                knockback = new Vector2(-Mathf.Abs(knockback.x), knockback.y);
            }
            else
            {
                knockback = new Vector2(Mathf.Abs(knockback.x), knockback.y);
            }

            IAttackable attackable = collision.gameObject.GetComponent<IAttackable>();
            if (attackable != null)
            {
                audioSource.clip = atkClip;
                audioSource.Play();

                attackable.Hit(atkDmg);
                rb.AddForce(knockback);
            }

        }
    }


    // Receive hits / knockback
    public void Hit(float damageTaken, Vector2 knockback)
    {
        if (!isPushed && IsHitByMeleeHitbox())
        {
            animator.SetTrigger("hit");
            currentHealth -= damageTaken;
            rb.AddForce(knockback);

            rb.velocity = Vector2.zero;
            rb.angularVelocity = 0f;

            StartCoroutine(DisableMovementForDuration(1f));

            if (currentHealth <= 0)
            {
                Die();
            }
        }
    }

    public void Hit(float damageTaken)
    {
        if (!isPushed && IsHitByMeleeHitbox())
        {
            Debug.Log("Hit Detected " + damageTaken);
            Debug.Log(currentHealth);
            animator.SetTrigger("hit");
            currentHealth -= damageTaken;
            StartCoroutine(DisableMovementForDuration(1f));

            if (currentHealth <= 0)
            {
                Die();
            }
        }
    }

    void Die()
    {
        Destroy(gameObject);

        if (room != null)
        {
            room.RemoveEnemy(gameObject);
        }
    }

    // If slime is hit disable movement for x amount
    IEnumerator DisableMovementForDuration(float duration)
    {
        isPushed = true;
        yield return new WaitForSeconds(duration);
        isPushed = false;
        rb.velocity = Vector2.zero;
    }
}