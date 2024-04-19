using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    public Animator animator;
    public float swordDmg = 1f;
    public float knockbackForce = 1000f;

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
        if (Input.GetAxis("Horizontal") > 0) // Right
        {
            MeleeHitbox.transform.localPosition = new Vector3(0.29f, 0.12f, 0);
        }
        else if (Input.GetAxis("Horizontal") < 0) // Left
        {
            MeleeHitbox.transform.localPosition = new Vector3(-0.29f, 0.10f, 0);
        }
    }

    void Attack()
    {
        Debug.Log("Function called");
        animator.SetTrigger("swingAtk");

        MeleeHitbox.SetActive(true);
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        IAttackable attackAble = collider.GetComponent<IAttackable>();

        if (attackAble != null)
        {
            Vector3 swordPos = transform.position;
            Vector2 direction = (Vector2)(swordPos - collider.gameObject.transform.position).normalized;

            Vector2 knockback = direction * knockbackForce;

            Debug.Log("Colliding with: " + collider.gameObject.name);
            Debug.Log("Applying knockback: " + knockback);

            attackAble.Hit(swordDmg, knockback);
        }
    }


}
