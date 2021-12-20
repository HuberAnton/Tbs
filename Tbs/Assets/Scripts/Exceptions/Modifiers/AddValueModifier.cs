using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddValueModifier : ValueModifier
{
    public readonly float toAdd;

    public AddValueModifier(int sortOrder, float a_toAdd) : base (sortOrder)
    {
        toAdd = a_toAdd;
    }

    public override float Modify(float fromValue, float toValue)
    {
        return toValue + toAdd;
    }
}
