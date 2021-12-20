using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Basic application with items in mind
// Adds the stat and then removes it from the 
// Is it worth simplifying this
// by adding a list of stats to be modified?
public class StatModifierFeature : Feature
{
    // Consider an array of types?
    // Cycle through on the apply and 
    // and OnRemove?
    // Consider lists in the for real version.
    //public StatTypes[] typeArray;
    //public int[] amountArray;
    public StatTypes type;
    public int amount;

    Stats stats
    {
        get
        {
            return _target.GetComponentInParent<Stats>();
        }
    }

    protected override void OnApply()
    {
        // Lmao does this work?
        //for(int i = typeArray.Length - 1; i >= 0; --i)
        //{
        //    if(amountArray[i] != 0)
        //    {
        //        stats[typeArray[i]] += amountArray[i];
        //    }
        //}

        stats[type] += amount;
    }

    protected override void OnRemove()
    {
        //for(int i = typeArray.Length - 1; i >= 0; --i)
        //{
        //    if(amountArray[i] != 0)
        //    {
        //        stats[typeArray[i]] -= amountArray[i];
        //    }
        //}

        stats[type] -= amount;
    }
}
