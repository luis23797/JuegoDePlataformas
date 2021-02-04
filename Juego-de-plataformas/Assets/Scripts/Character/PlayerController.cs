using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Start is called before the first frame update
    private CharacterController controller;
    private Vector3 playerVelocity;
    private bool groundedPlayer;    //Checar si el jugador esta en el suelo
    public float playerInitialSpeed = 4.0f; //Velocidad maxima al caminar 
    public float playerSpeed = 4.0f; //Velocidad maxima actual del jugador
    public float playerNewSpeed=0; //Velocidad actual del jugador
    public float jumpHeight = 1.0f; //Altura de salto
    public float gravityValue = -9.81f; //Gravedad del jugador
    private string animacionActual; //Animacion del jugador
    public float playerRunSpeed = 11.0f;    //Velocidad maxima al correr
    public bool running = false;    //Estado de correr
    public float acelPerSecond; //Cantidad de aceleracion a aumentar por segundo para llegar a la maxima
    private float timeToMaxSpd; //Tiempo para llegar a la velocidad maxima vease la funcion setTimeToMaxSpeed
    public bool banderaSalto=false;
    public bool noInputSlide = false;
    Animator animator;
    //Estados de animacion
    const string iddle = "iddle";
    const string walk = "walk";
    const string fall = "fall";
    const string prun = "prun";
    void Start()
    {
        controller = gameObject.GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {


        
        //Acciones del jugador cuando esta en el piso
        if (groundedPlayer)
        {
            if (Input.GetButtonDown("Jump"))
            {
                jump();
            }
            if (Input.GetButton("Run") && Input.GetAxis("Horizontal") != 0)
            {
                changeAnimationState(prun);
                running = true;
                run();

            }
            if (Input.GetAxis("Horizontal") == 0)
            {
                running = false;
            }
            if (!Input.GetButton("Run") && running)
            {
                running = false;
                changeAnimationState(walk);
            }
        }
        if (Input.GetAxis("Horizontal") != 0)
        {
            /*playerNewSpeed += acelPerSecond * Time.deltaTime;
            playerNewSpeed = Mathf.Min(playerNewSpeed, playerSpeed);*/
           
            if (playerNewSpeed<playerSpeed)
            {
                playerNewSpeed += acelPerSecond * Time.deltaTime;
            }
        }
       /*else
        {
           
            playerNewSpeed = 0;
        }*/
    }
    private void FixedUpdate()
    {
        setTimeToMaxSpeed();
       
        if (playerNewSpeed < 0)
        {
            noInputSlide = true;
            playerNewSpeed = 0;
        }
        if (playerNewSpeed>playerSpeed && !noInputSlide)
        {
            playerNewSpeed = Mathf.Round(playerNewSpeed);
        }

        if (!running || !controller.isGrounded)
        {
            playerSpeed = playerInitialSpeed;
        }
        groundedPlayer = controller.isGrounded;
        if (playerNewSpeed==0)
        {
            banderaSalto = false;
        }
        if (groundedPlayer && !running)
        {
            if (Input.GetAxis("Horizontal")!=0)
            {
                changeAnimationState(walk);
            }
            else
            {
                changeAnimationState(iddle);
            }
           
        }
        if (!groundedPlayer)
        {
            changeAnimationState(fall);
        }
        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }

        Vector3 move = new Vector3(Input.GetAxis("Horizontal"), 0, 0);
        

        if (!groundedPlayer)
        {
            banderaSalto = true;
            controller.Move(move * Time.deltaTime * (playerNewSpeed));
        }
        if (Input.GetAxis("Horizontal") == 0 && groundedPlayer)
        {
            if (playerNewSpeed <= 4)
            {
                playerNewSpeed = 0;
                noInputSlide = false;
            }
            if (playerNewSpeed > 4)
            {
                noInputSlide = true;
            }
            NoInputSlide();
            
        }
            if (Input.GetAxis("Horizontal") >= -.4 && Input.GetAxis("Horizontal") <= .4 && playerNewSpeed>1 && groundedPlayer)
        {
            if (playerNewSpeed <= 4)
            {
                playerNewSpeed = 0;
                noInputSlide = false;
            }
            if (playerNewSpeed>4)
            {
                noInputSlide = true;
            }
            NoInputSlide();
        }
        else
        {
            controller.SimpleMove(move * playerNewSpeed);
        }
       

        if (move != Vector3.zero)
        {
            gameObject.transform.forward = move;
        }

        // Changes the height position of the player..

        setGravity();
        
    }
    void setGravity()
    {
        playerVelocity.y += gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);
    }
    void jump()
    {
        if (playerSpeed==playerInitialSpeed)
        {
            playerNewSpeed /= 2;
        }
        playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
    }
    void changeAnimationState(string nuevoEstado)
    {
        if (animacionActual == nuevoEstado) return;
        animator.Play(nuevoEstado);
        animacionActual = nuevoEstado;
    }
    void run()
    {
        playerSpeed = playerRunSpeed;
        
    }
    void setTimeToMaxSpeed()
    {
        if (playerSpeed == playerInitialSpeed)
        {
            timeToMaxSpd = .3f;
        }
        if (playerSpeed == playerRunSpeed)
        {
            timeToMaxSpd = 1.2f;
        }
        acelPerSecond = playerSpeed / timeToMaxSpd;
    }
    void NoInputSlide()
    {
        
        float  res = playerNewSpeed *2;
        if (playerNewSpeed > 0)
        {
            playerNewSpeed -= res * Time.deltaTime;
            
        }
        controller.SimpleMove(gameObject.transform.forward * playerNewSpeed);
    }
}

