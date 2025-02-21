using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
[RequireComponent(typeof(EnemyMovement))]
[RequireComponent(typeof(EnemyProjectileAttack))]
[RequireComponent(typeof(EnemyMeleAttack))]
public class Enemy_Imp : Enemy
{
    
    public enum States
    {
        Distance,
        Close,
        Dead
    }
    [SerializeField] private States enemyState;
    [SerializeField] private float distanceRadius;
    [SerializeField] private float fleeRadius;

    EnemyProjectileAttack projectileAttack;
    EnemyMeleAttack meleAttack;


    override public void Awake()
    {
        base.Awake();
        movement = GetComponent<EnemyMovement>();
        projectileAttack = GetComponent<EnemyProjectileAttack>();
        meleAttack = GetComponent<EnemyMeleAttack>();
        enemyState = States.Distance;
    }
    void Update()
    {
        //FacePlayer();
        if (enemyState != States.Dead)
        {
            if ((player.transform.position - transform.position).magnitude < distanceRadius)
                enemyState = States.Close;
            else
                enemyState = States.Distance;
        }
        if (enemyState == States.Distance)
            DistanceCombat();
        else
            CloseCombat();

    }

    private void FacePlayer()
    {
        transform.LookAt(player.transform);
    }

    private void DistanceCombat()
    {
        // Mientras tiene linea de tiro
        // Carga disparo
        // Dispara
        //if ((player.transform.position - transform.position).magnitude < fleeRadius)
        //{
        //    projectileAttack.Shoot(player);
        //}
            // Se mueve
        // Se muevete a otra posición
    }

    private void CloseCombat()
    {
        // Golpea
        //meleAttack.Attack();
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
