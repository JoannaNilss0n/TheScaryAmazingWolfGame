using UnityEngine;

public class Coin : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ( collision.gameObject.CompareTag("Player"))
        {
            Player player = collision.gameObject.GetComponent<Player>();
            Enemy enemy = GameObject.FindGameObjectWithTag("Enemy").gameObject.GetComponent<Enemy>();

            if (enemy != null) player.AddMoney(1);
            
            if (enemy != null) enemy.Sandwich();

            //Förstör myntet när spelaren har samlat det
            Destroy(gameObject);
        }
        
        // gameObject: inbygd funktion, refererar till gameobject:et som klassen ligger i
        // HÄR: gameObject = coin
    }
}
