using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// If the ai is controlled by the player
// or another force.
// Need to update for cutscenes
public class Driver : MonoBehaviour
{
    public Drivers normal;
    public Drivers special;

    public Drivers Current
    {
        get
        {
            return special != Drivers.None ? special : normal;
        }
    }
}
