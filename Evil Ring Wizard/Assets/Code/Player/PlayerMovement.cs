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

    //MovementPrediction
    [SerializeField]
    [Range(0.1f, 5f)]
    private float historicalPositionDuration = 1f;
    [SerializeField]
    [Range(0.001f, 1f)]
    private float historicalPositionInterval = 0.1f;

    private Queue<Vector3> historicalVelocities;
    private float lastPositionTime;
    private int maxQueueSize;

    public Vector3 averageVelocity
    {
        get
        {
            Vector3 average = Vector3.zero;
            foreach (Vector3 velocity in historicalVelocities)
            {
                average += velocity;
            }
            average.y = 0;
            return average / historicalVelocities.Count;
        }
    }

    private void Awake()
    {
        camAnimator = GetComponentInChildren<Animator>();
        characterController = GetComponent<CharacterController>();
        maxQueueSize = Mathf.CeilToInt(1f / historicalPositionInterval * historicalPositionDuration);
        historicalVelocities = new Queue<Vector3>(maxQueueSize);
    }
    void Update()
    {
        GetInput();
        MovePlayer();
        Animate();
        PredictMovement();
    }

    private void Animate()
    {
        // Animación de subir y bajar la camara al andar
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
            // No hay animación
            walking = false;
            // Frena con inercia
            inputVector = Vector3.Lerp(inputVector, Vector3.zero, momentumDamping * Time.deltaTime);
        }
    }
    private void MovePlayer()
    {
        // Mueve al jugador según el input y la velocidad
        movementVector = (inputVector * playerSpeed)+(Vector3.up * gravity);
        characterController.Move(movementVector * Time.deltaTime);
    }
    private void PredictMovement()
    {
        if (lastPositionTime +historicalPositionInterval<= Time.time)
        {
            if (historicalVelocities.Count == maxQueueSize)
            {
                historicalVelocities.Dequeue();
            }
            historicalVelocities.Enqueue(characterController.velocity);
            lastPositionTime = Time.time;
        }
    }
    public Vector3 GetMovement()
    {
        return averageVelocity;
    }

}
