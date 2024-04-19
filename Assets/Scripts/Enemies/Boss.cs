using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Boss : MonoBehaviour, IAttackable
{
    public Rigidbody2D rb;
    public Transform player;
    public float moveSpeed = 2f;
    public float maxHP = 10;
    public float currentHealth;

    private bool OnCooldown = false;
    private float cooldownDur = 5f;
    private float cooldownTime = 0f;

    public GameObject roomBoundary;
    public Animator animator;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        currentHealth = maxHP;
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {

        if (OnCooldown)
        {
            cooldownTime -= Time.deltaTime;
            if (cooldownTime <= 0f)
            {
                OnCooldown = false;
            }
        }
    }

    private void FixedUpdate()
    {
        if (IsPlayerInRoom(player.position))
        {
            Vector2 directionToPlayer = (Vector2)player.position - rb.position;
            float distanceToPlayer = directionToPlayer.magnitude;

            RotateTowardsPlayer(directionToPlayer);

            if (distanceToPlayer > 1f)
            {
                Vector2 moveDirection = directionToPlayer.normalized;
                rb.MovePosition(rb.position + moveDirection * moveSpeed * Time.fixedDeltaTime);
            }
        }

        if (IsPlayerInRoom(player.position) && !OnCooldown)
        {
            // Perform attack if not on cooldown
            Attack();
        }
        else
        {
            // Move away from the player during cooldown
            MoveAway();
        }
    }
    private void RotateTowardsPlayer(Vector2 direction)
    {
        if (direction.x > 0)
        {
            transform.localScale = new Vector2(-1, 1);
        }
        else
        {
            transform.localScale = new Vector2(1, 1);
        }
    }

    private bool IsPlayerInRoom(Vector2 position)
    {
        Bounds roomBounds = roomBoundary.GetComponent<BoxCollider2D>().bounds;
        return roomBounds.Contains(position);
    }


    // Attack Related Stuff

    void Attack()
    {
        animator.SetTrigger("bossSwing");

        //MeleeHitbox.SetActive(true);

        // Set boss on cooldown
        OnCooldown = true;
        cooldownTime = cooldownDur;
    }

    void MoveAway()
    {
        // Calculate direction to move away from the player
        Vector2 directionToPlayer = (Vector2)player.position - rb.position;
        Vector2 moveDirection = -directionToPlayer.normalized;

        // Check if the distance to the player is less than the specified threshold
        if (directionToPlayer.magnitude < 0.6f)
        {
            // Move away from the player with a fixed speed
            rb.MovePosition(rb.position + moveDirection * 1f * Time.fixedDeltaTime);
        }
    }


        public void Hit(float damageTaken, Vector2 knockback)
    {
        currentHealth -= damageTaken;
    }

    public void Hit(float damageTaken)
    {
        currentHealth -= damageTaken;
    }

}