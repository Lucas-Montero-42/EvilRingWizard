using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy_Kamikaze : MonoBehaviour
{
    private GameObject player;
    public enum States
    {
        Roam,
        Attack
    }
    public States enemyState;
    private NavMeshAgent navMeshAgent;
    public float attackRadius;
    public float roamRadius;
    public float roamDelay;

    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        enemyState = States.Roam;
        Invoke("Roam", roamDelay);
    }
    private void Start()
    {
        player = GameManager.instance.player;
    }
    void Update()
    {
        if ((player.transform.position - transform.position).magnitude < attackRadius)
        {
            enemyState = States.Attack;
        }
        if(enemyState == States.Attack)
        {
            Attack();
        }

    }
    private void Roam()
    {
        //Setea una posición random para ir
        //navMeshAgent.nextPosition = Random.insideUnitCircle * roamRadius;
        Vector2 randomPosition = Random.insideUnitCircle * roamRadius;
        Vector3 offset = new Vector3(randomPosition.x, 0f, randomPosition.y);
        navMeshAgent.destination = transform.position + offset;
        if (enemyState == States.Roam)
            Invoke("Roam", roamDelay);
    }
    private void Attack()
    {
        //Haz un sonido de advertencia y algo visual
        navMeshAgent.destination = player.transform.position;
        navMeshAgent.speed = 10f;
        navMeshAgent.acceleration = 50f;
        //muevete hacia el player
        //Aumenta la velocidad hasta la velocidad máxima
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, roamRadius);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRadius);
        Gizmos.color = Color.green;
    }
}
