using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy_Kamikaze : MonoBehaviour
{
    private HP hp;
    private GameObject player;
    private Renderer matRenderer;
    public enum States
    {
        Roam,
        Attack,
        Dead
    }
    public States enemyState;
    private NavMeshAgent navMeshAgent;
    public float attackRadius;
    public float roamRadius;
    public float roamDelay;

    private void Awake()
    {
        hp = GetComponent<HP>();
        matRenderer = GetComponent<Renderer>();
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
            //Haz un sonido de advertencia y/o algo visual
            matRenderer.material.color = Color.red;
            StartCoroutine(DelayAttack());
        }
        if(enemyState == States.Attack)
        {
            Attack();
        }

    }

    IEnumerator DelayAttack()
    {
        navMeshAgent.destination = transform.position;
        yield return new WaitForSeconds(1f);
        enemyState = States.Attack;
    }

    private void Roam()
    {
        if (enemyState != States.Roam)
            return;
        //Setea una posición random para ir
        //navMeshAgent.nextPosition = Random.insideUnitCircle * roamRadius;
        Vector2 randomPosition = Random.insideUnitCircle * roamRadius;
        Vector3 offset = new Vector3(randomPosition.x, 0f, randomPosition.y);
        navMeshAgent.destination = transform.position + offset;
        Invoke("Roam", roamDelay+ Random.Range(.1f,.5f));
    }
    private void Attack()
    {
        navMeshAgent.destination = player.transform.position;
        navMeshAgent.speed = 10f;
        navMeshAgent.acceleration = 50f;
        //muevete hacia el player
        //Aumenta la velocidad hasta la velocidad máxima
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRadius);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, roamRadius);
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject == player)
        {
            Debug.Log("Damage");
            hp.health = 0;
        }
    }
}
