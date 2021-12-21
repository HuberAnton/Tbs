using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class InputController : MonoBehaviour
{
    // ISSUE
    // These are tied to unity input system

    Repearter _hor = new Repearter("Horizontal");
    Repearter _ver = new Repearter("Vertical");


    // All button presses. Eg cancel, accept, rotation, ect.
    string[] _buttons = new string[] { "Fire1", "Fire2", "Fire3"};

    string[] _rotation = new string[] { "RotateLeft", "RotateRight" };

    // I need to think about this a bit more.
    // So this will be used whenever we want to broadcast this event?
    // and we'll pass a point in the field.
    // So anything that needs to detect if input has been pressed
    // does not need to know about this script instance. Only that
    // it listens for this class to broadcast?

    public static event EventHandler<InfoEventArgs<Point>> moveEvent;
    public static event EventHandler<InfoEventArgs<int>> fireEvent;

    public static event EventHandler<InfoEventArgs<int>> rotationEvent;

    private void Update()
    {
        // This is the point here that listens for the input event
        int x = _hor.Update();
        int y = _ver.Update();

        for (int i = 0; i < _buttons.Length; ++i)
        {
            // Cycle through all buttons in string.
            if (Input.GetButtonUp(_buttons[i]))
            {
                if(fireEvent != null)
                {
                    // The Event that the listeners are waiting for.
                    // Since it passes a number I imagine each listener
                    // will be looking for a number.
                    // Don't know how this handles multiple presses on the 
                    // same frame.
                    fireEvent(this, new InfoEventArgs<int>(i));
                }
            }
        }

        for (int i = 0; i < _rotation.Length; ++i)
        {
            if (Input.GetButtonUp(_rotation[i]))
            {
                // Often will have rotaiton.
                if(rotationEvent != null)
                {
                    rotationEvent(this, new InfoEventArgs<int>(i));
                }
            }
        }

        if(x != 0 || y != 0)
        {
            // This is the planned event to be passed out. Stored locally.
            // If your going to broadcast you need something to do so.
            if(moveEvent != null)
            {
                // This is the broadcast.
                // I suppose any script that would be listening for this
                // would probably need some kind of reference to this event or class. 
                moveEvent(this, new InfoEventArgs<Point>(new Point(x, y)));
            }
        }
    }

}

// Used in case a button is held.
// It waits a certain amount of time that it is held
// then repeats the action.

// Reminds me of the input controller I did for soemthing else
// and if I'm right can probably be extended with that info
// broken down into their own seperate classes as well.

class Repearter
{
    const float m_threshold = 0.5f;
    const float m_rate = 0.25f;
    float _next;
    bool _hold;
    string _axis;

    public Repearter(string a_axisName)
    {
        _axis = a_axisName;
    }

    public int Update ()
    {
        int returnValue = 0;
        int value = Mathf.RoundToInt(Input.GetAxisRaw(_axis));


        if(value != 0)
        {
            if (Time.time > _next)
            {
                returnValue = value;
                _next = Time.time + (_hold ? m_rate : m_threshold);
                _hold = true;
            }
        }
        else
        {
            _hold = false;
            _next = 0;
        }

        return returnValue;
    }

}