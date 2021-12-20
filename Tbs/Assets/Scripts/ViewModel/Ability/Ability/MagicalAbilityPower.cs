﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicalAbilityPower : BaseAbilityPower
{
    public int level;

    protected override int GetBaseAttack()
    {
        return GetComponentInParent<Stats>()[StatTypes.MAT];
    }

    protected override int GetBaseDefence(Unit target)
    {
        return target.GetComponent<Stats>()[StatTypes.MDF];
    }

    protected override int GetPower()
    {
        return level;
    }
}
