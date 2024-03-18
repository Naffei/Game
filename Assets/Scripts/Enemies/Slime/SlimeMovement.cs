using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Assertions.Must;

public class SlimeMovement : MonoBehaviour
{
    public float moveSpeed = 2f;
    public float pushBack = 5f;
    public Rigidbody2D rb;

    private Transform player;
    private Vector3 lastPlayerPos;
    private List<Vector3> playerPositions = new List<Vector3>();
    private bool isPushed = false;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!isPushed && player != null)
        {
            if (Vector3.Distance(player.position, lastPlayerPos) > 0.1f)
            {
                // Clear list position if player moves
                playerPositions.Clear();
                playerPositions.Add(player.position);
                lastPlayerPos = player.position;
            }
            MoveToPlayer();
        }
    }

    //Move to Player
     void MoveToPlayer()
     {
        if (playerPositions.Count > 0)
        {
            Vector2 direction = ((Vector2)playerPositions[0] - rb.position).normalized;
            float distanceToPlayer = Vector2.Distance(rb.position, playerPositions[0]);

            if ( distanceToPlayer > 0.5f)
            {
                rb.MovePosition(rb.position + direction * moveSpeed * Time.fixedDeltaTime);
            }
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        // Check if there is a wall and make them collide
        if (collision.gameObject.CompareTag("Collidable"))
        {
            rb.velocity = Vector2.zero;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Detect player attack and push away
        if (collision.gameObject.name == "MeleeHitbox")
        {
            Vector2 pushbackDir = (transform.position - collision.transform.position).normalized;

            // Calculate distance between player and slime
            float distance = Vector2.Distance(transform.position, collision.transform.position);
            // Range for how far the slime gets pushed
            float maxRange = 2f;

            Vector2 pushForce = pushbackDir * pushBack;

            if (distance > maxRange)
            {
                pushForce *= Vector2.zero;
            }

            rb.AddForce(pushForce, ForceMode2D.Impulse);
            StartCoroutine(DisableMovementForDuration(0.7f));

            Debug.Log(pushForce);
        }


    }

    IEnumerator DisableMovementForDuration(float duration)
    {
        isPushed = true;
        yield return new WaitForSeconds(duration);
        isPushed = false;
        rb.velocity = Vector2.zero;
    }
}