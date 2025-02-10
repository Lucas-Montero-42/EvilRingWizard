using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileManager : MonoBehaviour
{
    [HideInInspector] public GameObject particleEffect;
    public bool onFloor;
    private HP enemyHP;
    [HideInInspector] public int damage;

    // Si entra en contacto con algo...
    private void OnTriggerEnter(Collider other)
    {
        // Si tiene vida, le quita
        if (enemyHP = other.GetComponent<HP>())
        {
            enemyHP.health -= damage;
        }
        
        // Busca la posición para colocar el efecto de partículas
        Vector3 floorPosition = transform.position;
        if (onFloor)
            floorPosition.y = 0.1f;

        // Crea el efecto de particulas y se elimina a si mismo
        GameObject newEffect = Instantiate(particleEffect);
        newEffect.transform.position = floorPosition;
        Destroy(gameObject);
    }
}

