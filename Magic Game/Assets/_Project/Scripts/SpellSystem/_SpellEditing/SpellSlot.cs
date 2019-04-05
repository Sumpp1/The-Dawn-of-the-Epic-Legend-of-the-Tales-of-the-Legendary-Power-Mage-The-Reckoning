﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpellSlot : MonoBehaviour
{

    public SpellData data;
    private Text spellSlotText;

    // called from SpellEditorController
    public void Init(SpellData data)
    {
        this.data.type = data.type;
        this.data.cards = data.cards;
        this.data.spell = data.spell;
        spellSlotText = GetComponentInChildren<Text>();
        spellSlotText.text = this.data.type.ToString();
    }

}
