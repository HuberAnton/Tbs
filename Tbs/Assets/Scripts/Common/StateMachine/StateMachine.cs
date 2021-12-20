using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine : MonoBehaviour
{
    // Active state
    // I really should do more property stuff
    // like this. 
    public virtual State CurrentState
    {
        get { return _currentState; }
        set { Transition(value); }
    }
    protected State _currentState;


    // To stop from state chanes overlapping.
    protected bool _inTransition;

    public virtual T GetState<T> () where T : State
    {
        T target = GetComponent<T>();
        if(target == null)
        {
            target = gameObject.AddComponent<T>();
        }
        return target;
    }

    public virtual void ChangeState<T>() where T : State
    {
        CurrentState = GetState<T>();
    }

    // All switching state shenanigans.
    // Note that this locks you into completing the states "Enter".
    protected virtual void Transition(State value)
    {
        Debug.Log(string.Format("Trying to enter {0}", value.ToString()));
        // Enter same state or _already in transition
        // quick return. Should be helpful
        if (_currentState == value || _inTransition)
            return;


        _inTransition = true;
        Debug.Log(string.Format("Entering {0}", value.ToString()));
        // Do exit things.
        // This reminds me that I should
        // have a state that does stuff while it 
        // is exiting.
        if (_currentState != null)
            _currentState.Exit();

        _currentState = value;

        // Do enter things
        if (_currentState != null)
            _currentState.Enter();

        _inTransition = false;
        Debug.Log(string.Format("Now in {0}", value.ToString()));
    }
}
