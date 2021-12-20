using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// I suppose this would be a common exception.
// If a stat is modified by something other than
// level?
public class ValueChangeException : BaseException
{
    public readonly float fromValue;
    public readonly float toValue;
    public float delta { get { return toValue - fromValue; } }
    List<ValueModifier> modifiers;



    public ValueChangeException(float a_fromValue, float a_toValue) : base(true)
    {
        fromValue = a_fromValue;
        toValue = a_toValue;
    }

    public void AddModifier(ValueModifier m)
    {
        if (modifiers == null)
            modifiers = new List<ValueModifier>();
        modifiers.Add(m);
    }

    public float GetModifiedValue()
    {
        if (modifiers == null)
            return toValue;

        float value = toValue;

        // Sorting a list by passing in a function
        // which cyles through the values?
        
        modifiers.Sort(Compare);
        for(int i = 0; i < modifiers.Count; ++i)
        {
            // Since you pass in the value it gets changed
            // by the modifiers and then restored oustide.
            value = modifiers[i].Modify(fromValue, value);
        }
        return value;
    }


    // I should pay attenditon to this.
    // Would a base variant of this be better?
    // Since they always use the sort order.
    int Compare(ValueModifier x, ValueModifier y)
    {
        return x.sortOrder.CompareTo(y.sortOrder);
    }

}