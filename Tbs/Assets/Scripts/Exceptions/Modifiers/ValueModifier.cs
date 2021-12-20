using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// So what I'm assuming is that there will be a
// notification where you will add these to the actions.
// then they will fire in the order given.
// Does that mean I will need some kind of ui element
// as well when these modifiers are active?

public abstract class ValueModifier : Modifier
{
    public ValueModifier (int sortOrder) : base (sortOrder) { }
    public abstract float Modify(float fromValue, float toValue);
}
