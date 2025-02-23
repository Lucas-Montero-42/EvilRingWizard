using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(EnemyMeleAttack))]
public class Enemy_Stone : Enemy
{
    // Start is called before the first frame update
    public GameObject stoneAttack;
    public GameObject AOE;

    public enum States
    {
        Distance,
        Close,
        Dead
    }
    [SerializeField] private States enemyState;
    [SerializeField] private float closeDistanceRadius;
    [SerializeField] private float chargeAttackTime;

    //EnemyProjectileAttack projectileAttack;
    EnemyMeleAttack meleAttack;


    override public void Awake()
    {
        base.Awake();
        movement = GetComponent<EnemyMovement>();
        meleAttack = GetComponent<EnemyMeleAttack>();
        enemyState = States.Distance;
        DistanceCombat();
    }

    void Update()
    {
        if (enemyState != States.Dead)
        {
            if ((player.transform.position - transform.position).magnitude > closeDistanceRadius && enemyState != States.Distance)
            {
                enemyState = States.Distance;
                StopAllCoroutines();
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
    private IEnumerator Distance()
    {
        yield return null;
        movement.Iddle();
        ChangeColor(Color.red);
        yield return new WaitForSeconds(chargeAttackTime);
        transform.LookAt(player.transform.position + player.GetComponent<PlayerMovement>().GetMovement()* 2f);
        GameObject line = Instantiate(stoneAttack);
        line.transform.position = new Vector3(transform.position.x,0,transform.position.z) + (transform.forward * 2f);
        line.transform.rotation = transform.rotation;
        ChangeColor(Color.white);
        yield return new WaitForSeconds(2f);
        movement.Roam();
        yield return new WaitForSeconds(movement.roamDelay*4);
        StartCoroutine(Distance());
    }

    private void CloseCombat()
    {
        StartCoroutine(Close());
    }
    public IEnumerator Close()
    {
        ChangeColor(Color.blue);
        yield return new WaitForSeconds(1f);
        meleAttack.Attack();
        ChangeColor(Color.white);
        yield return new WaitForSeconds(1f);
        StartCoroutine(Close());
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, closeDistanceRadius);
    }

}