using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultValueModifier : ValueModifier
{
    public readonly float toMultiply;

    public MultValueModifier(int sortOrder, float a_toMultiply) : base (sortOrder)
    {
        toMultiply = a_toMultiply;
    }

    public override float Modify(float fromValue, float toValue)
    {
        return toValue * toMultiply;
    }
}
