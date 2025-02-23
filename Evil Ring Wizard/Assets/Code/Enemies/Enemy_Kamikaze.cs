using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy_Kamikaze : Enemy
{
    public enum States
    {
        Roam,
        Attack,
        Dead
    }
    [SerializeField] private States enemyState;
    [SerializeField] private float attackRadius;

    override public void Awake()
    {
        base.Awake();
        enemyState = States.Roam;
        movement.Roam();
    }
    void Update()
    {
        if ((player.transform.position - transform.position).magnitude < attackRadius)
        {
            //Haz un sonido de advertencia y/o algo visual
            ChangeColor(Color.red);
            StartCoroutine(ChargeAttack());
        }
        if(enemyState == States.Attack)
        {
            Attack();
        }
    }
    IEnumerator ChargeAttack()
    {
        navMeshAgent.destination = transform.position;
        yield return new WaitForSeconds(1f);
        enemyState = States.Attack;
    }
    private void Attack()
    {
        movement.GoToPlayer();
        navMeshAgent.speed = 10f;
        navMeshAgent.acceleration = 50f;
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject == player)
        {
            collision.gameObject.GetComponent<HP>().Damage(10);
            hp.health = 0;
        }
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRadius);
    }
}
