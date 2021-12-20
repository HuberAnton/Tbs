using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class HasteStatusEffect : StatusEffect
{
    // A status effect should be added as a 
    // component to a unit when it occurs.
    Stats myStats;

    // Adds a listener to ctr when it's modifeid
    // applying the OnCounterWillChange
    private void OnEnable()
    {
        myStats = GetComponentInParent<Stats>();
        if (myStats)
            this.AddObserver(OnCounterWillChange,
                Stats.WillChangeNotification(StatTypes.CTR), myStats);

    }

    // Removes listener on disable.
    private void OnDisable()
    {
        this.RemoveObserver(OnCounterWillChange,
            Stats.WillChangeNotification(StatTypes.CTR), myStats);
    }

    // All the lifting of the haste effect happens here.
    // This function is passed to the 
    void OnCounterWillChange(object sender, object args)
    {
        // I'm sure casting like this is all good.
        ValueChangeException exc = (ValueChangeException)args;
        MultDeltaModifier m = new MultDeltaModifier(0, 2);
        exc.AddModifier(m);
    }

}
