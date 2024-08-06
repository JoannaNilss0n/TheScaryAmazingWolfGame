using UnityEngine;

public class Tor : MonoBehaviour
{
    Rigidbody2D rb;

    [SerializeField] float runSpeed;
    [SerializeField] float horizontalMove;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        horizontalMove = Input.GetAxisRaw("Horizontal");
    }

    void FixedUpdate()
    {
        if (rb.velocity.x >= runSpeed || rb.velocity.x <= -runSpeed)
        {
            return;
        }
        rb.velocity = new Vector2(rb.velocity.x + horizontalMove/10, rb.velocity.y);
    }
}
