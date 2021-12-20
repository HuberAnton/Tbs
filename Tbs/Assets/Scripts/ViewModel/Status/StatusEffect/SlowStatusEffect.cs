using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Carbon copy of haste
// Only difference being the OnCounterWillChange 0.5f instead of 2
public class SlowStatusEffect : StatusEffect
{
    Stats myStats;

    private void OnEnable()
    {
        myStats = GetComponent<Stats>();
        if (myStats)
            this.AddObserver(OnCounterWillChange, 
                Stats.WillChangeNotification(StatTypes.CTR), myStats);
    }

    private void OnDisable()
    {
        this.RemoveObserver(OnCounterWillChange,
            Stats.WillChangeNotification(StatTypes.CTR), myStats);
    }

    // Make an excpetion and
    void OnCounterWillChange(object sender, object args)
    {
        ValueChangeException exc = (ValueChangeException)args;
        MultDeltaModifier m = new MultDeltaModifier(0, 0.5f);
        exc.AddModifier(m);
    }

}
