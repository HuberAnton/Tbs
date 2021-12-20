using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Used to multiply the result of all your
// modifiers rather than the preceding value.
// There is still 
public class MultDeltaModifier : ValueModifier
{
    public readonly float toMultiply;

    public MultDeltaModifier(int sortOrder, float toMultiply) : base (sortOrder)
    {
        this.toMultiply = toMultiply;
    }


    public override float Modify(float fromValue, float toValue)
    {
        // New value of stat - Origianl value - 
        float delta = toValue - fromValue;
        // Mulitply the overall change.
        // I see a * by 0 here.

        // Just return the toValue if delta == 0. Just in case.
        return (delta != 0) ? fromValue + delta * toMultiply : toValue;
    }

}
