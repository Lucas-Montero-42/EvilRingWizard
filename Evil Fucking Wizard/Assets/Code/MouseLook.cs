using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
    // AÑADIR OPCIONES EN EL MENÚ PARA CAMBIARLO--------------------------------------
    public float sensitivity = 1.5f;
    public float smoothing = 10f;
    float xMousePos = 0;
    float smoothedMousePos = 0;
    float currentLookingPos;

    private void Start()
    {
        // Bloquea el ratón
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    void Update()
    {
        GetInput();
        ModifyInput();
        ModifyPlayer();
    }
    private void GetInput()
    {
        // Usa solo el movimiento horizontal del ratón para apuntar
        xMousePos = Input.GetAxisRaw("Mouse X");
    }
    private void ModifyInput()
    {
        // Suaviza el movimiento de la camara
        xMousePos *= sensitivity * smoothing;
        smoothedMousePos = Mathf.Lerp(smoothedMousePos, xMousePos, 1f/smoothing);
    }
    private void ModifyPlayer()
    {
        // Gira al jugador
        currentLookingPos += smoothedMousePos;
        transform.localRotation = Quaternion.AngleAxis(currentLookingPos, transform.up);
    }


}
