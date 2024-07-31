using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class Enemy : MonoBehaviour
{
    [SerializeField] private int maxHealth = 100;
    int curentHealth;

    [Header("Pathfinding")]
    [SerializeField] private Transform target;
    [SerializeField] private float activateDistance = 50f;
    [SerializeField] private float pathUpdateSeconds = 0.5f;

    [Header("Physics")]
    [SerializeField] private float speed = 200f, jumpForce = 100f;
    [SerializeField] private float nextWaypointDistance = 3f;
    [SerializeField] private float jumpNodeHeightRequirement = 0.8f;
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
    Seeker seeker;
    Rigidbody2D rb;
    private bool isOnCoolDown;


    // Start is called before the first frame update
    void Start()
    {
        curentHealth = maxHealth;

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
            PathFollow();
        }
    }

    private void UpdatePath()
    {
        if (followEnabled && TargetInDistance() && seeker.IsDone())
        {
            seeker.StartPath(rb.position, target.position, OnPathComplete);
        }
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
            if (direction.y > jumpNodeHeightRequirement)
            {
                if (isInAir) return; 
                isJumping = true;
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
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
}
