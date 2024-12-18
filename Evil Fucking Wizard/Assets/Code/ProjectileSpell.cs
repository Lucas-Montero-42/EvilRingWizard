using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ProjectileSpell", menuName = "Create Projectile Spell", order = 0)]
public class ProjectileSpell : SpellParams
{
    public float force;
    public float angle;
    public GameObject projectile;

}
