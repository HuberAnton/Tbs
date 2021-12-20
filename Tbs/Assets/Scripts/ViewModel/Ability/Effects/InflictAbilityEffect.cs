using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using System;

public class InflictAbilityEffect : BaseAbilityEffect
{
    public string statusName;
    public int duration;

    // No prediction of stat effects.
    // Might need to adjust this based on somehting.
    public override int Predict(Tile target)
    {
        return 0;
    }

    // Uses reflection.
    // Should be the case in status effects.
    // Note that we are always passing in duration status condition
    // and need to get the type status condition from this object if 
    // we want another.
    // Really cool though.
    protected override int OnApply(Tile target)
    {
        // Will this look through the name space for this type?
        Type statusType = Type.GetType(statusName);

        if (statusType == null || !statusType.IsSubclassOf(typeof(StatusEffect)))
        {
            Debug.LogError(string.Format("Invalid status stype. Status {0} not applied", statusName));
            return 0;
        }
        // Gets the function add from the status class.
        MethodInfo mi = typeof(Status).GetMethod("Add");
        // Initialized list of types.
        Type[] types = new Type[] { statusType, typeof(DurationStatusCondition) };
        
        // Since Add uses generics you need to define them before calling.
        // So the status effect and always being a duration effect.
        MethodInfo constructed = mi.MakeGenericMethod(types);
        // Get the targets status.
        Status status = target.m_content.GetComponent<Status>();
        // Call the generic funciton on status?
        // I'm going to assume right now this will call add
        // on status. 
        // Because of the way this is called all we need to 
        // do is define the 2 types and not pass any arguments.
        object retValue = constructed.Invoke(status, null);

        DurationStatusCondition condition = (DurationStatusCondition)retValue;
        condition.duration = duration;
        return 0;
    }

}
