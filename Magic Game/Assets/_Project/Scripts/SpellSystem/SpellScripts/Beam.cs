﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Beam : Spell
{

    [Header("-- Beam --")]
    [SerializeField] private float baseDamage       = 1.0f;
    [SerializeField] private float baseRange        = 150.0f;
    [SerializeField] private float baseRadius       = 1f;

    private Vector3 direction                       = Vector3.zero;

    public override void CastSpell(Spellbook spellbook, SpellData data)
    {
        // get the look direction from spellbook and spawn new beam according to that // also child it to player to follow pos and rot
        direction = spellbook.GetDirection();
        Quaternion rot = Quaternion.LookRotation(direction, Vector3.up);
        Beam beam = Instantiate(this, spellbook.spellPos.position, rot);
        beam.caster = spellbook.gameObject;
        beam.transform.SetParent(spellbook.transform);

        // apply all spellmodifiers to the beam
        ApplyModifiers(beam.gameObject, data);

        // keep casting beam as long as the beam button is held down TODO:: change this
        beam.StartCoroutine(CastBeam(beam.gameObject, spellbook, data));
    }

    IEnumerator CastBeam(GameObject self, Spellbook spellbook, SpellData data)
    {

        print("Started beam cast");

        int spellIndex = 0;
        for (int i = 0; i < spellbook.spells.Length; i++)
        {
            if(spellbook.spells[i].spell == data.spell)
            {
                spellIndex = i;
                break;
            }
        }

        while (true)
        {

            // if radius is samller than X limit do beam collision check with ray
            // else if radius is bigger make capsule cast from spellcast position to look direction with range, returns collider[]
            // if collider[].length > 0     compare distances and get the closest one to the caster we hit...

            // keep updating the direction the player is looking and check if our beam hits something
            Vector3 direction = spellbook.GetDirection();
            GameObject hitObject = null;

            if (baseRadius > 0.5f)
            {
                hitObject = CapsuleBeam(spellbook, self);
            }
            else
            {
                hitObject = RaycastBeam(spellbook, self);
            }

            if(hitObject.CompareTag("Player") || hitObject.CompareTag("Enemy"))
            {
                // deal damage
            }

            SpellModifier[] modifiers = GetComponents<SpellModifier>();
            foreach (SpellModifier modifier in modifiers)
            {
                //modifier.BeamCollide(hitObject, direction);
            }

            // if player is not pressing or releases the beam key stop the cast
            if(Input.GetKeyUp((spellIndex + 1).ToString()) || !Input.GetKey((spellIndex + 1).ToString()))
            {
                print("Beam cast ended");
                break;
            }

            yield return null;
        }

        // stop the spellcast and set the cooldown for the spell
        spellbook.StopCasting();
        Destroy(self);

    }

    private GameObject RaycastBeam(Spellbook spellbook, GameObject self)
    {
        Ray ray = new Ray(spellbook.spellPos.position, direction * baseRange);
        RaycastHit hit;

        // if beam hits something do this
        if (Physics.Raycast(ray, out hit, baseRange))
        {
            Debug.DrawRay(spellbook.spellPos.position, (hit.point - spellbook.spellPos.position), Color.red);

            if(hit.collider.GetComponent<Rigidbody>() != null)
            {
                return hit.collider.gameObject;
            }
            else
            {
                return null;
            }

            //// apply beam effects here to target we hit
            //if (hit.collider.gameObject.CompareTag("Enemy") || hit.collider.gameObject.CompareTag("Player"))
            //{
            //    // deal damage to the enemy and apply all collision modifiers ( knockback, burn, etc )
            //    hit.collider.GetComponent<Health>().Hurt(baseDamage);
            //}
            //
            //SpellModifier[] modifiers = self.GetComponents<SpellModifier>();
            //foreach (SpellModifier modifier in modifiers)
            //{
            //    modifier.BeamCollide(hit, direction);
            //}

        }
        else
        {
            // do max range beam if nothing is hit
            Debug.DrawRay(spellbook.spellPos.position, ray.direction * baseRange, Color.green);
            return null;
        }
    }

    private GameObject CapsuleBeam(Spellbook spellbook, GameObject self)
    {

        Collider[] objectsHit = Physics.OverlapCapsule(spellbook.spellPos.position, direction * baseRange, baseRadius);
        GameObject closest = null;
      
        for (int i = 0; i < objectsHit.Length; i++)
        {
            if(closest != null)
            {
                if((objectsHit[i].gameObject.transform.position - spellbook.transform.position).magnitude < (closest.transform.position - spellbook.transform.position).magnitude)
                {
                    closest = objectsHit[i].gameObject;
                }
            }
            else
            {
                closest = objectsHit[i].gameObject;
            }
        }
        return closest;
    }

    public void ModifyDamage(float amount)
    {
        baseDamage += amount;
    }

    public void ModifyRange(float amount)
    {
        baseRange += amount;
    }

    public void ModifyRadius(float amount)
    {
        baseRadius += amount;
    }

}
