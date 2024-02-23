using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    public Animator animator;
    public BoxCollider2D MeleeHitbox;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        //Get user input and call attack function
        if (Input.GetKeyDown(KeyCode.Space))
        {
                Attack();
        }
    }
            
    void Attack()
    {
        //call atk state from animator
        Debug.Log("Function called");
        animator.SetTrigger("swingAtk");
    }

    public void EnableHitbox()
    {
        MeleeHitbox.enabled = true;
    }

    public void DisableHitbox()
    {
        MeleeHitbox.enabled = false;
    }
}