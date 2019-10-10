﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityStandardAssets.Characters.ThirdPerson;

public class Controller : MonoBehaviour
{
    //public Camera cam;

    public NavMeshAgent agent;

    public ThirdPersonCharacter character;

    public Rigidbody rb;

    bool isGrounded;

    private void Start()
    {
        agent.updateRotation = false;
        rb.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {   
        /*
        if(Input.GetMouseButtonDown(0))
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

           if ( Physics.Raycast(ray, out hit))
            {
                // MOVE OUT AGENT
                agent.SetDestination(hit.point);
            }
        }
        */

        if (agent.remainingDistance > agent.stoppingDistance)
        {
            character.Move(agent.desiredVelocity, false, false);
        }
        else
        {
            character.Move(Vector3.zero, false, false);
        }

    }

   
}
