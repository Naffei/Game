using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions.Must;

public class SlimeMovement : MonoBehaviour
{
    public float moveSpeed = 2f;
    public Rigidbody2D rb;

    private Transform player;
    private Vector3 lastPlayerPos;
    private List<Vector3> playerPositions = new List<Vector3>();

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (player != null)
        {
            if (Vector3.Distance(player.position, lastPlayerPos) > 0.1f)
            {
                // Clear the list position if player moves again
                playerPositions.Clear();
                playerPositions.Add(player.position);
                lastPlayerPos = player.position;
            }

            // Calculate and move to player's positions
            MoveToPlayer();
        }
    }

    void MoveToPlayer()
    {
        if (playerPositions.Count > 0)
        {
            rb.simulated = true;
            Vector2 direction = ((Vector2)playerPositions[0] - rb.position).normalized;
            rb.MovePosition(rb.position + direction * moveSpeed * Time.fixedDeltaTime);

            if (Vector2.Distance(rb.position, playerPositions[0]) < 0.1f)
            {
                // Remove the reached position from the list
                playerPositions.RemoveAt(0);
            }
        }
        else
        {
            // If there are no positions in the list, disable Rigidbody
            rb.simulated = false;
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        // Check if there is a wall and make player stop
        if (collision.gameObject.CompareTag("Collidable"))
        {
            rb.velocity = Vector2.zero;
        }
    }
}