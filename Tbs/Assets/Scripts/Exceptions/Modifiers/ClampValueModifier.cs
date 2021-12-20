using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClampValueModifier : ValueModifier
{
    public readonly float min;
    public readonly float max;

    public ClampValueModifier(int sortOrder, float a_min, float a_max) : base (sortOrder)
    {
        min = a_min;
        max = a_max;
    }

    public override float Modify(float fromValue, float toValue)
    {
        return Mathf.Clamp(toValue, min, max);
    }
}
