using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleCleaner : MonoBehaviour
{
    public float DeathTime;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Clean());
    }
    IEnumerator Clean()
    {
        yield return new WaitForSeconds(DeathTime);
        Destroy(gameObject);
    }
}
