using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleCleaner : MonoBehaviour
{
    //public bool testing = false;
    public float DeathTime;
    void Start()
    {
        // Temporizador para eliminar partículas
        StartCoroutine(Clean());
    }
    IEnumerator Clean()
    {
        yield return new WaitForSeconds(DeathTime);
        //if(testing)Instantiate(gameObject, transform.position, transform.rotation);
        Destroy(gameObject);
    }
}
