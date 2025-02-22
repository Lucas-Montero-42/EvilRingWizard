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
    [SerializeField] private float chargeAttackTime;

    EnemyProjectileAttack projectileAttack;
    EnemyMeleAttack meleAttack;


    override public void Awake()
    {
        base.Awake();
        movement = GetComponent<EnemyMovement>();
        projectileAttack = GetComponent<EnemyProjectileAttack>();
        meleAttack = GetComponent<EnemyMeleAttack>();
        enemyState = States.Distance;
        DistanceCombat();
    }
    void Update()
    {
        if (enemyState != States.Dead)
        {
            if ((player.transform.position - transform.position).magnitude > distanceRadius && enemyState != States.Distance)
            {
                enemyState = States.Distance;
                DistanceCombat();
            }
            else if ((player.transform.position - transform.position).magnitude < distanceRadius && enemyState != States.Close)
            {
                enemyState = States.Close;
                CloseCombat();
            }
        }
    }

    private void DistanceCombat()
    {
        StartCoroutine(RangeCombat());
    }
    public IEnumerator RangeCombat()
    {
        if (true) //Linea de tiro
        {

            ChangeColor(Color.red);
            yield return new WaitForSeconds(chargeAttackTime);
            //Dispara
            ChangeColor(Color.white);
            yield return new WaitForSeconds(.25f);
            movement.OrbitPlayer(); // Sustituir por orbit
            yield return new WaitForSeconds(movement.maxtravelTime);
            movement.Iddle();
            StartCoroutine(RangeCombat());
        }
        else
        {
            //busca linea de tiro
        }
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
