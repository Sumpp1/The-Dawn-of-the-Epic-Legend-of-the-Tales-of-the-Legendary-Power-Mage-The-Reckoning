﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnockBack : OnCollision
{

    public float knockbackForce = 10.0f;

    public override void Hit(GameObject go, Spellbook spellbook) // fix this and pushback
    {
        go.transform.position += (go.transform.position - spellbook.transform.position).normalized * knockbackForce * Time.deltaTime;
    }

}
