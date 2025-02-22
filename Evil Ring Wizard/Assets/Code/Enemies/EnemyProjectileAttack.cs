using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyProjectileAttack : MonoBehaviour
{
    public GameObject shootPoint;
    public GameObject shootProjectile;
    private PlayerMovement playerMovement;
    public float maxVerticalDisplacement = 0.5f;
    private float g = Physics.gravity.y;

    private void Start()
    {
        playerMovement = GameManager.instance.player.GetComponent<PlayerMovement>();
    }
    private void Update()
    {
    }
    public void Shoot()
    {
        
        GameObject projectile = Instantiate(shootProjectile);
        projectile.transform.position = shootPoint.transform.position;
        projectile.GetComponent<Rigidbody>().velocity = CalculateLaunchVelocity();
    }
    private Vector3 CalculateLaunchVelocity()
    {
        Vector3 targetPosition = playerMovement.gameObject.transform.position + playerMovement.GetMovement() * 1f;
        Debug.Log(playerMovement.GetMovement());
        float h = targetPosition.y + maxVerticalDisplacement;
        float displacementY = targetPosition.y - shootPoint.transform.position.y;
        Vector3 displacementXZ = new Vector3(targetPosition.x - shootPoint.transform.position.x, 0, targetPosition.z - shootPoint.transform.position.z);

        Vector3 velocityY = Vector3.up * Mathf.Sqrt(-2 * g * h);
        Vector3 velocityXZ = displacementXZ / (Mathf.Sqrt(-2 * (h) / g) + Mathf.Sqrt(2 * (displacementY - h) / g));
        return velocityXZ+velocityY;
    }

}
