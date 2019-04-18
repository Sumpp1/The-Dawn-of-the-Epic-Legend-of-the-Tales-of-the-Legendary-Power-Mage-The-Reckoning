﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "KnockBack", menuName = "SpellSystem/Modifiers/KnockBack")]
public class KnockBackModifier : SpellScriptableModifier
{

    [SerializeField] private float knockbackForce = 0f;

    public override void AddSpellModifier(GameObject spellObject)
    {
        var compo = spellObject.GetComponent<KnockBack>();
        if (compo != null)
        {
            // do what
            return;
        }

        KnockBack component = spellObject.AddComponent<KnockBack>();
        component.knockbackForce = knockbackForce;
    }
}
