using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Stone : Enemy
{
    // Start is called before the first frame update
    public GameObject[] Stones;
    public GameObject AOE;

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

    //EnemyProjectileAttack projectileAttack;
    EnemyMeleAttack meleAttack;


    override public void Awake()
    {
        base.Awake();
        movement = GetComponent<EnemyMovement>();
        //projectileAttack = GetComponent<EnemyProjectileAttack>();
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
        // Crea una linea de rocas que spawnean y van subiendo y bajando
        /*
        movement.Iddle();
        ChangeColor(Color.red);
        yield return new WaitForSeconds(chargeAttackTime);
        projectileAttack.Shoot();
        ChangeColor(Color.white);
        yield return new WaitForSeconds(.25f);
        movement.OrbitPlayer(); // Sustituir por orbit
        yield return new WaitForSeconds(movement.maxtravelTime);
        StartCoroutine(Distance());
         */
        yield return null;
    }

    private void CloseCombat()
    {
        StartCoroutine(Close());
    }
    public IEnumerator Close()
    {
        /*
        ChangeColor(Color.blue);
        meleAttack.Attack();
        yield return new WaitForSeconds(.25f);
        ChangeColor(Color.white);
        movement.FleePlayer();
        */
        yield return null;
    }
    private void OnDrawGizmosSelected()
    {

    }

}