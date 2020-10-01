using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ouiattack : MonoBehaviour
{
    private float AttackTimer;
    public float StartAttackTimer;

    public Transform attackPos;
    public float attackRange;
    public LayerMask whatisennemy;
    public int damage;

    // Update is called once per frame
    void Update()
    {
        if (AttackTimer <= 0)
        {
            if (Input.GetKey(KeyCode.Space))
            {
                Collider2D[] enemiesToDamage = Physics2D.OverlapCircleAll(attackPos.position, attackRange, whatisennemy);
                for(int i=0; i< enemiesToDamage.Length; i++)
                {
                    enemiesToDamage[i].GetComponent<EnemyBehaviour>().TakeDamage(damage);
                }
            }
        }
         
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPos.position, attackRange);
    }
}
