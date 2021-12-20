using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Flags]
public enum EquipSlots
{
    // Equip slots should be defined by the unit
    // itself somewhat. 
    // Eg: Human = 1 of each.
    // Monster with 'no' arms = no primary or secondary but 2 extra accessories.
    None = 0,
    Primary = 1 << 0,
    Secondary = 1 << 1,
    Head = 1 << 2,
    Body = 1 << 3,
    Accessory = 1 << 4,
}
