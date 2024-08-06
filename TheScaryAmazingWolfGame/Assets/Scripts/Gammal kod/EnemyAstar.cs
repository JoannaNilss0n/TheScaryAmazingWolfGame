using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyAstar : MonoBehaviour
{
    [SerializeField] private int maxHealth = 100;
    int curentHealth;

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

     private void FixedUpdate()
    {
        if (TargetInDistance() && followEnabled)
        {
            speed = 9;
            PathFollow();
        }
        /*if (TargetInDistance())
        {
            if (transform.position.x < target.position.x)
            {
                rb.velocity = new Vector2(speed, 0);
                transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            }
            else
            {
                rb.velocity = new Vector2(-speed, 0);
                transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x)*-1f, transform.localScale.y, transform.localScale.z);
            }

            PathFollow();
        }*/
        else
        {
            speed = 2;

            //Vector2 point = curentPoint.position - transform.position;

            if (curentPoint == B.transform)
            {
                rb.velocity = new Vector2(speed, 0);
            }
            else
            {
                rb.velocity = new Vector2(-speed, 0);
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

        
    }

    private void UpdatePath()
    {
        if (followEnabled && TargetInDistance() && seeker.IsDone())
        {
            seeker.StartPath(rb.position, target.position, OnPathComplete);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(A.transform.position, 0.5f);
        Gizmos.DrawWireSphere(B.transform.position, 0.5f);
        Gizmos.DrawLine(A.transform.position, B.transform.position);
        Gizmos.DrawLine(startOffset + (new Vector3(0f, 0.2f, 0f)), startOffset + (new Vector3(0f, 0.2f, 0f)) + Vector3.right*transform.localScale.x);
        Gizmos.DrawWireSphere(transform.position, activateDistance);
    }

    private void PathFollow()
    {
        if (path == null)
        {
            return;
        }

        // Reached end of path
        if (currentWaypoint >= path.vectorPath.Count)
        {
            return;
        }

        // See if colliding with anything
        startOffset = transform.position - new Vector3(0f, GetComponent<Collider2D>().bounds.extents.y + jumpCheckOffset, transform.position.z);
        isGrounded = Physics2D.Raycast(startOffset, -Vector3.up, 0.05f);

        // Direction Calculation
        Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;
        Vector2 force = direction * speed;

        // Jump
        if (jumpEnabled && isGrounded && !isInAir && !isOnCoolDown)
        {
            if (direction.y > jumpNodeHeightRequirement || JumpCheck())
            {
                if (isInAir) return; 
                isJumping = true;
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                //rb.AddForce(Vector3.up*jumpForce*100);
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

        // Movement
        rb.velocity = new Vector2(force.x, rb.velocity.y);

        // Next Waypoint
        float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);
        if (distance < nextWaypointDistance)
        {
            currentWaypoint++;
        }

        // Direction Graphics Handling
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
}
