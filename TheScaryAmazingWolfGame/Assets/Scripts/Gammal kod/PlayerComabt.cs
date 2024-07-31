using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    [SerializeField] private Transform attackPoint;
    [SerializeField] private float attackRange = 1;
    [SerializeField] private LayerMask enemyLayers;
    [SerializeField] private int attackDamage = 40;
    [SerializeField] private float attackRate = 2;
    float nextAttackTime = 0;

    [SerializeField]private Animator anim;

    public AudioSource audiosource;


    void Start()
    {
        //audiosource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time >= nextAttackTime)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Attack();
                nextAttackTime = Time.time + 1 / attackRate;
                //Debug.Log("Attack");

                // attack audio:
                if(!audiosource.isPlaying)
                {
                    audiosource.Play();
                }
            } 
        } 



    }

    public void Attack()
    {
        // Attack animation
        anim.SetTrigger("Attack");
        // upptack fiender
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);
        // Skada fiender
        foreach(Collider2D enemy in hitEnemies)
        {
            enemy.GetComponent<Enemy>().TakeDamage(attackDamage);
        }


    }

    void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;

        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }

    

}
