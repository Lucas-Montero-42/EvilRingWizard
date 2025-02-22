using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectileAttack : MonoBehaviour
{
    public float ShootForce = 25f;
    public GameObject shootPoint;
    public GameObject shootProjectile;

    public void Shoot()
    {
        GameObject projectile = Instantiate(shootProjectile);
        projectile.transform.position = shootPoint.transform.position;
        projectile.GetComponent<Rigidbody>().AddForce(shootPoint.transform.forward * ShootForce, ForceMode.Impulse);
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(shootPoint.transform.position, shootPoint.transform.position+shootPoint.transform.forward);
    }
}
