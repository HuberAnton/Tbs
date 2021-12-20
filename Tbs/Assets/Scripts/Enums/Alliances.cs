using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Enum for alliances.
// Note that with this you should be able
// to have several alliance values combined.
public enum Alliances 
{
    None = 0,
    Neutral = 1 << 0, //  1
    Hero = 1 << 1,    //  2
    Enemy = 1 << 2    //  4
}
