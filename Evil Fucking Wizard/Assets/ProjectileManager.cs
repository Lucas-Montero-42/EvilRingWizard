using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileManager : MonoBehaviour
{
    [HideInInspector] public GameObject particleEffect; //Tiene que venirle del padre HEREDARLO
    public bool OnFloor;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        /*
        if (other.GetComponent<HP>)
        {

        }
        */
        
        Vector3 a = transform.position;
        if (OnFloor)
            a.y = 0.1f;

        GameObject b = Instantiate(particleEffect);
        b.transform.position = a;
        Destroy(gameObject);
    }
}

