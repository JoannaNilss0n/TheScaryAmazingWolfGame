using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyTrap : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ( collision.gameObject.CompareTag("Player"))
        {
            Player player = collision.gameObject.GetComponent<Player>();
            Enemy enemy = GameObject.FindGameObjectWithTag("Enemy").gameObject.GetComponent<Enemy>();
            
            if (enemy != null) enemy.FlyTrap();
        }
        
        // gameObject: inbygd funktion, refererar till gameobject:et som klassen ligger i
        // HÄR: gameObject = coin
    }
}
