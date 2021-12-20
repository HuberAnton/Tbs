using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stats : MonoBehaviour
{
    // So this means that all you'll need
    // to change for stats will be the enum.
    // Everything that has a stats will not need
    // to have seperate values for stats.
    public int this[StatTypes s]
    {
        get { return _data[(int)s]; }
        set { SetValue(s, value, true); }
    }
    int[] _data = new int[(int)StatTypes.Count];

    static Dictionary<StatTypes, string> _willChangeNotifications = new
        Dictionary<StatTypes, string>();
    static Dictionary<StatTypes, string> _didChangeNotifications = new
        Dictionary<StatTypes, string>();

    // For use with the notification system
    // Fires when a stat has changed.
    public static string WillChangeNotification(StatTypes type)
    {
        if (!_willChangeNotifications.ContainsKey(type))
            _willChangeNotifications.Add(type, string.Format("Stats.{0}WillChange",
                type.ToString()));
        return _willChangeNotifications[type];
    }

    // For use with the notification system
    // A stat has been changed and needs to be changed
    // back to it's original once completed.
    public static string DidChangeNotification(StatTypes type)
    {
        if (!_didChangeNotifications.ContainsKey(type))
        {
            _didChangeNotifications.Add(type, string.Format("Stats.{0}DidChange",
                type.ToString()));
        }
            return _didChangeNotifications[type];
    }

    // Reusable setter for all stats.
    // I assume stats will be attached to
    // characters, and possibly items?
    public void SetValue (StatTypes type, int value, bool allowExceptions)
    {
        int oldValue = this[type];
        // Skip set statvalue
        if (oldValue == value)
            return;

        // If the stat has an excepetion case.
        // Still unsure how to call this.
        if(allowExceptions)
        {
            
            ValueChangeException exc = new ValueChangeException(oldValue, value);
            
            // Notification Extension
            this.PostNotification(WillChangeNotification(type), exc);

            value = Mathf.FloorToInt(exc.GetModifiedValue());

            if (exc.toggle == false || value == oldValue)
                return;
        }
        // New value of it.
        _data[(int)type] = value;
        // Notification of old value to reset the value to the 
        // original once the exception is no longer valid.
        this.PostNotification(DidChangeNotification(type), oldValue);

    }

}
