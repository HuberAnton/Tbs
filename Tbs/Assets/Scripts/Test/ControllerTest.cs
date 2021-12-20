using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerTest : MonoBehaviour
{
    // Add what you want the script to listen for here.
    // Might do well to remove certain events when entering
    // and exiting game states.
    // Potentially might need to have a system that keeps
    // track of what events are being removed and then 
    // re adds them later. Eg cutscene mid battle.

    private void OnEnable()
    {
        InputController.moveEvent += onMoveEvent;
        InputController.fireEvent += onFireEvent;
    }
    private void OnDisable()
    {
        InputController.moveEvent -= onMoveEvent;
        InputController.fireEvent -= onFireEvent;
    }

    // Ahh so I was mostly right with that was occuring.
    // Instead of things listening out for the event to fire.
    // Funcitons are being added to the event instead.
    // So when the event is hit it then calls whatever functions
    // attached to it.
    // May only work on Mono behaviours though.
    private void onMoveEvent(object sender, InfoEventArgs<Point> e)
    {
        Debug.Log("Move " + e.m_info.ToString());
    }

    private void onFireEvent(object sender, InfoEventArgs<int> e)
    {
        Debug.Log("Fire " + e.m_info);
    }





}
