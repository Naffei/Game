using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions.Must;

public class Slime : MonoBehaviour, IAttackable
{
    public Rigidbody2D rb;
    public float moveSpeed = 2f;
    public float maxHP = 3;
    public float currentHealth;

    public GameObject roomBoundary;

    private Transform player;
    private Vector3 lastPlayerPos;
    private List<Vector3> playerPositions = new List<Vector3>();
    private bool isPushed = false;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        currentHealth = maxHP;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!isPushed && IsPlayerInRoom(player.position))
        {
            if (Vector3.Distance(player.position, lastPlayerPos) > 0.5f)
            {
                // Clear list position if player moves
                playerPositions.Clear();
                playerPositions.Add(player.position);
                lastPlayerPos = player.position;
            }
            MoveToPlayer();
        }
    }

    void MoveToPlayer()
    {
        if (playerPositions.Count > 0)
        {
            Vector2 direction = ((Vector2)playerPositions[0] - rb.position).normalized;
            float distanceToPlayer = Vector2.Distance(rb.position, playerPositions[0]);
            float stopThreshold = 0.5f;

            if (distanceToPlayer > stopThreshold)
            {
                rb.MovePosition(rb.position + direction * moveSpeed * Time.fixedDeltaTime);
            }
        }
    }

    private bool IsPlayerInRoom(Vector2 position)
    {
        Bounds roomBounds = roomBoundary.GetComponent<BoxCollider2D>().bounds;
        return roomBounds.Contains(position);
    }


    public void Hit(float damageTaken, Vector2 knockback)
    {
        Debug.Log("Hit Detected " + damageTaken);
        Debug.Log(currentHealth);
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

    public void Hit(float damageTaken)
    {
        currentHealth -= damageTaken;
    }

    void Die()
    {
        Destroy(gameObject);
    }

    IEnumerator DisableMovementForDuration(float duration)
    {
        isPushed = true;
        yield return new WaitForSeconds(duration);
        isPushed = false;
        rb.velocity = Vector2.zero;
    }
}