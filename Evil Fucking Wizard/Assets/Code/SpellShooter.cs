using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellShooter : MonoBehaviour
{
    //public SpellParams spell;
    public SpellManager spellManager;
    public Animator gunAnimator;
    //bool canShoot = true;
    public LayerMask collisionMask;
    public Transform castPoint;

    private void Awake()
    {
        spellManager = GetComponent<SpellManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (spellManager.spells[spellManager.currentSpell].canShoot && spellManager.spells.Count != 0)
        {
            GetInput();
        }
    }

    private void GetInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            StartCoroutine(Shoot(spellManager.spells[spellManager.currentSpell]));
        }
    }
    IEnumerator Shoot(SpellParams currentSpell)
    {
        currentSpell.canShoot = false;
        //gunAnimator.SetTrigger("Shoot");

        if (currentSpell is RaycastSpell)
        {
            Vector3 position = Raycast(currentSpell as RaycastSpell);
            position.y = 0.1f;
            GameObject Effect = Instantiate(currentSpell.particleEffect);
            Effect.transform.position = position;
        }
        else if (currentSpell is ProjectileSpell)
        {
            GameObject projectile = Instantiate((currentSpell as ProjectileSpell).projectile);
            projectile.transform.position = castPoint.position;
            var rotation = Quaternion.AngleAxis((currentSpell as ProjectileSpell).angle, Vector3.left);
            var direction = rotation * Vector3.forward;
            Vector3 shootdirection = transform.TransformDirection(direction);
            projectile.GetComponent<Rigidbody>().AddForce((currentSpell as ProjectileSpell).force * shootdirection.normalized, ForceMode.Impulse);
            projectile.GetComponent<ProjectileManager>().particleEffect = currentSpell.particleEffect;
        }
        yield return new WaitForSeconds(currentSpell.fireRate);
        currentSpell.canShoot = true;
    }
    private Vector3 Raycast(RaycastSpell currentSpell)
    {
        Ray ray = new Ray(castPoint.position, transform.forward.normalized);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, currentSpell.range, collisionMask))
        {
            float a = (castPoint.position - hit.point).magnitude;
            return (castPoint.position + (transform.forward.normalized*a*0.9f));
            //return hit.point;
        }
        else
        {
            Vector3 endPoint = castPoint.position + transform.forward.normalized * currentSpell.range;
            return endPoint;
        }

    }
}