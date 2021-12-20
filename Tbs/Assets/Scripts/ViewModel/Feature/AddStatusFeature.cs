using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Reading this
// Class with a template inheriting from feature 
// Making T a status effect type.
// Since this is abstract you need to create a
// new class inheriting from this replacing T with the status effect.
public abstract class AddStatusFeature<T> : Feature where T : StatusEffect
{
    StatusCondition statusCondition;

    protected override void OnApply()
    {
        Status status = GetComponentInParent<Status>();
        statusCondition = status.Add<T, StatusCondition>();
    }

    protected override void OnRemove()
    {
        if (statusCondition != null)
            statusCondition.Remove();
    }

}

// To use:
// Create a new script inheriting from this.
// Leave empty but change T to the statuseffect.