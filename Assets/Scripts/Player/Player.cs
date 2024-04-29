
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;
using UnityEditor;

public class Player : MonoBehaviour, IAttackable
{
    public Walk walkScript;

    public float maxHP = 10;
    public float currentHealth;
    public HealthBar healthBar;

    private bool isInputEnabled = true;
    public float moveSpeed = 3f;
    public Rigidbody2D rb;

    Vector2 movement;

    public Animator animator;
    public float swordDmg = 1f;
    public float knockbackForce = 1000f;
    public GameObject playerUI;

    private bool isPlayingSound = false;
    private bool isAttacking = false;
    public AudioSource audioSource;
    public AudioClip atkClip;
    public AudioClip hitClip;

    public AudioClip drinkSound;
    public AudioClip pickUp;

    public AudioClip stoneWalk;
    public AudioClip grassWalk;
    public AudioClip clingWalk;


    public GameObject MeleeHitbox;

    public int potionCount = 1;
    public PotionCounter potionCounter;

    public GameObject exitPrefab;
    public GameObject gameOverCanvas;

    public void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        potionCounter = FindObjectOfType<PotionCounter>();
        currentHealth = maxHP;

        healthBar.player = this;
        healthBar.PopulateHealthBar();

        audioSource = GetComponent<AudioSource>();
    }


    // Update is called once per frame
    void Update()
    {
        if (isInputEnabled)
        {
            rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
        }
        else
        {
            rb.velocity = Vector2.zero;
        }


        if (isInputEnabled)
        {
            // User Inputs
            movement.x = Input.GetAxisRaw("Horizontal");
            movement.y = Input.GetAxisRaw("Vertical");
            animator.SetFloat("Vertical", movement.y);
            animator.SetFloat("Horizontal", movement.x);
            animator.SetFloat("Speed", movement.sqrMagnitude);

            if (movement.magnitude > 0 && !isPlayingSound && !isAttacking)
            {
                int randomIndex = UnityEngine.Random.Range(0, 3);
                switch (randomIndex)
                {
                    case 0:
                        StartCoroutine(PlaySound(stoneWalk));
                        break;
                    case 1:
                        StartCoroutine(PlaySound(grassWalk));
                        break;
                    case 2:
                        StartCoroutine(PlaySound(clingWalk));
                        break;
                    default:
                        break;
                }
            }



            if (movement.x > 0)
            {
                transform.localScale = new Vector2(1, 1);
            }
            else if (movement.x < 0)
            {
                transform.localScale = new Vector2(-1, 1);
            }
        }

        // Attack
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Attack();
            Debug.Log("Input detected");
        }

        // Use Potion
        if (Input.GetKeyDown(KeyCode.E))
        {
            UseHealthPotion();
        }

    }

    IEnumerator PlaySound(AudioClip clip)
    {
        isPlayingSound = true;
        audioSource.PlayOneShot(clip);
        yield return new WaitForSeconds(0.25f);
        isPlayingSound = false;
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
        }
        string objectName = other.gameObject.name;

        if (objectName.Equals("Portal") || other.CompareTag("Portal"))
        {
            TriggerGameOver();
        }

        IAttackable attackAble = other.GetComponent<IAttackable>();

        if (attackAble != null)
        {
            Vector3 pushPos = transform.position;
            Vector2 direction = ((Vector2)pushPos - (Vector2)other.transform.position).normalized;
            Vector2 knockback = direction * knockbackForce;

            if (other.transform.position.x < transform.position.x)
            {
                knockback = new Vector2(-Mathf.Abs(knockback.x), knockback.y);
            }
            else
            {
                knockback = new Vector2(Mathf.Abs(knockback.x), knockback.y);
            }

            Debug.Log("Applying knockback: " + knockback);

            attackAble.Hit(swordDmg, knockback);
            audioSource.clip = hitClip;
            audioSource.Play();
        }
    }

    void TriggerGameOver()
    {
        Time.timeScale = 0f;
        playerUI.SetActive(false);
        gameOverCanvas.SetActive(true);
    }

    void Attack()
    {
        isAttacking = true;
        Debug.Log("Attack Called: ");
        isInputEnabled = false;
        StartCoroutine(DisableMovementForDuration(0.18f));

        animator.SetTrigger("swingAtk");
        audioSource.clip = atkClip;
        audioSource.Play();

        MeleeHitbox.SetActive(true);
    }

    public void Hit(float damageTaken, Vector2 knockback)
    {
        currentHealth -= damageTaken;

        if (healthBar != null)
        {
            healthBar.UpdateHealthBar(currentHealth);
        }

        rb.AddForce(knockback);

        rb.velocity = Vector2.zero;
        rb.angularVelocity = 0f;
        StartCoroutine(DisableMovementForDuration(0.25f));
        isInputEnabled = false;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void Hit(float damageTaken)
    {
        currentHealth -= damageTaken;

        if (healthBar != null)
        {
            healthBar.UpdateHealthBar(currentHealth);
        }

        rb.velocity = Vector2.zero;
        rb.angularVelocity = 0f;
        StartCoroutine(DisableMovementForDuration(0.25f));
        isInputEnabled = false;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    IEnumerator DisableMovementForDuration(float duration)
    {
        StartCoroutine(DisableSound(0.35f));
        yield return new WaitForSeconds(duration);
        rb.velocity = Vector2.zero;
        isInputEnabled = true;
    }

    IEnumerator DisableSound(float duration)
    {
        yield return new WaitForSeconds(duration);
        isAttacking = false;
    }

    void Die()
    {
        isInputEnabled = false;
        StartCoroutine(PlayDeathAnimation());
    }

    IEnumerator PlayDeathAnimation()
    {
        animator.SetTrigger("Die");
        yield return new WaitForSeconds(0.8f);
        Time.timeScale = 0f;
        gameOverCanvas.SetActive(true);
    }


    // Modifiers / Items
    public void IncreaseDamage()
    {
        swordDmg += 1f;
        audioSource.clip = pickUp;
        audioSource.Play();
    }

    public void AddHealthPotion()
    {
        potionCount++;
        audioSource.clip = pickUp;
        audioSource.Play();

        if (potionCounter != null)
        {
            potionCounter.UpdatePotionCount();
        }
    }

    public void UseHealthPotion()
    {
        if (currentHealth < maxHP && potionCount > 0)
        {
            Debug.Log("Potion used");
            currentHealth++;
            audioSource.clip = drinkSound;
            audioSource.Play();
            if (healthBar != null)
            {
                healthBar.UpdateHealthBar(currentHealth);
            }

            potionCount--;
            potionCounter.UpdatePotionCount();
            Debug.Log("Potions remaining: " + potionCount);


        }
        else if (currentHealth == maxHP)
        {
            Debug.Log("Player is already at max hp");
        }
        else
        {
            Debug.Log("No potions reamin");
        }
    }
}