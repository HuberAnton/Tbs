using System.Collections;
using System.Collections.Generic;
using UnityEngine;



// Test case for animation system with observers.
public class AnimationEventTest : MonoBehaviour
{

    public GameObject TestUnit;
    private GameObject _testUnit;

    public string clipName;
    public int eventNumber;
    public float eventTime;
    public int currentMode;

    private void Start()
    {
        _testUnit = Instantiate(TestUnit);
        var anim = _testUnit.GetComponentInChildren<Animator>();
        var test = anim.gameObject.AddComponent<AnimationEventNotificaitonHandler>();
        // Will trigger when animation event fires.
        // Can exist without the animation existing without any issues.
        test.AddObserverToAnimation(Test, 0);
        test.AddObserverToAnimation(Test, 1);
        test.AddObserverToAnimation(Test, 2);
    }


    public void Execute()
    {
        if (currentMode == 1)
        {
            AnimationController.AddAnimationEventTest(_testUnit.GetComponent<Animator>(), clipName, eventNumber, eventTime);

        }
        else if (currentMode == 2)
        {
            AnimationController.RemoveAnimationEventTest(_testUnit.GetComponent<Animator>(), clipName, eventNumber);
        }
        else if (currentMode == 3)
        {
            AnimationController.AdjustAnimationEventTest(_testUnit.GetComponent<Animator>(), clipName, eventNumber, eventTime);
        }
    }
    

    private void Update()
    {
        var animator = _testUnit.GetComponentInChildren<Animator>();
        if (Input.GetKeyDown(KeyCode.Q))
        {
            animator.Play("Idle");

        }
        else if(Input.GetKeyDown(KeyCode.E))
        {
            animator.Play("Jump");
        }
    }


    // All these values will be passed in by sequences.
    public void EditClipName(string newClip)
    {
        clipName = newClip;
    }

    public void EditEventName(string newEvent)
    {
        eventNumber = int.Parse(newEvent);
    }

    public void AdjustTime(string newTime)
    {

        eventTime = float.Parse(newTime);
    }

    public void AdjustSetting(float newMode)
    {
        currentMode = (int)newMode;
    }


    // Test case function.
    // Should be attached to the 
    public void Test(object sender, object thing)
    {
        Debug.Log(thing);
    }


}