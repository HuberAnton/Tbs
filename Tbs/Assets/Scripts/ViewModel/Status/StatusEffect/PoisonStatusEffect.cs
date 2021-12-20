using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonStatusEffect : StatusEffect
{
    Unit owner;

    private void OnEnable()
    {
        owner = GetComponentInParent<Unit>();
        if (owner)
            this.AddObserver(OnNewTurn, TurnOrderController.TurnBeganNotification, owner);

    }

    private void OnDisable()
    {
        this.RemoveObserver(OnNewTurn, TurnOrderController.TurnBeganNotification, owner);
    }

    void OnNewTurn(object sender, object args)
    {
        Stats s = GetComponentInParent<Stats>();
        int currentHP = s[StatTypes.HP];
        int maxHP = s[StatTypes.MHP];
        // Does 10% per turn. That's a lot of damage.
        int reduce = Mathf.Min(currentHP, Mathf.FloorToInt(maxHP * 0.1f));
        // Sets the stat value with no exceptions applied.
        // What you see is what you get.
        s.SetValue(StatTypes.HP, (currentHP - reduce), false);
    }
}
