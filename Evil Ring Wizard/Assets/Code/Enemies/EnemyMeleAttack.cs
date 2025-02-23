using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMeleAttack : MonoBehaviour
{
    public GameObject hitbox;
    public float attackRadius;
    private GameObject player;
    public LayerMask playerLayer;
    // Start is called before the first frame update
    void Start()
    {
        player = GameManager.instance.player;
    }
 
    // Update is called once per frame
    void Update()
    {
        transform.LookAt(player.transform.position);
    }
    public void Attack()
    {
        Collider[] hitPlayer = Physics.OverlapSphere(hitbox.transform.position, attackRadius, playerLayer);
        foreach (Collider player in hitPlayer)
        {
            player.GetComponent<HP>().Damage(10);
        }
    }
    /*
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(hitbox.transform.position, attackRadius);
    }
     */
}
