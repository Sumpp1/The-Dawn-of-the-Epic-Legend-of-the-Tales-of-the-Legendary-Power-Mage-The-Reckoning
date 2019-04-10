﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TeleportSelf", menuName = "SpellSystem/Modifiers/TeleportSelf")]
public class TeleportSelfModifier : SpellScriptableModifier
{

    [SerializeField] private GameObject teleportParticles = null;

    public override void AddSpellModifier(GameObject spellObject)
    {
        TeleportSelf tp = spellObject.AddComponent<TeleportSelf>();
        tp.teleportParticles = teleportParticles;
    }
}
