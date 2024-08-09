using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class Enemy : MonoBehaviour
{
    [SerializeField] private int maxHealth = 100;
    int curentHealth;
    [SerializeField] private Vector2 ChasePoint;


    [Header("Patrolling")]
    [SerializeField] private GameObject A;
    [SerializeField] private GameObject B;
    [SerializeField] private Transform curentPoint;

    [Header("Pathfinding")]
    [SerializeField] private Transform target;
    [SerializeField] private float activateDistance = 50f;
    [SerializeField] private float pathUpdateSeconds = 0.5f;

    [Header("Physics")]
    [SerializeField] private float speed = 200f, jumpForce = 100f;
    [SerializeField] private float nextWaypointDistance = 3f;
    [SerializeField] private float jumpNodeHeightRequirement = 0.6f;
    [SerializeField] private float jumpModifier = 0.3f;
    [SerializeField] private float jumpCheckOffset = 0.1f;

    [Header("Custom Behavior")]
    [SerializeField] private bool followEnabled = true;
    [SerializeField] private bool jumpEnabled = true, isJumping, isInAir;
    [SerializeField] private bool directionLookEnabled = true;

    [SerializeField] Vector3 startOffset;

    private Path path;
    private int currentWaypoint = 0;
    [SerializeField] private RaycastHit2D isGrounded;
    [SerializeField] private LayerMask groundLayerMask;
    Seeker seeker;
    Rigidbody2D rb;
    private bool isOnCoolDown;


    // Start is called before the first frame update
    void Start()
    {
        curentHealth = maxHealth;

        ChasePoint = transform.position;

        curentPoint = B.transform;

        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
        isJumping = false;
        isInAir = false;
        isOnCoolDown = false; 

        InvokeRepeating("UpdatePath", 0f, pathUpdateSeconds);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Sandwich()
    {
        //FlytaVargen();
        transform.position = ChasePoint;
        activateDistance = 100;
        speed = 9;
    }

    /*private void FlytaVargen()
    {
        transform.position = ChasePoint;
        activateDistance = 100;
        speed = 9;
    }*/

     private void FixedUpdate()
    {
        startOffset = transform.position - new Vector3(0f, GetComponent<Collider2D>().bounds.extents.y + jumpCheckOffset, transform.position.z);
        isGrounded = Physics2D.Raycast(startOffset, -Vector3.up, 0.05f);

        // Jump
        if (jumpEnabled && isGrounded && !isInAir && !isOnCoolDown)
        {
            if (JumpCheck())
            {
                if (isInAir) return; 
                isJumping = true;
                //rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                rb.AddForce(Vector3.up*jumpForce*10);
                StartCoroutine(JumpCoolDown());

            }
        }
        if (isGrounded)
        {
            isJumping = false;
            isInAir = false; 
        }
        else
        {
            isInAir = true;
        }

        if (TargetInDistance())
        {
            if (transform.position.x < target.position.x)
            {
                rb.velocity = new Vector2(speed, rb.velocity.y);
            }
            else
            {
                rb.velocity = new Vector2(-speed, rb.velocity.y);
            }
            speed = 9;
        }
        else
        {
            speed = 3;

            if (curentPoint == B.transform)
            {
                rb.velocity = new Vector2(speed, rb.velocity.y);
            }
            else
            {
                rb.velocity = new Vector2(-speed, rb.velocity.y);
            }

            if (Vector2.Distance(transform.position, curentPoint.position) < 0.5f || transform.position.x >= B.transform.position.x)
            {
                
                curentPoint = A.transform;
            }

            if (Vector2.Distance(transform.position, curentPoint.position) < 0.5f || transform.position.x <= A.transform.position.x)
            {
                curentPoint = B.transform;
            }
        }

        if (directionLookEnabled)
        {
            if (rb.velocity.x > 0.05f)
            {
                transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            }
            else if (rb.velocity.x < -0.05f)
            {
                transform.localScale = new Vector3(-1f * Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            }
        }

        
    }

    /*private void UpdatePath()
    {
        if (followEnabled && TargetInDistance() && seeker.IsDone())
        {
            seeker.StartPath(rb.position, target.position, OnPathComplete);
        }
    }*/

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(A.transform.position, 0.5f);
        Gizmos.DrawWireSphere(B.transform.position, 0.5f);
        Gizmos.DrawLine(A.transform.position, B.transform.position);
        Gizmos.DrawLine(startOffset + new Vector3(0f, 0.2f, 0f), startOffset + new Vector3(0f, 0.2f, 0f) + Vector3.right*transform.localScale.x);
        Gizmos.DrawWireSphere(transform.position, activateDistance);
    }

    private bool TargetInDistance()
    {
        return Vector2.Distance(transform.position, target.transform.position) < activateDistance;
    }

    private void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }

    IEnumerator JumpCoolDown()
    {
        isOnCoolDown = true; 
        yield return new WaitForSeconds(1f);
        isOnCoolDown = false;
    }



    public void TakeDamage(int damage)
    {
        curentHealth -= damage;

        // play hurt animation

        if (curentHealth <= 0 )
        {
            Die();
        }
    }

    void Die()
    {
        // Die aniamation

        Destroy(gameObject);
    }

    bool JumpCheck()
    {
        return Physics2D.Raycast(startOffset + (new Vector3(0f, 0.2f, 0f)), Vector2.right*transform.localScale.x, 0.5f, groundLayerMask);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ( collision.gameObject.CompareTag("Player"))
        {
            Player player = collision.gameObject.GetComponent<Player>();
            Enemy enemy = GameObject.FindGameObjectWithTag("Enemy").gameObject.GetComponent<Enemy>();

            player.TakeDamge(1000);
        }
        
        // gameObject: inbygd funktion, refererar till gameobject:et som klassen ligger i
        // HÄR: gameObject = coin
    }

    public void FlyTrap()
    {
        activateDistance = 100;
        speed = 9;
    }
}
