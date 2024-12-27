using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
    public float sensitivity;
    public float smoothing = 10f;
    float xMousePos = 0;
    float smoothedMousePos = 0;
    float currentLookingPos;

    private void Awake()
    {
        StartCoroutine(GetMouseSensitivity());
    }

    IEnumerator GetMouseSensitivity()
    {
        yield return new WaitForEndOfFrame();
        sensitivity = GameManager.instance.mouseSensitivity;
    }

    private void Start()
    {
        // Bloquea el rat�n
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
        // Usa solo el movimiento horizontal del rat�n para apuntar
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
