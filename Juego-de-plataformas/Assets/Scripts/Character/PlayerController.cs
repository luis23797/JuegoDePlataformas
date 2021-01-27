using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Start is called before the first frame update
    private CharacterController controller;
    private Vector3 playerVelocity;
    private bool groundedPlayer;
    public float playerSpeed = 2.0f;
    public float jumpHeight = 1.0f;
    private float gravityValue = -9.81f;
    private string animacionActual;

    Animator animator;
    //Estados de animacion
    const string iddle = "iddle";
    const string walk = "walk";
    void Start()
    {
        controller = gameObject.GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {


        //Debug.Log(Input.GetButtonDown("Jump"));
        if (groundedPlayer && Input.GetButtonDown("Jump"))
        {
            jump();
        }
        
    }
    private void FixedUpdate()
    {
        groundedPlayer = controller.isGrounded;
        Debug.Log(controller.isGrounded);
        if (controller.isGrounded)
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
        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }
        Vector3 move = new Vector3(Input.GetAxis("Horizontal"), 0, 0);
        controller.Move(move * Time.deltaTime * playerSpeed);

        if (move != Vector3.zero)
        {
            gameObject.transform.forward = move;
        }

        // Changes the height position of the player..
        

        playerVelocity.y += gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);
    }
    void jump()
    {
        playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
    }
    void changeAnimationState(string nuevoEstado)
    {
        if (animacionActual == nuevoEstado) return;
        animator.Play(nuevoEstado);
        animacionActual = nuevoEstado;
    }
}

