using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    public Animator animator;


    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Attack();
            Debug.Log("Input detected");
        }
    }

    void Attack()
    {
        Debug.Log("Function called");
        animator.SetTrigger("swingAtk");
    }
}
