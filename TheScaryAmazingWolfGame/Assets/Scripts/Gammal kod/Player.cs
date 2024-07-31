using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{   
    [SerializeField] private float runSpeed = 40;
    [SerializeField] private int money;
    float horizontalMove = 0;
    bool jump = false;
    public bool crouch = false;
    [SerializeField] private int health = 100;

    Vector2 reSpawnPoint;

    [SerializeField]private Rigidbody2D rb2D;
    [SerializeField]private CharacterController2D controller;

    //Referens till animatorn
    [SerializeField]private Animator anim;
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
        controller = GetComponent<CharacterController2D>(); //hätar lite stulen kod som gör att karaktären kan gå
        //audiosource = GetComponent<AudioSource>(); //hämtar audio
        reSpawnPoint = transform.position;
    }

    void Update()
    {
        horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;

        anim.SetFloat("Speed", Mathf.Abs(horizontalMove));

        if (Input.GetButtonDown("Jump"))
        {
            jump = true;
            anim.SetBool("IsJumping", true);
        }

        if (Input.GetButtonDown("Crouch"))
        {
            crouch = true;
            anim.SetBool("IsCrouching", true);
        }
        else if (Input.GetButtonUp("Crouch"))
        {
            crouch = false;
            anim.SetBool("IsCrouching", false);
        }

    }

    public void OnLanding()
    {
        anim.SetBool("IsJumping", false);
    }

    // Update is called at fixed intevalls
    void FixedUpdate()
    {
        controller.Move(horizontalMove * Time.fixedDeltaTime, crouch, jump);
        jump = false;

         if(horizontalMove < -0.1f || horizontalMove > 0.1f)
        {
            if (!audiosource.isPlaying)
            {
                audiosource.Play();
            }
        } 
        else //if(moveInput == 0)
        {
            audiosource.Stop();
        }
        if (GroundCheck() == false)
        {    
            audiosource.Stop();
        }

    }

    public void AddMoney(int amount)
    {
        money += amount;
    }


    public void TakeDamge(int damage)
    {
        health -= damage;
        audiosource2.Play();

        if (health <= 0)
        {
            die();
        }


    }

    public void die()
    {
        transform.position = reSpawnPoint;
        health = 100;
    }
    
    public bool GroundCheck()
    {
        return Physics2D.OverlapCircle(groundCheckTransform.position, groundCheckRadius, groundCheckLayers);
    }
    

}
