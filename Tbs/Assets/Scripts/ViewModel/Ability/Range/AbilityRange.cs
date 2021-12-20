using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Base component for all abilities to inherit from.
public abstract class AbilityRange : MonoBehaviour
{

    // For ai
    // If the abilitys needs to be within a specific range.
    public virtual bool PositionOriented { get { return true; } }


    public int horizontal = 1;
    // How many steps up or down the ability can reach.
    // Might need a lower and higher value. Eg can shoot down but
    // not up.
    public int vertical = int.MaxValue; 
    public virtual bool directionOrientation { get { return false; } }
    // Abilitys will be placed onto units.
    protected Unit unit { get { return GetComponentInParent<Unit>(); } }

    public abstract List<Tile> GetTilesInRange(Board board);
}