using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    protected HP hp;
    protected GameObject player;
    protected Renderer matRenderer;
    protected NavMeshAgent navMeshAgent;

    virtual public void Awake()
    {
        hp = GetComponent<HP>();
        matRenderer = GetComponent<Renderer>();
        navMeshAgent = GetComponent<NavMeshAgent>();
    }
    virtual public void Start()
    {
        player = GameManager.instance.player;
    }
}
