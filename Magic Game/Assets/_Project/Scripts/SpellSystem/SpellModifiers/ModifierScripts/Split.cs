﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Split : SpellModifier
{

    private bool splitted = false;
    public int splitCount = 2;


    public override void ProjectileCollide(Collision collision, Vector3 direction)
    {
        
        if(!splitted)
        {
            for (int i = 0; i < splitCount; i++)
            {
                GameObject copy = Instantiate(gameObject, transform.position, Quaternion.identity);
                copy.transform.rotation = Quaternion.FromToRotation(copy.transform.forward, collision.contacts[0].normal);  // also rotate the whole thing for graphics to face the right direction
                copy.GetComponent<Projectile>().direction = collision.contacts[0].normal;                                   // this changes the direction the projectile is moving
                Destroy(copy.GetComponent<Split>());

                copy.transform.Rotate(copy.transform.right, Random.Range(-45f, 45f));
                copy.transform.Rotate(copy.transform.up, Random.Range(-45f, 45f));

                Projectile copyProj = copy.GetComponent<Projectile>();
                copyProj.caster = gameObject.GetComponent<Projectile>().caster;
            }
        }

    }


    List<GameObject> beamCopies = new List<GameObject>();
    List<Beam> beams = new List<Beam>();

    public override void BeamCollide(RaycastHit hitInfo, Vector3 direction)
    {

        for (int i = 0; i < splitCount; i++)
        {

            if(!splitted)
            {
                GameObject copy = Instantiate(gameObject);
                beamCopies.Add(copy);
                Destroy(copy.GetComponent<Split>());
                splitted = true;
            }

            beamCopies[i].SetActive(true);
            Beam copyBeam = beamCopies[i].GetComponent<Beam>();
            copyBeam.startPos = hitInfo.point;
            copyBeam.isMaster = false;
            Vector3 reflectedDir = Vector3.Reflect(direction, hitInfo.normal);
            reflectedDir = Quaternion.Euler(0, -45f, 0) * reflectedDir;
            copyBeam.direction = reflectedDir;           

        }

    }

    public override void BeamCollisionEnd()
    {
        foreach (GameObject go in beamCopies)
        {
            go.SetActive(false);
        }
    }

}
