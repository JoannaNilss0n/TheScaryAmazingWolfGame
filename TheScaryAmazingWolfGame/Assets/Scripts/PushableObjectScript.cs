using UnityEngine;

public class PushableObjectScript : MonoBehaviour
{
    public Rigidbody2D rb2d;

    private void Awake()
    {
        if (rb2d == null) // Kollar om rb2d är tom, eller "null", alltså att den inte redan har en RigidBody kopplad till sig.
        {
            rb2d = GetComponent<Rigidbody2D>(); // Kollar om det finns en Rigidbody2D component på gameobjektet som script:et sitter på.
        }
    }

    private void OnCollisionEnter2D(Collision2D collision) // Den här metoden kallas i början av en kollision, data:n från kollitionen sparas i variablen collision.
    {
        if (collision.gameObject.CompareTag("Player")) // kollar på kollisionen, på vilket gameobject som kolliderade och kollar om den har tagen "Player"
        {
            rb2d.bodyType = RigidbodyType2D.Dynamic; // Ändrar RigidBody typen till dynamic så att spelaren kan förflytta objektet.
        }
    }

    private void OnCollisionExit2D(Collision2D collision) // Den här metoden kallas i slutet av en kollision.
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            rb2d.bodyType = RigidbodyType2D.Static; // Ändrar RigidBody typen till Static så att ingen annan än spelaren kan förflytta objektet.
        }
    }
}
