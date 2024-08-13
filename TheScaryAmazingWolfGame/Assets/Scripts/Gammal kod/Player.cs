using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{   
    [SerializeField] private float runSpeed = 40;
    [SerializeField] private int money;
    float horizontalMove = 0;
    bool jump = false;
    public bool crouch = false;
    [SerializeField] private int health = 100;
    public int smörgåsBit;

    Vector2 reSpawnPoint;

    [SerializeField]private Rigidbody2D rb2D;
    [SerializeField]private CharacterController2D controller;
    [SerializeField]private LadderMovement ladderMovement;

    //Referens till animatorn
    [SerializeField]private Animator anim;
    private const string ANIM_IDLE = "Oldman_Idle";
    private const string ANIM_JUMP = "Oldman_Jump";
    private const string ANIM_WALK = "Oldman_Walk";
    private const string ANIM_FALL = "Oldman_Fall";
    private const string ANIM_CROUCH = "Oldman_Crouch";
    private const string ANIM_CLIMB = "Oldman_Climb";
    private string currentAnimation;
    


    //Audio
    public AudioSource audiosource;
    public AudioSource audiosource2;
    [SerializeField] private Transform groundCheckTransform;
    [SerializeField] private float groundCheckRadius;
    [SerializeField] private LayerMask groundCheckLayers;
    // Start is called before the first frame update
    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>(); //hämtar spelarens rigidbody
        controller = GetComponent<CharacterController2D>(); //hämtar lite stulen kod som gör att karaktären kan gå
        audiosource = GetComponent<AudioSource>(); //hämtar audio
        reSpawnPoint = transform.position;
    }

    void Update()
    {
        horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;

        //anim.SetFloat("Speed", Mathf.Abs(horizontalMove));

        if (Input.GetButtonDown("Jump"))
        {
            Debug.Log(horizontalMove);
            jump = true;
            //anim.SetBool("IsJumping", true);
        }

        if (Input.GetButtonDown("Crouch"))
        {
            crouch = true;
            //anim.SetBool("IsCrouching", true);
            ChangeAnimationState(ANIM_CROUCH);
        }
        else if (Input.GetButtonUp("Crouch"))
        {
            crouch = false;
            //anim.SetBool("IsCrouching", false);
        }

        PlayAnimationState();

    }

    public void OnLanding()
    {
        //anim.SetBool("IsJumping", false);
    }

    // Update is called at fixed intevalls
    void FixedUpdate()
    {
        controller.Move(horizontalMove * Time.fixedDeltaTime, crouch, jump);
        jump = false;

         if(horizontalMove < -0.1f || horizontalMove > 0.1f)
        {
            /*if (!audiosource.isPlaying)
            {
                //audiosource.Play();
            }*/
        } 
        /*else if(moveInput == 0)
        {
            //audiosource.Stop();
        }*/
        /*if (GroundCheck() == false)
        {    
            //audiosource.Stop();
        }*/

    }

    public void AddMoney(int amount)
    {
        money += amount;
    }

    public void AddSmörgåsBit(int amount)
    {
        smörgåsBit += amount;
    }


    public void TakeDamge(int damage)
    {
        health -= damage;
        //audiosource2.Play();

        if (health <= 0)
        {
            die();
        }


    }

    public void die()
    {
        SceneManager.LoadScene(4);
    }

    public void Victory()
    {
        if (money >= 1)
        {
            SceneManager.LoadScene(3);
        }
    }

    void PlayAnimationState()
    {
        anim.speed = 1;
        //En metod där vi berättar att vi vill spela upp spring animation när vi rör oss och hopp animation när vi är i luften
        if(controller.m_Grounded)
        {
            if(controller.m_Rigidbody2D.velocity.x != 0)
            {
                ChangeAnimationState(ANIM_WALK);
            }
            else
            {
                ChangeAnimationState(ANIM_IDLE);
            }
        }
        if (!controller.m_Grounded && ladderMovement.isClimbing)
        {
            ChangeAnimationState(ANIM_CLIMB);

            if (controller.m_Rigidbody2D.velocity.y != 0)
            {
                anim.speed = 1;
            }
            else
            {
                anim.speed = 0;
            }
        }

        if(!controller.m_Grounded && !ladderMovement.isClimbing)
        { 
            if(controller.m_Rigidbody2D.velocity.y > 0)
            {
                ChangeAnimationState(ANIM_JUMP);
            }
            else
            {
                ChangeAnimationState(ANIM_FALL);
            }
        }
    }
    void ChangeAnimationState(string newState)
    {
        //En metod som kollar vilken animation den spelar just nu
        if (currentAnimation == newState) return;
        anim.Play(newState);
        currentAnimation = newState;
    }
    
    /*public bool GroundCheck()
    {
        return Physics2D.OverlapCircle(groundCheckTransform.position, groundCheckRadius, groundCheckLayers);
    }*/
    

}
