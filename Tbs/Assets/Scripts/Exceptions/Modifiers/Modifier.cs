using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Base class for modifiers.
//
public abstract class Modifier 
{
    public readonly int sortOrder;

    public Modifier(int sortOrder)
    {
        this.sortOrder = sortOrder;
    }
}
