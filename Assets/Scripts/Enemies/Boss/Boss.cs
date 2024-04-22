using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Boss : MonoBehaviour, IAttackable
{
    public Rigidbody2D rb;
    public Transform player;
    public float moveSpeed = 2f;
    public float atkMoveSpeed = 8f;
    public float maxHP = 10;
    public float currentHealth;

    public GameObject roomBoundary;
    public Animator animator;


    private bool isIdle = true;
    private bool isAttacking = false;
    public Vector2 roamDir;


    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        currentHealth = maxHP;
        player = GameObject.FindGameObjectWithTag("Player").transform;

        if (Random.Range(0, 2) == 0)
        {
            StartCoroutine(IdlePhase());
        }
        else
        {
            StartCoroutine(AttackPhase());
        }
    }

    void Update()
    {
        if (IsPlayerInRoom(player.position))
        {
            Vector2 directionToPlayer = (Vector2)player.position - rb.position;
            RotateTowardsPlayer(directionToPlayer);
        }
    }

    IEnumerator BossBehavior()
    {
        while (true)
        {
            if (isIdle)
            {
                yield return StartCoroutine(IdlePhase());
            }
            else if (isAttacking)
            {
                yield return StartCoroutine(AttackPhase());
            }
        }
    }

    IEnumerator IdlePhase()
    {
        isIdle = true;
        float idleTime = Random.Range(5f, 8f);
        float timeElapsed = 0f;
        Vector2 roamDir = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;

        while (timeElapsed < idleTime)
        {
            rb.MovePosition(rb.position + roamDir * moveSpeed * Time.deltaTime);
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        // Switch to attack phase
        if (IsPlayerInRoom(player.position))
        {
            isIdle = false;
            isAttacking = true;
            StartCoroutine(AttackPhase());
        }
        else
        {
            StartCoroutine(IdlePhase());
        }
    }

    IEnumerator AttackPhase()
    {
        isAttacking = true;

        while (IsPlayerInRoom(player.position))
        {
            // Position the boss to the left or right of the player
            Vector2 directionToPlayer = (Vector2)player.position - rb.position;
            Vector2 perpendicularDirection = new Vector2(-directionToPlayer.y, directionToPlayer.x).normalized;
            Vector2 bossPosition = (Vector2)player.position + perpendicularDirection * 2.0f; // Adjust the distance as needed
            rb.MovePosition(bossPosition);

            // Move towards the player
            while (Vector2.Distance(transform.position, player.position) > 1.5f)
            {
                directionToPlayer = (Vector2)player.position - rb.position;
                rb.MovePosition(rb.position + directionToPlayer.normalized * atkMoveSpeed * Time.deltaTime);
                RotateTowardsPlayer(directionToPlayer);
                yield return null;
            }

            // Perform attack
            animator.SetTrigger("bossSwing");

            float cooldownTime = Random.Range(3f, 5f);
            yield return new WaitForSeconds(cooldownTime);


            isAttacking = false;
            StartCoroutine(IdlePhase());
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

    public void Hit(float damageTaken, Vector2 knockback)
    {
        currentHealth -= damageTaken;
    }

    public void Hit(float damageTaken)
    {
        currentHealth -= damageTaken;
    }

    public void randRoamDir()
    {
        roamDir = new Vector2(Random.Range(-3f, 3f), Random.Range(-3f, 3f)).normalized;
    }
}