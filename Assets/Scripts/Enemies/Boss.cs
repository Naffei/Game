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
            Debug.Log(cooldownTime);
            if (cooldownTime <= 0f)
            {
                OnCooldown = false;
            }
        }

        if (IsPlayerInRoom(player.position) && !OnCooldown && cooldownTime <= 0f)
        {
            Attack();
        }
        else if (OnCooldown)
        {
            MoveAway();
        }
    }

    private void FixedUpdate()
    {
        if (IsPlayerInRoom(player.position))
        {
            Vector2 directionToPlayer = (Vector2)player.position - rb.position;
            float distanceToPlayer = directionToPlayer.magnitude;

            LookTowardsPlayer(directionToPlayer);

            if (distanceToPlayer > 1f)
            {
                Vector2 moveDirection = directionToPlayer.normalized;
                rb.MovePosition(rb.position + moveDirection * moveSpeed * Time.fixedDeltaTime);
            }
        }
    }
    private void LookTowardsPlayer(Vector2 direction)
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
        OnCooldown = true;
        cooldownTime = cooldownDur;
    }

    void MoveAway()
    {
        Vector2 directionToPlayer = (Vector2)player.position - rb.position;
        Vector2 moveDirection = -directionToPlayer.normalized;

        if (directionToPlayer.magnitude < 1f)
        {
            rb.MovePosition(rb.position + moveDirection * 1.5f * Time.fixedDeltaTime);
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