using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

// Handles all changes in animations for
// units in scenes. Meant to decouple and
// and allow for reuse withs several systems.

// Note that this might be null as the intention is to create an extension that allows for playing
// animation. That would mean the animationEventNotificationHandler will deal with it all and you
// will call unit.Play(anim,speed) ect and that will cover all it's listeners.

// Issue is the listeners will always need to know about the unit.
public class AnimationController
{

    // May not need to exist.
    // It does keep a list of all animations that are playable in a scene so it could be used to 
    // filter out calls that don't work.
    static Dictionary<string, string> _animationPlayNotificaiton = new Dictionary<string, string>();
    public static string AnimationPlayNotificaiton(string animationName)
    {
        if (!_animationPlayNotificaiton.ContainsKey(animationName))
            _animationPlayNotificaiton.Add(animationName, string.Format("AnimationController.PlayAnimation_{0}",
                 animationName));
        return _animationPlayNotificaiton[animationName];
    }



    private static string Format(Unit unit, string animationName)
    {
        return string.Format(unit.name + "_" + animationName);
    }

    // Adds observers to 
    //private static void AddAnimationObservers(Unit unit, string animationName)
    //{
    //    // Add an observer for starting and ending animaiton.
    //    Animator anim = unit.GetComponentInChildren<Animator>();

    //    var clips = anim.runtimeAnimatorController.animationClips;

    //    for (int i = 0; i < clips.Length; ++i)
    //    {

    //        // Adding observers for commands
    //        unit.AddObserver(PlayAnimation, AnimationPlayNotificaiton(animationName));
    //        //unit.AddObserver(CompletedAnimation, AnimationFinishNotificaiton(animationName));

    //        // Animaiton events will need the associated animation attached onto the gameobject
    //        // houseing the animation controller.
    //        var clip = clips[i];
    //        // Start event
    //        AnimationEvent ev = new AnimationEvent();
    //        ev.functionName = "PostNotificaitonInt";
    //        ev.intParameter = 0;
    //        ev.time = clip.length;
    //        clip.AddEvent(ev);
    //        // End event
    //    }
    //}






    // Get the animator and cycle through all the clips
    //public static void AddAnimationObservers(Unit unit)
    //{
    //    Animator anim = unit.GetComponentInChildren<Animator>();
    //    if (anim)
    //    {
    //        var clips = anim.runtimeAnimatorController.animationClips;
    //        for (int i = 0; i < clips.Length; ++i)
    //        {
    //            string animationName = Format(unit, clips[i].name);

    //            //unit.AddObserver(PlayAnimation, AnimationPlayNotificaiton(animationName));

    //            AddAnimationObservers(unit, animationName);
    //        }
    //    }
    //}

    public static void Stop(Unit unit)
    {
        var anim = unit.GetComponent<Animator>();
        if (anim)
        {
            anim.speed = 0;
        }
    }

    public static void Slow(Unit unit, float from, float to)
    {

    }

    public static void Play(Unit unit, string animationName)
    {
        Play(unit, animationName, 1);
    }

    public static void Play(Unit unit, string animationName, float speed)
    {
        string check = Format(unit, animationName);
        if (_animationPlayNotificaiton.ContainsKey(check))
        {
            Animator anim = unit.GetComponentInChildren<Animator>();
            anim.speed = speed;
            anim.Play(animationName);
        }
        else
        {
            //Debug.LogError(unit.ToString() + " does not have an animation called " + animationName + ".");
        }
    }

    // Test cases for animation events.
    // Event name is always PostNotificaitonInt.
    // The funciton will post a notification to all listeners.
    // So the system will need
    public static void AddAnimationEventTest(Animator animator, string clipName, int eventNumber, float eventTime)
    {
        if (animator)
        {
            var clips = animator.runtimeAnimatorController.animationClips;
            for (int i = 0; i < clips.Length; ++i)
            {
                if (clips[i].name == clipName)
                {

                    AnimationEvent ev = new AnimationEvent();

                    // May not need to exist in proposed system.
                    // Instead compare parameter on event.
                    ev.functionName = "PostNotificaitonInt";
                    //ev.time = clips[i].length;
                    ev.time = eventTime;

                    // This value 
                    ev.intParameter = eventNumber;

                    clips[i].AddEvent(ev);
                    return;
                }
            }
            Debug.LogError(string.Format("No event added as animation {0} does not exist on {1}. Make sure the correct controller is attached and has the animation in its tree.", clipName, animator.name));
        }
    }
    
    // Remove a specific event the a clip.
    public static void RemoveAnimationEventTest(Animator animator, string clipName, int eventNumber)
    {
        if (animator)
        {
            var clips = animator.runtimeAnimatorController.animationClips;

            for (int i = 0; i < clips.Length; ++i)
            {
                if (clips[i].name == clipName)
                {
                    var events = clips[i].events;
                    for(int j = 0; j < events.Length; ++j)
                    {
                        var newEvents = new List<AnimationEvent>();
                        
                        // Unless something has gone very wrong you should always be looking for an event.
                        if (events[j].intParameter != eventNumber)
                        {
                            newEvents.Add(events[j]);
                        }

                        if(newEvents.Count > 0)
                        {
                           clips[i].events = newEvents.ToArray();
                        }
                        else
                        {
                            clips[i].events = new AnimationEvent[0];
                        }
                    }
                    return;
                }
            }
            Debug.LogWarning(string.Format("No event with number {0}", eventNumber));
        }
    }

    // Remove all events on a on a clip
    public static void RemoveAnimationEventsTest(Animator animator, string clipName)
    {
        if (animator)
        {
            var clips = animator.runtimeAnimatorController.animationClips;

            for(int i = 0; i < clips.Length; ++i)
            {
                var cl = clips[i];

                if(cl.name == clipName && cl.events.Length > 0)
                {
                    cl.events = new AnimationEvent[0];
                }
            }

        }

    }

    // Removes event then adds event.
    // Can not edit at runtime by changing values.
    public static void AdjustAnimationEventTest(Animator animator, string clipName, int eventNumber, float eventTime)
    {
        RemoveAnimationEventTest(animator, clipName, eventNumber);
        AddAnimationEventTest(animator, clipName, eventNumber, eventTime);
    }


}
// Things I need to sort out.

// Animations listeners should be associated with the unit.
// The chain is outside system -Unit and animation-> extension method -if unit has animation-> post notification.
// Currently there is no association to the unit in the animation controller.
// Also I need to create an animation parsing class.
// In editor mode only, right click on a folder it will create a animation controller and add and rename all.