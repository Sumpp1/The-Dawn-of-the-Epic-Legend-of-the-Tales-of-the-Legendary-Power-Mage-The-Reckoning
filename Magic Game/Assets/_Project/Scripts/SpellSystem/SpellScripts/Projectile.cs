﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : Spell
{

    #region Variables

    [Header("Projectile variables")]
    [SerializeField] protected float baseDamage         = 50.0f;
    [SerializeField] protected float baseRange          = 1000.0f;
    public float baseSpeed          = 15.0f;

    public GameObject graphics                          = null;
    public GameObject explosionParticle                 = null;

    public Vector3 direction                            { get; set; }
    private Vector3 lastPos                             = Vector3.zero;
    private float distanceTravelled                     = 0.0f;

    public SpellModifier[] modifiers;
    public bool isMaster = false;

    #endregion

    #region Unitys_Methods

    void Start()
    {
        GameObject graphicsCopy = Instantiate(graphics, transform.position, transform.rotation);
        graphicsCopy.transform.SetParent(gameObject.transform);

        lastPos = transform.position;
        direction = transform.forward;

        modifiers = GetComponents<SpellModifier>();
    }

    void FixedUpdate()
    {
        distanceTravelled += Vector3.Distance(transform.position, lastPos);
        lastPos = transform.position;

        if(distanceTravelled < baseRange)
        {
            transform.position += direction * baseSpeed * Time.fixedDeltaTime;
        }
        else
        {
            print("Out of range");
            // explosion particle ??
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // DEAL DAMAGE + APPLY STATUSEFFECTS
        if(collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("Player"))
        {
            var health = collision.gameObject.GetComponent<Health>();
            if (health != null)
            {
                base.DealDamage(health, baseDamage);
            }

            var effectManager = collision.gameObject.GetComponent<StatusEffectManager>();
            if (effectManager != null)
            {
                base.ApplyStatusEffects(effectManager, statusEffects);
            }
        }

        // APPLY ALL COLLISION MODIFIERS
        foreach (SpellModifier modifier in modifiers)
        {
            modifier.ProjectileCollide(collision, direction);
        }

        Instantiate(explosionParticle, collision.contacts[0].point, Quaternion.FromToRotation(transform.up, collision.contacts[0].normal));
        Destroy(gameObject);
    }

    #endregion

    #region Custom_Methods

    public override void CastSpell(Spellbook spellbook, SpellData data)
    {

        ///<summary>
        ///
        ///                                 PROJECTILE SPELLS
        /// 
        ///     • Projectile it self only moves forwards until it's reached maxRange
        ///     • Projectiles can have multiple types of effects
        ///         • Projectiles can home towards closest enemy target
        ///         • Projectiles can have OnCollision modifiers that take effect when projectile collides with something (Split, Bounce, etc.)
        /// 
        /// </summary>


        // get the direction from the spellbook and spawn projectile accodring to that
        direction = spellbook.GetDirection();
        Quaternion rot = Quaternion.LookRotation(direction, Vector3.up);
        Projectile projectile = Instantiate(this, spellbook.spellPos.position, rot);
        projectile.direction = direction;
        projectile.caster = spellbook.gameObject;
        projectile.isMaster = true;

        // apply all modifiers to the projectile ( this is inherited from spell class )
        ApplyModifiers(projectile.gameObject, data);

        // casting is done
        spellbook.StopCasting();

    }

    // THESE ARE USED TO MODIFY PROJECTILES BASE VALUES
    public void ModifyDamage(float amount)
    {
        baseDamage += amount;
    }

    public void ModifyRange(float amount)
    {
        baseRange += amount;
    }

    public void ModifySpeed(float amount)
    {
        baseSpeed += amount;
    }

    #endregion

}
