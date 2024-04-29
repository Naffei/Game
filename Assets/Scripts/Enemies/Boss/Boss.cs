using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Boss : MonoBehaviour, IAttackable
{
    public Rigidbody2D rb;
    private Transform player;
    public float moveSpeed = 2f;
    public float atkMoveSpeed = 8f;
    public float maxHP = 15;
    public float currentHealth;

    public GameObject BossMelee;
    public float atkDmg;
    public float knockbackForce = 1000f;

    public GameObject roomBoundary;
    public Animator animator;

    public AudioSource audioSource;
    public AudioClip atkClip;    
    public AudioClip hitClip;

    private bool isIdle;
    private bool isAttacking;
    private bool isOnCooldown = false;
    public Vector2 roamDir;

    public GameObject exitPrefab;


    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        currentHealth = maxHP;
        player = GameObject.FindGameObjectWithTag("Player").transform;

        StartCoroutine(IdlePhase());
    }

    void Update()
    {
        if (IsPlayerInRoom(player.position))
        {
            Vector2 directionToPlayer = (Vector2)player.position - rb.position;
            RotateTowardsPlayer(directionToPlayer);
        }

    }


    IEnumerator IdlePhase()
    {
        Debug.Log("Entered Idle");

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

        // Switch to attack phase if not in cooldown
        if (!isOnCooldown && IsPlayerInRoom(player.position))
        {
            StartCoroutine(AttackPhase());
        }
        else
        {
            StartCoroutine (IdlePhase());
        }

    }

    IEnumerator CooldownPhase()
    {
        float cooldownTime = Random.Range(3f, 5f);

        // Cooldown timer
        while ( cooldownTime > 0) {
            cooldownTime -= Time.deltaTime;
            Debug.Log(cooldownTime);
            yield return null;
        }

        yield return new WaitForSeconds(cooldownTime);

        isOnCooldown = false;
    }

    IEnumerator AttackPhase()
    {
        Debug.Log("Entered Atk");

        isAttacking = true;

        while (IsPlayerInRoom(player.position))
        {
            Vector2 directionToPlayer = (Vector2)player.position - rb.position;
            Vector2 perpendicularDirection = new Vector2(-directionToPlayer.y, directionToPlayer.x).normalized;
            Vector2 bossPosition = (Vector2)player.position + perpendicularDirection * 2.0f;
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
            audioSource.clip = atkClip;
            audioSource.Play();

            // Start the cooldown phase
            isAttacking = false;
            isOnCooldown = true;
            StartCoroutine(CooldownPhase());
            StartCoroutine(IdlePhase());

            yield break;

        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            float atkDmg = Random.Range(1, 2);
            Debug.Log("Damage: " + atkDmg);

            Vector2 direction = (transform.position - other.transform.position).normalized;
            Vector2 knockback = knockbackForce * direction;
            if (other.transform.position.x > transform.position.x)
            {
                knockback = new Vector2(-Mathf.Abs(knockback.x), knockback.y);
            }
            else
            {
                knockback = new Vector2(Mathf.Abs(knockback.x), knockback.y);
            }

            IAttackable attackable = other.gameObject.GetComponent<IAttackable>();
            if (attackable != null)
            {
                audioSource.clip = hitClip;
                audioSource.Play();
                attackable.Hit(atkDmg);
                rb.AddForce(knockback);
            }

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
        animator.SetTrigger("hit");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void Hit(float damageTaken)
    {
        currentHealth -= damageTaken;
        animator.SetTrigger("hit");

        if (currentHealth <= 0) 
        {
            Die();
        }
    }

    void Die()
    {
        Destroy(gameObject);
        exitPrefab.SetActive(true);
    }

    public void randRoamDir()
    {
        roamDir = new Vector2(Random.Range(-3f, 3f), Random.Range(-3f, 3f)).normalized;
    }
}