using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyProjectileAttack : MonoBehaviour
{
    public float ShootForce = 25f;
    public GameObject shootPoint;
    public GameObject shootProjectile;
    private PlayerMovement playerMovement;

    private void Start()
    {
        playerMovement = GameManager.instance.player.GetComponent<PlayerMovement>();
    }
    private void Update()
    {
        Aim();
    }
    public void Shoot()
    {
        
        GameObject projectile = Instantiate(shootProjectile);
        projectile.transform.position = shootPoint.transform.position;
        projectile.GetComponent<Rigidbody>().AddForce(shootPoint.transform.forward * ShootForce, ForceMode.Impulse);
    }
    private void Aim()
    {
        //ARREGLAR
        Vector3 targetPosition = playerMovement.gameObject.transform.position + playerMovement.GetMovement() * 1f;
        transform.LookAt(targetPosition);
        float X;
        float Y;
        float g = -Physics.gravity.y;
        X = targetPosition.z - transform.position.z;
        Y = targetPosition.y - transform.position.y;
        float angle = (float)Mathf.Atan((ShootForce+Mathf.Sqrt((ShootForce*ShootForce*ShootForce*ShootForce)-g*(g*(X*X)+2*Y*(ShootForce*ShootForce))))/(g*X));
        targetPosition.y = (targetPosition - transform.position).magnitude * Mathf.Tan(angle);
        shootPoint.transform.LookAt(targetPosition);

    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(shootPoint.transform.position, shootPoint.transform.position+shootPoint.transform.forward);
    }
}
