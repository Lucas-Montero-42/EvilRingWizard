using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy_Imp : MonoBehaviour
{
    private HP hp;
    private GameObject player;
    private Renderer matRenderer;
    public enum States
    {
        Distance,
        Close,
        Dead
    }
    public States enemyState;
    private NavMeshAgent navMeshAgent;
    public float distanceRadius;
    public float fleeRadius;

    private void Awake()
    {
        hp = GetComponent<HP>();
        matRenderer = GetComponent<Renderer>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        enemyState = States.Distance;
    }
    void Start()
    {
        player = GameManager.instance.player;
    }

    // Update is called once per frame
    void Update()
    {
        if (enemyState != States.Dead)
        {
            if ((player.transform.position - transform.position).magnitude < distanceRadius)
                enemyState = States.Close;
            else
                enemyState = States.Distance;
        }
        if (enemyState == States.Close)
            CloseCombat();
        else
            DistanceCombat();

    }

    private void DistanceCombat()
    {
        // Mientras tiene linea de tiro
            // Carga disparo
            // Dispara
            // Se mueve
        // Se muevete a otra posición
    }

    private void CloseCombat()
    {
        // Golpea
        // Se aleja hasta el radio externo
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, distanceRadius);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, fleeRadius);
    }
}
