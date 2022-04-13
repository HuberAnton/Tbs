using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

// Should handle any unit changes

public abstract class BaseUnitAnimation : MonoBehaviour
{
    // Animation to change
    protected Animator animationController;
    // Event handler to attach observers to.
    private AnimationEventNotificaitonHandler _eventHandler;
    // Animation string found in animation system.
    public string animationClipName;

    // What event number to trigger
    public int eventNumber = 0;
    // What time the event will fire
    public int eventTime;

    // I feel there are better ways to do this then store something like this.
    protected bool complete = false;


    public virtual void AddListeners()
    {
        // Neeed a listener to be added to the skip function here. To complete all coroutines.
        //AnimationController.AddAnimationEventTest(animationController, animationClipName, eventNumber, eventTime);

    }


    public virtual void RemoveListeners()
    {
        // Neeed a listener to be removed from the skip function here.
        //AnimationController.RemoveAnimationEventTest(animationController, animationClipName, eventNumber);
    }

    public virtual void Start()
    {
        //Should not fail.
        // Might cause issues if you can swap abilities?
        animationController = this.transform.root.GetComponentInChildren<Animator>();
        // Might have init problems.
        if(animationController)
        _eventHandler = animationController.gameObject.GetComponent<AnimationEventNotificaitonHandler>();
    }

    // Safety
    public virtual void OnDestroy()
    {
        RemoveListeners();
    }


    public virtual IEnumerator PlayAnimation()
    {
        if (animationController)
        {
            // Play the animaiton clip stored in the component
            // Will fail silently.
            animationController.Play(animationClipName);
            // Wait till then animations change in the next frame.
            yield return null;
            Setup();

            //    // Needs to wait until animation has completed in default case.
            while (complete != true)
            {
                yield return null;
            }

            // Wait for the fired script to complete running.
            yield return null;
            animationController.Play("Idle");
            yield return null;
        }
    }


    protected virtual void Setup()
    {
        complete = false;

        // Check that the event being added isn't passed the length of the clip.
        // If it is use the max clip length instead.
        var length = animationController.GetCurrentAnimatorClipInfo(0)[0].clip.length;
        float eventAddTime;
        if(eventTime < length)
        {
            eventAddTime = eventTime;
        }
        else
        {
            eventAddTime = length;
        }
        AnimationController.AddAnimationEventTest(animationController, animationClipName, eventNumber, eventAddTime);
        _eventHandler.AddObserverToAnimation(Complete, eventNumber);
    }

    protected virtual void Complete(object sender, object thing)
    {
        complete = true;
        AnimationController.RemoveAnimationEventTest(animationController, animationClipName, eventNumber);
    }

}
