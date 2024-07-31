using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PushableObjectScript : MonoBehaviour
{
    Rigidbody2D rb2d;
    
    private void OnCollision2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {

           rb2d = GetComponent<Rigidbody2D>();
        }

       
    }
}
