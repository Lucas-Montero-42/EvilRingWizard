using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class Enemy_Imp : Enemy
{
    public struct ThrowData
    {
        public ThrowData(Vector3 a, float b, float c, float d)
        {
            initialVelocity = a;
            angle = b;
            deltaXZ = c;
            deltaY = d;
        }
        public Vector3 initialVelocity;
        public float angle;
        public float deltaXZ;
        public float deltaY;
    }
    public enum States
    {
        Distance,
        Close,
        Dead
    }
    [SerializeField] private States enemyState;
    [SerializeField] private float distanceRadius;
    [SerializeField] private float fleeRadius;
    [SerializeField] private float maxThrowForce;
    public GameObject attackProjectile;

    //Shooting
    private float lastAttackTime;
    [SerializeField] private float attackDelay;
    [SerializeField] private float sphereRadius;
    public LayerMask sightLayers;



    override public void Awake()
    {
        base.Awake();
        enemyState = States.Distance;
    }
    void Update()
    {
        FacePlayer();
        if (enemyState != States.Dead)
        {
            if ((player.transform.position - transform.position).magnitude < distanceRadius)
                enemyState = States.Close;
            else
                enemyState = States.Distance;
        }
        if (enemyState == States.Close)
            CloseCombat();
        else
            DistanceCombat();

    }

    private void FacePlayer()
    {
        transform.LookAt(player.transform);
    }

    private void DistanceCombat()
    {
        // Mientras tiene linea de tiro
        // Carga disparo
        // Dispara
        if ((player.transform.position - transform.position).magnitude < fleeRadius)
        {
            Shoot();
        }
            // Se mueve
        // Se muevete a otra posición
    }

    private void Shoot()
    {
        if (Time.time > lastAttackTime + attackDelay 
            && Physics.SphereCast(transform.position, sphereRadius,
            (player.transform.position+Vector3.up - transform.position).normalized,
            out RaycastHit hit,
            float.MaxValue,
            sightLayers)
            && hit.transform == player)
        {
            lastAttackTime = Time.time;
            attackProjectile.transform.SetParent(transform, true);
            attackProjectile.transform.SetParent(transform, true);
            attackProjectile.GetComponent<Rigidbody>().useGravity = false;
            attackProjectile.GetComponent<Rigidbody>().velocity = Vector3.zero;
            StartCoroutine(Attack());
        }
    }
    public IEnumerator Attack()
    {
        //StopMoving
        FacePlayer();
        //attackProjectile.gameObject.SetActive(true);
        //attackProjectile.transform.SetParent(null, true);
        yield return null;

        ThrowData throwData = CalculateThrowData(player.transform.position + player.GetComponent<PlayerMovement>().GetMovement(), transform.position);

        DoThrow(throwData);
        yield return null;
        //MoveEnemy
    }

    private void DoThrow(ThrowData throwData)
    {
        attackProjectile.GetComponent<Rigidbody>().useGravity = true;
        attackProjectile.GetComponent<Rigidbody>().isKinematic = false;
        attackProjectile.GetComponent<Rigidbody>().velocity = throwData.initialVelocity;
    }

    private ThrowData CalculateThrowData(Vector3 TargetPosition, Vector3 StartPosition)
    {
        Vector3 displacement = new Vector3
        (
            TargetPosition.x,
            StartPosition.y,
            TargetPosition.z
        ) - StartPosition;
        float deltaY = TargetPosition.y - StartPosition.y;
        float deltaXZ = displacement.magnitude;

        float gravity = Mathf.Abs(Physics.gravity.y);
        float throwStrength = Mathf.Clamp
        (
            Mathf.Sqrt
            (
                gravity
                * (deltaY + Mathf.Sqrt(Mathf.Pow(deltaY,2)
                + Mathf.Pow(deltaY,2)))
            ),
            0.01f,
            maxThrowForce
        );

        float angle = Mathf.PI / 2f - (0.5f * (Mathf.PI / 2 - (deltaY / deltaXZ)));

        Vector3 initialVelocity =
            Mathf.Cos(angle) * throwStrength * displacement.normalized
            + Mathf.Sin(angle) * throwStrength * Vector3.up;

        return new ThrowData(initialVelocity,angle,deltaXZ,deltaY);

    }

    private void CloseCombat()
    {
        // Golpea
        // Se aleja hasta el radio externo
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, distanceRadius);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, fleeRadius);
    }

}
