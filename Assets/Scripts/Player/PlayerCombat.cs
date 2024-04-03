using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    public Animator animator;
    public float swordDmg = 1f;

    public GameObject MeleeHitbox;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Attack();
            Debug.Log("Input detected");
        }

        //Position hitbox to be on the right or left
        if (Input.GetAxis("Horizontal") > 0) // Right facing
        {
            MeleeHitbox.transform.localPosition = new Vector3(0.29f, 0.12f, 0);
        }
        else if (Input.GetAxis("Horizontal") < 0) // Left facing
        {
            MeleeHitbox.transform.localPosition = new Vector3(-0.29f, 0.10f, 0);
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        string colliderName = collider.gameObject.name;

        // Check if the collider's name contains "Door" but is not one of the specific doors
        if (colliderName.Contains("Door") && colliderName != "TopDoor" && colliderName != "BotDoor" && colliderName != "LeftDoor" && colliderName != "RightDoor")
        {
            collider.SendMessage("Hit", swordDmg);
        }
    }

    void Attack()
    {
        Debug.Log("Function called");
        animator.SetTrigger("swingAtk");

        MeleeHitbox.SetActive(true);


    }
}
