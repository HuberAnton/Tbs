using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Should handle any unit changes

public abstract class BaseUnitAnimation : MonoBehaviour
{
    // Animation to change
    protected Unit owner;
    // Animation string found in animation system.
    public string animation;

    protected bool complete = false;


    public abstract void AddListeners();


    public abstract void RemoveListeners();

    public virtual void Start()
    {
        //Should not fail.
        // Might cause issues if you can swap abilities?
        owner = GetComponentInParent<Unit>();
    }


    public virtual void OnDestroy()
    {
        RemoveListeners();
    }


    public virtual IEnumerator ApplyAnimation()
    {
        AnimationController.Play(owner, animation);
        // Needs to add a listener to the animation here.
        complete = false;
        //AnimationController.AddListenerForCompletion(owner, animation);
        //while (complete != true)
        //{
        //    yield return null;
        //}

        AnimationController.Play(owner, "Idle");
        yield return null;

    }

    // Will this work?
    public virtual void CompletedAnimation()
    {
        complete = true;
    }

}
