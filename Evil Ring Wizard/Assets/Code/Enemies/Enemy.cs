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
    [SerializeField]
    [Range(-1f,1f)]
    protected float movementPredictionThreshold = 0f;
    [SerializeField]
    [Range(0.25f,2f)]
    protected float movementPredictionTime = 1f;

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

    protected Vector3 PredictedPosition()
    {
        PlayerMovement playerMovement = player.GetComponent<PlayerMovement>();
        Vector3 futurePos = new Vector3(0, 0, 0);
        futurePos = player.transform.position + (playerMovement.averageVelocity * movementPredictionTime);
        futurePos.y = player.transform.position.y;
        return futurePos;
    }
}
