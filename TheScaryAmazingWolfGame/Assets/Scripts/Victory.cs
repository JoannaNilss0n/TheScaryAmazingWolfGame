using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Victory : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ( collision.gameObject.CompareTag("Player"))
        {
            Player player = collision.gameObject.GetComponent<Player>();
            

            player.Victory();
            
            

            //Förstör myntet när spelaren har samlat det
            
        }
        
        // gameObject: inbygd funktion, refererar till gameobject:et som klassen ligger i
        // HÄR: gameObject = coin
    }
}
