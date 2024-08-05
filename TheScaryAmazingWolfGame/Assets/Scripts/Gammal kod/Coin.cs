using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ( collision.gameObject.CompareTag("Player"))
        {
            Player player = collision.gameObject.GetComponent<Player>();
            Enemy enemy = collision.gameObject.GetComponent<Enemy>();
            player.AddMoney(1);
            
            enemy.Sandwich();
            //Förstör myntet när spelaren har samlat det
            Destroy(gameObject);
        }
        else
        {
            return;
        }
        
        // gameObject: inbygd funktion, refererar till gameobject:et som klassen ligger i
        // HÄR: gameObject = coin
    }
}
