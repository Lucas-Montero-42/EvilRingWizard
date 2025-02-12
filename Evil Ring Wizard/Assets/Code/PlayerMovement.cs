using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float playerSpeed = 20f;
    public float momentumDamping = 5f;
    CharacterController characterController;
    Vector3 inputVector;
    Vector3 movementVector;
    float gravity = -10f;
    public Animator camAnimator;
    public bool walking = false;

    private void Awake()
    {
        camAnimator = GetComponentInChildren<Animator>();
        characterController = GetComponent<CharacterController>();
    }
    void Update()
    {
        GetInput();
        MovePlayer();
        Animate();
    }

    private void Animate()
    {
        // Animaci�n de subir y bajar la camara al andar
        camAnimator.SetBool("Walking", walking);
    }

    private void GetInput()
    {
        // WASD para moverse
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))
        {
            walking = true;
            inputVector = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
            inputVector.Normalize();
            inputVector = transform.TransformDirection(inputVector);
        }
        // Si no se mueve...
        else
        {
            // No hay animaci�n
            walking = false;
            // Frena con inercia
            inputVector = Vector3.Lerp(inputVector, Vector3.zero, momentumDamping * Time.deltaTime);
        }
    }
    private void MovePlayer()
    {
        // Mueve al jugador seg�n el input y la velocidad
        movementVector = (inputVector * playerSpeed)+(Vector3.up * gravity);
        characterController.Move(movementVector * Time.deltaTime);
    }

}
