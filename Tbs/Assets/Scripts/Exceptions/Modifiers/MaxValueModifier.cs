using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaxValueModifier : ValueModifier
{
    public float max;

    public MaxValueModifier (int sortOrder, float a_max) : base (sortOrder)
    {
        max = a_max;
    }

    public override float Modify(float fromValue, float toValue)
    {
        return Mathf.Max(toValue, max);
    }
}
