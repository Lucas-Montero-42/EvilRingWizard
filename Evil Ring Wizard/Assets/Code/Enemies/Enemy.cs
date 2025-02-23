using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
[RequireComponent(typeof(EnemyMovement))]
public class Enemy : MonoBehaviour
{
    protected HP hp;
    protected GameObject player;
    protected Renderer matRenderer;
    protected NavMeshAgent navMeshAgent;
    [HideInInspector]public EnemyMovement movement;

    virtual public void Awake()
    {
        hp = GetComponent<HP>();
        matRenderer = GetComponent<Renderer>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        movement = GetComponent<EnemyMovement>();
    }
    virtual public void Start()
    {
        player = GameManager.instance.player;
    }
    protected void ChangeColor(Color color)
    {
        matRenderer.material.color = color;
    }
}
