using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleCleaner : MonoBehaviour
{
    public float DeathTime;
    void Start()
    {
        // Temporizador para eliminar part�culas
        StartCoroutine(Clean());
    }
    IEnumerator Clean()
    {
        yield return new WaitForSeconds(DeathTime);
        Destroy(gameObject);
    }
}
