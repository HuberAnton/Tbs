using System.Collections;
using System.Collections.Generic;
using UnityEngine;



// Base class that ability visuals are derived from.
// Incomplete. Stopping to finish solving the rotation
// and movment issues.
public abstract class AbilityVisual : MonoBehaviour
{
    Ability owner;
    protected abstract IEnumerator PerformActions();



    private void Awake()
    {
        owner = GetComponent<Ability>();
    }

    private void OnEnable()
    {
        this.AddObserver(OnDidPerformNotification, Ability.DidPerfromNotificaiton, owner);
    }

    private void OnDisable()
    {
        this.RemoveObserver(OnDidPerformNotification, Ability.DidPerfromNotificaiton, owner);
    }

    protected virtual void OnDidPerformNotification(object sender, object args)
    {
        StartCoroutine("PerformActions");
    }


}
