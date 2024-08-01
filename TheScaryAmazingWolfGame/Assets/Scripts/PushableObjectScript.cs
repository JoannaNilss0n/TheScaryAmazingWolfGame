using UnityEngine;

public class PushableObjectScript : MonoBehaviour
{
    public Rigidbody2D rb2d;

    private void Awake()
    {
        if (rb2d == null) // Kollar om rb2d �r tom, eller "null", allts� att den inte redan har en RigidBody kopplad till sig.
        {
            rb2d = GetComponent<Rigidbody2D>(); // Kollar om det finns en Rigidbody2D component p� gameobjektet som script:et sitter p�.
        }
    }

    private void OnCollisionEnter2D(Collision2D collision) // Den h�r metoden kallas i b�rjan av en kollision, data:n fr�n kollitionen sparas i variablen collision.
    {
        if (collision.gameObject.CompareTag("Player")) // kollar p� kollisionen, p� vilket gameobject som kolliderade och kollar om den har tagen "Player"
        {
            rb2d.bodyType = RigidbodyType2D.Dynamic; // �ndrar RigidBody typen till dynamic s� att spelaren kan f�rflytta objektet.
        }
    }

    private void OnCollisionExit2D(Collision2D collision) // Den h�r metoden kallas i slutet av en kollision.
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            rb2d.bodyType = RigidbodyType2D.Static; // �ndrar RigidBody typen till Static s� att ingen annan �n spelaren kan f�rflytta objektet.
        }
    }
}
