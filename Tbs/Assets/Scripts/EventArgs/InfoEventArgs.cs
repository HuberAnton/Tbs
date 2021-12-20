using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Generic class for events.
// Note that it only has 1 field.

public class InfoEventArgs<T> : EventArgs
{
    public T m_info;

    public InfoEventArgs()
    {
        m_info = default(T);
    }
    public InfoEventArgs(T info)
    {
        this.m_info = info;
    }

}
