using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinValueModifier : ValueModifier
{
    public float min;

    public MinValueModifier(int sortOrder, float a_min) : base(sortOrder)
    {
        min = a_min;
    }


    public override float Modify(float fromValue, float toValue)
    {
        return Mathf.Min(min, toValue);
    }

}
