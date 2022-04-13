using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


// For system to work you need to add an animation event and then
// add an observer with the associated int value.
// On initializatoin of a unit this component should be added to the same position
// as the animator.

// By default 0 is start animation and 1 is end
public class AnimationEventNotificaitonHandler : MonoBehaviour
{

    static Dictionary<int, string> _animationEventNotification = new Dictionary<int, string>();
    string AnimationEventNotification(int eventNumber)
    {
        if (!_animationEventNotification.ContainsKey(eventNumber))
            _animationEventNotification.Add(eventNumber, string.Format("PostEvent{0}", eventNumber.ToString()));
        return _animationEventNotification[eventNumber];
    }

    // May want to consider passing through the sender.
    public void PostNotificaitonInt(int i)
    {
        this.PostNotification(AnimationEventNotification(i), i);
    }

    // These should be in the animation controller

    public void AddObserverToAnimation(Action<object, object> function, int eventNotificaiton)
    {
        this.AddObserver(function, AnimationEventNotification(eventNotificaiton));
    }

    public void RemoveObserverFromAnimation(Action<object, object> function, int eventNotificaiton)
    {
        this.RemoveObserver(function, AnimationEventNotification(eventNotificaiton));
    }
}
