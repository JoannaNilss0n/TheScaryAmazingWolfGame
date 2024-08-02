using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableObjectScript : MonoBehaviour
{

    [SerializeField] private int maxHealth = 100;
    int curentHealth;

    // Start is called before the first frame update
    void Start()
    {
        curentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void TakeDamage(int damage)
    {
        curentHealth -= damage;

        // play hurt animation

        if (curentHealth <= 0)
        {
            Die();
        }

        void Die()
        {
            // Die aniamation

            Destroy(gameObject);
        }
    }
}
