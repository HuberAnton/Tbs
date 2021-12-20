using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Handler = System.Action<System.Object, System.Object>;


// These functions call the main Notification script.
// All other scripts should call these to both allow them
// to be passed in and stored as argumets, as well as quickly find
// who is adding listeners and posting notifications.
// May end up having a ton of refferences but since adding 
// and posting is a fairly short call it should be pretty good.
public static class NotificationExtensions
{
    // Posting notifications occurs in the 
    // 
    public static void PostNotification(this object obj, string notificationName)
    {
        NotificationCenter.instance.PostNotification(notificationName, obj);
    }

    public static void PostNotification(this object obj, string notificationName, object e)
    {
        NotificationCenter.instance.PostNotification(notificationName, obj, e);
    }


    // Observers are always out of script.
    // Usually added in OnEnable and removed in OnDisable.
    public static void AddObserver(this object obj, Handler handler, string notificationName)
    {
        NotificationCenter.instance.AddObserver(handler, notificationName);
    }

    public static void AddObserver(this object obj, Handler handler, string notificationName,
        object sender)
    {
        NotificationCenter.instance.AddObserver(handler, notificationName, sender);
    }

    public static void RemoveObserver(this object obj, Handler handler, string notificaitonName)
    {
        NotificationCenter.instance.RemoveObserver(handler, notificaitonName);
    }

    public static void RemoveObserver(this object obj, Handler handler, string notificationName,
        System.Object sender)
    {
        NotificationCenter.instance.RemoveObserver(handler, notificationName, sender);
    }
}
