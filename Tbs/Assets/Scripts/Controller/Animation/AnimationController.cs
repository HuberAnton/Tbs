using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

// Handles all changes in animations for
// units in scenes. Meant to decouple and
// and allow for reuse withs several systems.
public class AnimationController
{
    static Dictionary<string, string> _animationPlayNotificaiton = new Dictionary<string, string>();
    public static string AnimationPlayNotificaiton(string animationName)
    {
        if (!_animationPlayNotificaiton.ContainsKey(animationName))
            _animationPlayNotificaiton.Add(animationName, string.Format("AnimationController.PlayAnimation_{0}",
                 animationName));
        return _animationPlayNotificaiton[animationName];
    }

    static Dictionary<string, string> _animationFinishNotificaiton = new Dictionary<string, string>();
    public static string AnimationFinishNotificaiton(string animationName)
    {
        if (!_animationFinishNotificaiton.ContainsKey(animationName))
            _animationFinishNotificaiton.Add(animationName, string.Format("AnimationController.FinishAnimation_{0}",
                 animationName));
        return _animationFinishNotificaiton[animationName];
    }


    private static string Format(Unit unit, string animationName)
    {
        return string.Format(unit.name + "_" + animationName);
    }

    // Adds observers to 
    private static void AddAnimationObservers(Unit unit, string animationName)
    {
        // Add an observer for starting and ending animaiton.
        Animator anim = unit.GetComponentInChildren<Animator>();

        var clips = anim.runtimeAnimatorController.animationClips;

        for (int i = 0; i < clips.Length; ++i)
        {

            // Adding observers for commands
            unit.AddObserver(PlayAnimation, AnimationPlayNotificaiton(animationName));
            //unit.AddObserver(CompletedAnimation, AnimationFinishNotificaiton(animationName));

            // Animaiton events will need the associated animation attached onto the gameobject
            // houseing the animation controller.
            var clip = clips[i];
            // Start event
            AnimationEvent ev = new AnimationEvent();
            ev.functionName = "CompletedAnimation";
            ev.intParameter = i;
            ev.time = clip.length;
            clip.AddEvent(ev);



            // End event
        }
    }






    // Get the animator and cycle through all the clips
    public static void AddAnimationObservers(Unit unit)
    {
        Animator anim = unit.GetComponentInChildren<Animator>();
        if (anim)
        {
            var clips = anim.runtimeAnimatorController.animationClips;
            for(int i = 0; i < clips.Length; ++i)
            {
                string animationName = Format(unit, clips[i].name);
                
                //unit.AddObserver(PlayAnimation, AnimationPlayNotificaiton(animationName));

                AddAnimationObservers(unit, animationName);
            }
        }
    }

    static void PlayAnimation(object sender, object args)
    {

        Unit unit = (Unit)sender;
        string animation = (string)args;
        Animator anim = unit.GetComponentInChildren<Animator>();
        anim.Play(animation);
    }
    
    public static void Play(Unit unit, string animationName)
    {
        string check = Format(unit, animationName);
        if (_animationPlayNotificaiton.ContainsKey(check))
        {
            string test = _animationPlayNotificaiton[check];
            unit.PostNotification(test, animationName);
        }
        else
        {
            //Debug.LogError(unit.ToString() + " does not have an animation called " + animationName + ".");
        }
    }


    public static void AddListenerForCompletion(Unit unit, string animationName)
    {
        string check = Format(unit, animationName);

        if (_animationPlayNotificaiton.ContainsKey(check))
        {
            // Should be attached to an animation event.
            // The animation event should then trigger my event listeners.
            // Potentially these should be added upon the tree creation.
            // When parsing add these.

            Animator anim = unit.GetComponentInChildren<Animator>();

            var clips = anim.runtimeAnimatorController.animationClips;



            //even.time = anim.length;
            //even.functionName = "CompletedAnimation";
            //anim.AddEvent(even);




        }

        //Debug.Log("Listeners");
    }


    // Cleanup of events on clips.
    public static void RemoveClipEvents(Unit unit, string animationName)
    {
        string check = Format(unit, animationName);
    }
}


// Things I need to sort out.

// Animations listeners should be associated with the unit.
// The chain is outside system -Unit and animation-> extension method -if unit has animation-> post notification.
// Currently there is no association to the unit in the animation controller.
// Also I need to create an animation parsing class.
// In editor mode only, right click on a folder it will create a animation controller and add and rename all.