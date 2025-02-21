using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.GraphicsBuffer;

public class EnemyMovement : MonoBehaviour
{
    public enum State
    {
        Iddle,
        Roam,
        GoToPlayer,
        PredictPlayer,
        FleePlayer
        //TacticalPositioning SOLO PARA VALIENTES. No me atrevo a hacer movimiento tactico despues de las 4 horas de arreglar el fleePlayer
    }
    public State currentMovement;
    private NavMeshAgent navMeshAgent;
    private GameObject player;
    private PlayerMovement playerMovement;

    [Header("Roam Params")]
    [SerializeField] private float roamRadius = 5f;
    [SerializeField] private float roamDelay = 1f;
    [SerializeField] private float roamVariation = .5f;

    [Header("Predict Params")]
    [SerializeField]
    [Range(0.25f, 2f)]
    protected float movementPredictionTime = 1f;
    int right = 0;
    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
    }
    private void Start()
    {
        player = GameManager.instance.player;
        playerMovement = player.GetComponent<PlayerMovement>();
        //DEBUG -----------------------------
        switch (currentMovement)
        {
            case State.Iddle:
                break;
            case State.Roam:
                Roam();
                break;
            case State.GoToPlayer:
                GoToPlayer();
                break;
            case State.PredictPlayer:
                PredictPlayer();
                break;
            case State.FleePlayer:
                FleePlayer();
                break;
            //case State.TacticalPositioning:
            //    TacticalPositioning();
            //    break;
        }
    }
    public void Roam()
    {
        currentMovement = State.Roam;
        RoamMovement();
    }
    public void GoToPlayer()
    {
        currentMovement = State.GoToPlayer;
        GoToPlayerMovement();
    }
    public void PredictPlayer()
    {
        currentMovement = State.PredictPlayer;
        PredictPlayerMovement();
    }
    public void FleePlayer()
    {
        currentMovement = State.FleePlayer;
        FleePlayerMovement();
    }
    /*
    public void TacticalPositioning()
    {
        currentMovement = State.TacticalPositioning;
    }
     */


    private void RoamMovement()
    {
        if (currentMovement != State.Roam)
            return;
        Vector2 randomPosition = UnityEngine.Random.insideUnitCircle * roamRadius;
        Vector3 offset = new Vector3(randomPosition.x, 0f, randomPosition.y);
        navMeshAgent.destination = transform.position + offset;
        Invoke("RoamMovement", roamDelay + UnityEngine.Random.Range(roamDelay-roamVariation, roamDelay+roamVariation));
    }
    private void GoToPlayerMovement()
    {
        if (currentMovement != State.GoToPlayer)
            return;
        navMeshAgent.SetDestination(player.transform.position);
        Invoke("GoToPlayerMovement", 0.15f);
    }
    private void PredictPlayerMovement()
    {
        if (currentMovement != State.PredictPlayer)
            return;
        navMeshAgent.SetDestination(player.transform.position + playerMovement.GetMovement() * movementPredictionTime);
        Invoke("PredictPlayerMovement", 0.15f);
    }
    private void FleePlayerMovement()
    {
        if (currentMovement != State.FleePlayer)
            return;
        float corneredDistance = 5;
        float escapeAngle = 10;
        
        Vector3 fleeDirection = (transform.position - player.transform.position).normalized;
        Vector3 targetPosition = transform.position + fleeDirection * corneredDistance;
        targetPosition.y = 0;
        var path = new NavMeshPath();
        navMeshAgent.CalculatePath(targetPosition, path);

        if (path.status == NavMeshPathStatus.PathComplete)
        {
            navMeshAgent.SetDestination(transform.position + fleeDirection * corneredDistance);
            right = 0;
        }       
        else
        {            
            for (int i = 1; i < 360/(escapeAngle*2); i++)
            {
                Vector3 rotatedFleeDirection = Quaternion.AngleAxis(escapeAngle*(i),Vector3.up) * fleeDirection;
                targetPosition = transform.position + rotatedFleeDirection * corneredDistance;
                navMeshAgent.CalculatePath(targetPosition, path);
                if (path.status == NavMeshPathStatus.PathComplete && right != 2)
                {
                    navMeshAgent.SetDestination(transform.position + rotatedFleeDirection * corneredDistance);
                    right = 1;
                    break;
                }
                rotatedFleeDirection = Quaternion.AngleAxis(-escapeAngle * (i), Vector3.up) * fleeDirection;
                targetPosition = transform.position + rotatedFleeDirection * corneredDistance;
                navMeshAgent.CalculatePath(targetPosition, path);
                if (path.status == NavMeshPathStatus.PathComplete && right != 1)
                {
                    navMeshAgent.SetDestination(transform.position + rotatedFleeDirection * corneredDistance);
                    right = 2;
                    break;
                }
            }
        }
         
        Invoke("FleePlayerMovement", .5f);
    }
    /*
    private void TacticalPositioningMovement()
    {
        if (currentMovement != State.TacticalPositioning)
            return;
    }
     */

    private void OnDrawGizmos()
    {
        if (!Application.isPlaying)
            return;

        //Gizmos.color = Color.red;
    }
}
