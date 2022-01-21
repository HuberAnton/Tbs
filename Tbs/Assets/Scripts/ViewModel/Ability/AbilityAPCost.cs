using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityAPCost : MonoBehaviour
{
    public int amount;
    Ability owner;

    private void Awake()
    {
        owner = GetComponent<Ability>();
    }

    private void OnEnable()
    {
        this.AddObserver(OnCanPerformCheck, Ability.CanPerformCheck, owner);
        this.AddObserver(OnDidPerformNotification, Ability.DidPerfromNotificaiton, owner);
    }

    private void OnDisable()
    {
        this.RemoveObserver(OnCanPerformCheck, Ability.CanPerformCheck, owner);
        this.RemoveObserver(OnDidPerformNotification, Ability.DidPerfromNotificaiton, owner);
    }


    void OnCanPerformCheck(object sender, object args)
    {
        Stats s = GetComponentInParent<Stats>();
        if (s[StatTypes.AP] < amount)
        {
            BaseException exc = (BaseException)args;
            exc.FlipToggle();
        }
    }


    // Reduce ap
    void OnDidPerformNotification(object sender, object args)
    {
        Stats s = GetComponentInParent<Stats>();
        s[StatTypes.AP] -= amount;
    }
}