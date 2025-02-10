using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellShooter : MonoBehaviour
{
    public SpellManager spellManager;
    public Animator gunAnimator;
    public LayerMask collisionMask;
    public Transform castPoint;

    private void Awake()
    {
        spellManager = GetComponent<SpellManager>();
    }

    void Update()
    {
        // Si hay hechizos en la lista y el cooldown lo permite, se dispara
        if (spellManager.spells[spellManager.currentSpell].canShoot && spellManager.spells.Count != 0)
        {
            GetInput();
        }
    }

    private void GetInput()
    {
        // Input del ratón para disparar
        if (Input.GetMouseButtonDown(0))
        {
            StartCoroutine(Shoot(spellManager.spells[spellManager.currentSpell]));
        }
    }
    IEnumerator Shoot(SpellParams currentSpell)
    {
        // Resetea el cooldown
        currentSpell.canShoot = false;

        // Si el Hechizo es tipo Raycast...
        if (currentSpell is RaycastSpell)
        {
            // Lanaza el rayo para ver donde ha impactado
            Vector3 position = Raycast(currentSpell as RaycastSpell);

            // Instancia el efecto de particulas en su posición
            GameObject Effect = Instantiate(currentSpell.particleEffect);
            position.y = 0.1f;
            Effect.transform.position = position;
        }
        // Si el Hechizo es tipo Proyectil...
        else if (currentSpell is ProjectileSpell)
        {
            GameObject projectile = Instantiate((currentSpell as ProjectileSpell).projectile);

            // Pasa al proyectil el daño y el efecto de particulas de su hechizo
            projectile.GetComponent<ProjectileManager>().damage = currentSpell.damage;
            projectile.GetComponent<ProjectileManager>().particleEffect = currentSpell.particleEffect;
            
            // Prepara para lanzar el proyectil. Posición, angulo y dirección
            projectile.transform.position = castPoint.position;
            var rotation = Quaternion.AngleAxis((currentSpell as ProjectileSpell).angle, Vector3.left);
            var direction = rotation * Vector3.forward;
            Vector3 shootdirection = transform.TransformDirection(direction);

            // Lanza el proyectil
            projectile.GetComponent<Rigidbody>().AddForce((currentSpell as ProjectileSpell).force * shootdirection.normalized, ForceMode.Impulse);
        }
        // Calcula el cooldown entre disparos
        yield return new WaitForSeconds(currentSpell.fireRate);
        currentSpell.canShoot = true;
    }
    private Vector3 Raycast(RaycastSpell currentSpell)
    {
        // Lanza el rayo
        Ray ray = new Ray(castPoint.position, transform.forward.normalized);
        RaycastHit hit;

        // Si choca contra algo
        if (Physics.Raycast(ray, out hit, currentSpell.range, collisionMask))
        {
            // Si tiene vida le hace daño
            if (hit.collider.GetComponent<HP>())
            {
                hit.collider.GetComponent<HP>().health -= currentSpell.damage;
            }

            // Devuelve la posición del objeto lista para instanciar el efecto de particulas
            float floorPoint = (castPoint.position - hit.point).magnitude;
            return (castPoint.position + (transform.forward.normalized* floorPoint * 0.95f));
        }
        // Si no choca contra nada
        else
        {
            // Devuelve la posición al final del rango lista para instanciar el efecto de particulas 
            Vector3 endPoint = castPoint.position + transform.forward.normalized * currentSpell.range;
            return endPoint;
        }

    }
}