using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonGenerator : MonoBehaviour
{
    private GameObject player;
    private enum States
    {
        Roam,
        Attack
    }
    private States enemyState;
    public float attackRadius;
    public float roamRadius;

    private void Awake()
    {
        player = GameManager.instance.player;
        enemyState = States.Roam;
    }

    void Update()
    {
        if ((player.transform.position-transform.position).magnitude<attackRadius)
        {
            enemyState = States.Attack;
        }

    }
    private void Roam()
    {
        //Setea una posición random para ir
    }
    private void Attack()
    {
        //muevete hacia el player
        //Aumenta la velocidad hasta la velocidad máxima
    }
}
