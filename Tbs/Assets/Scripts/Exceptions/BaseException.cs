using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Exceptions to rules
// Should be added to the notification center?

public class BaseException
{
    public bool toggle { get; private set; }
    public readonly bool defaultToggle;

    // Bad names.
    public BaseException(bool a_defaultToggle)
    {
        defaultToggle = a_defaultToggle;
        toggle = a_defaultToggle;
    }

    public void FlipToggle()
    {
        toggle = !defaultToggle;
    }
}
