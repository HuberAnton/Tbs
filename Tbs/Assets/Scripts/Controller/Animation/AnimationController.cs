using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Handles all changes in animations for
// units in scenes. Meant to decouple and
// and allow for reuse withs several systems.
public class AnimationController
{
    static Dictionary<Info<Unit, string>, string> _animationPlayNotificaiton = new Dictionary<Info<Unit, string>, string>();
    public static string AnimationPlayNotificaiton(Info<Unit, string> info)
    {
        if (!_animationPlayNotificaiton.ContainsKey(info))
            _animationPlayNotificaiton.Add(info, string.Format("AnimationController.{0}PlayAnimation{1}",
                 info.arg0.name,info.arg1));
        return _animationPlayNotificaiton[info];
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
                string name = clips[i].name;
                unit.AddObserver(PlayAnimation, AnimationPlayNotificaiton(new Info<Unit, string>(unit, name)));
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
    
    public static void Play(Unit unit, string animation)
    {
        var info = new Info<Unit, string>(unit, animation);
        if (_animationPlayNotificaiton.ContainsKey(info))
        {
            string test = _animationPlayNotificaiton[info];
            Debug.Log(test);
            unit.PostNotification(test, animation);
        }
        else
        {
            Debug.LogError(unit.ToString() + " does not have an animation called " + animation + ".");
        }
    }

}


// Things I need to sort out.

// Animations listeners should be associated with the unit.
// The chain is outside system -Unit and animation-> extension method -if unit has animation-> post notification.
// Currently there is no association to the unit in the animation controller.
// Also I need to create an animation parsing class.
// In editor mode only, right click on a folder it will create a animation controller and add and rename all.