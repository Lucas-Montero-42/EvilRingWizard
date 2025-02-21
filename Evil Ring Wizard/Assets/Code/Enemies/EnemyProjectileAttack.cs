using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectileAttack : MonoBehaviour
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

    [SerializeField] private float maxThrowForce;
    public GameObject attackProjectile;

    [SerializeField] private float sphereRadius;
    public LayerMask sightLayers;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Shoot(GameObject target)
    {
        if (Physics.SphereCast(transform.position, sphereRadius,
            (target.transform.position + Vector3.up - transform.position).normalized,
            out RaycastHit hit,
            float.MaxValue,
            sightLayers)
            && hit.transform == target)
        {
            attackProjectile.transform.SetParent(transform, true);
            attackProjectile.transform.SetParent(transform, true);
            attackProjectile.GetComponent<Rigidbody>().useGravity = false;
            attackProjectile.GetComponent<Rigidbody>().velocity = Vector3.zero;
            StartCoroutine(Attack(target));
        }
    }
    public IEnumerator Attack(GameObject target)
    {
        //StopMoving
        //attackProjectile.gameObject.SetActive(true);
        //attackProjectile.transform.SetParent(null, true);
        yield return null;

        ThrowData throwData = CalculateThrowData(target.transform.position + target.GetComponent<PlayerMovement>().GetMovement(), transform.position);

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
                * (deltaY + Mathf.Sqrt(Mathf.Pow(deltaY, 2)
                + Mathf.Pow(deltaY, 2)))
            ),
            0.01f,
            maxThrowForce
        );

        float angle = Mathf.PI / 2f - (0.5f * (Mathf.PI / 2 - (deltaY / deltaXZ)));

        Vector3 initialVelocity =
            Mathf.Cos(angle) * throwStrength * displacement.normalized
            + Mathf.Sin(angle) * throwStrength * Vector3.up;

        return new ThrowData(initialVelocity, angle, deltaXZ, deltaY);

    }
}
