using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Not sure if this is a /VIEW not ViewModel since no monobehavior
// Filled out by the ai when doing a turn.
public class PlanOfAttack
{
    public Ability ability;
    public Targets target;
    public Point moveLocation;
    public Point fireLocation;
    public Directions attackDirection;
}
