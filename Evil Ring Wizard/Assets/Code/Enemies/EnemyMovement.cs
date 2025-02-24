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
        FleePlayer,
        OrbitPlayer //SOLO PARA VALIENTES. No me atrevo a hacer movimiento tactico despues de las 4 horas de arreglar el fleePlayer
    }
    public State currentMovement;
    private NavMeshAgent navMeshAgent;
    private GameObject player;
    private PlayerMovement playerMovement;

    [Header("Roam Params")]
    public float roamRadius = 5f;
    public float roamDelay = 1f;
    [SerializeField] private float roamVariation = .5f;

    [Header("Predict Params")]
    [SerializeField]
    [Range(0.25f, 2f)]
    protected float movementPredictionTime = 1f;
    int right = 0;

    [Header("OrbitParams")]
    public float travelDistance = 10f;
    public float orbitDistance = 10f;
    public float maxtravelTime = 5f;
    Vector2 Option1 = new Vector2(0, 0);
    Vector2 Option2 = new Vector2(0, 0);

    List<Vector3> debug = new List<Vector3>();

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
            case State.OrbitPlayer:
                OrbitPlayer(); // No loopeable
                break;
        }
    }
    public void Iddle()
    {
        currentMovement = State.Iddle;
        navMeshAgent.SetDestination(transform.position);
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
    public void OrbitPlayer()
    {
        currentMovement = State.OrbitPlayer;
        OrbitPlayerMovement();
    }
    private void RoamMovement()
    {
        if (currentMovement != State.Roam || !gameObject.activeSelf)
            return;
        Vector2 randomPosition = UnityEngine.Random.insideUnitCircle * roamRadius;
        Vector3 offset = new Vector3(randomPosition.x, 0f, randomPosition.y);
        navMeshAgent.destination = transform.position + offset;
        Invoke("RoamMovement", roamDelay + UnityEngine.Random.Range(roamDelay-roamVariation, roamDelay+roamVariation));
    }
    private void GoToPlayerMovement()
    {
        if (currentMovement != State.GoToPlayer || !gameObject.activeSelf)
            return;
        navMeshAgent.SetDestination(player.transform.position);
        Invoke("GoToPlayerMovement", 0.15f);
    }
    private void PredictPlayerMovement()
    {
        if (currentMovement != State.PredictPlayer || !gameObject.activeSelf)
            return;
        navMeshAgent.SetDestination(player.transform.position + playerMovement.GetMovement() * movementPredictionTime);
        Invoke("PredictPlayerMovement", 0.15f);
    }
    private void FleePlayerMovement()
    {
        if (currentMovement != State.FleePlayer || !gameObject.activeSelf)
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
                targetPosition.y = 0;
                debug.Add(targetPosition);
                navMeshAgent.CalculatePath(targetPosition, path);
                if (path.status != NavMeshPathStatus.PathInvalid && right != 2)
                {
                    navMeshAgent.SetDestination(transform.position + rotatedFleeDirection * corneredDistance);
                    right = 1;
                    break;
                }
                rotatedFleeDirection = Quaternion.AngleAxis(-escapeAngle * (i), Vector3.up) * fleeDirection;
                targetPosition = transform.position + rotatedFleeDirection * corneredDistance;
                targetPosition.y = 0;
                debug.Add(targetPosition);
                navMeshAgent.CalculatePath(targetPosition, path);
                if (path.status != NavMeshPathStatus.PathInvalid && right != 1)
                {
                    navMeshAgent.SetDestination(transform.position + rotatedFleeDirection * corneredDistance);
                    right = 2;
                    break;
                }
            }
        }
        Invoke("FleePlayerMovement", .5f);
    }
    
    private void OrbitPlayerMovement()
    {
        if (currentMovement != State.OrbitPlayer || !gameObject.activeSelf)
            return;
        
        Vector2 playerPos;
        Vector2 enemyPos;
        playerPos.x = player.transform.position.x; playerPos.y = player.transform.position.z;
        enemyPos.x = transform.position.x; enemyPos.y = transform.position.z;
        int Intersections;
        Intersections = FindCircleCircleIntersections(
            playerPos, orbitDistance,
            enemyPos, travelDistance,
            out Option1, out Option2);

        if (Intersections == 0)
        {
            navMeshAgent.SetDestination(player.transform.position);
        }
        else if (Intersections == 1)
        {
            navMeshAgent.SetDestination(new Vector3(Option1.x,0,Option1.y));
        }
        else
        {
            System.Random random = new System.Random();
            bool randomBool = random.NextDouble() >= 0.5;
            if(randomBool)
                navMeshAgent.SetDestination(new Vector3(Option1.x, 0, Option1.y));
            else
                navMeshAgent.SetDestination(new Vector3(Option2.x, 0, Option2.y));
        }
        
        
    }

    private int FindCircleCircleIntersections(Vector2 c0, float r0, Vector2 c1, float r1, out Vector2 intersection1, out Vector2 intersection2)
    {
        // Find the distance between the centers.
        double dx = c0.x - c1.x;
        double dy = c0.y - c1.y;
        double dist = Math.Sqrt(dx * dx + dy * dy);

        if (Math.Abs(dist - (r0 + r1)) < 0.00001)
        {
            intersection1 = Vector2.Lerp(c0, c1, r0 / (r0 + r1));
            intersection2 = intersection1;
            return 1;
        }

        // See how many solutions there are.
        if (dist > r0 + r1)
        {
            // No solutions, the circles are too far apart.
            intersection1 = new Vector2(float.NaN, float.NaN);
            intersection2 = new Vector2(float.NaN, float.NaN);
            return 0;
        }
        else if (dist < Math.Abs(r0 - r1))
        {
            // No solutions, one circle contains the other.
            intersection1 = new Vector2(float.NaN, float.NaN);
            intersection2 = new Vector2(float.NaN, float.NaN);
            return 0;
        }
        else if ((dist == 0) && (r0 == r1))
        {
            // No solutions, the circles coincide.
            intersection1 = new Vector2(float.NaN, float.NaN);
            intersection2 = new Vector2(float.NaN, float.NaN);
            return 0;
        }
        else
        {
            // Find a and h.
            double a = (r0 * r0 -
                        r1 * r1 + dist * dist) / (2 * dist);
            double h = Math.Sqrt(r0 * r0 - a * a);

            // Find P2.
            double cx2 = c0.x + a * (c1.x - c0.x) / dist;
            double cy2 = c0.y + a * (c1.y - c0.y) / dist;

            // Get the points P3.
            intersection1 = new Vector2(
                (float)(cx2 + h * (c1.y - c0.y) / dist),
                (float)(cy2 - h * (c1.x - c0.x) / dist));
            intersection2 = new Vector2(
                (float)(cx2 - h * (c1.y - c0.y) / dist),
                (float)(cy2 + h * (c1.x - c0.x) / dist));

            return 2;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, roamRadius);
        if (!Application.isPlaying)
            return;
        // Debug solo en playmode
    }
        /*
         */
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        foreach (Vector3 v in debug)
        {
            Gizmos.DrawSphere(v,0.25f);
        }
        if (!Application.isPlaying)
            return;
    }

}
