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
    [SerializeField] private float closeDistanceRadius;
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
            if ((player.transform.position - transform.position).magnitude > fleeRadius && enemyState != States.Distance)
            {
                enemyState = States.Distance;
                DistanceCombat();
            }
            else if ((player.transform.position - transform.position).magnitude < closeDistanceRadius && enemyState != States.Close)
            {
                enemyState = States.Close;
                StopAllCoroutines();
                CloseCombat();
            }
        }
    }

    private void DistanceCombat()
    {
        StartCoroutine(Distance());
    }
    public IEnumerator Distance()
    {
        movement.Iddle();
        ChangeColor(Color.red);
        yield return new WaitForSeconds(chargeAttackTime);
        projectileAttack.Shoot();
        ChangeColor(Color.white);
        yield return new WaitForSeconds(.25f);
        movement.OrbitPlayer(); // Sustituir por orbit
        yield return new WaitForSeconds(movement.maxtravelTime);
        StartCoroutine(Distance());
    }

    private void CloseCombat()
    {
        StartCoroutine(Close());
    }
    public IEnumerator Close()
    {
        ChangeColor(Color.red);
        meleAttack.Attack();
        yield return new WaitForSeconds(.25f);
        ChangeColor(Color.white);
        movement.FleePlayer();
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, closeDistanceRadius);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, fleeRadius);
    }

}
