using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Ko.
// Unit runs out of hp.
public class KnockOutStatusEffect : StatusEffect
{
    Unit owner;
    Stats stats;

    private void Awake()
    {
        owner = GetComponentInParent<Unit>();
        stats = owner.GetComponent<Stats>();
    }

    private void OnEnable()
    {
        // Hmm.... so this will make the unit smaller.
        // Would rather replace it with an animation or model.
        owner.transform.localScale = new Vector3(0.75f, 0.1f, 0.75f);
        this.AddObserver(OnTurnCheck, TurnOrderController.TurnCheckNotification, owner);
        // Ahh this stops it..
        this.AddObserver(OnStatCounterWillChange, Stats.WillChangeNotification(StatTypes.CTR), stats);
    }

    private void OnDisable()
    {
        owner.transform.localScale = Vector3.one;
        this.RemoveObserver(OnTurnCheck, TurnOrderController.TurnCheckNotification, owner);
        this.RemoveObserver(OnStatCounterWillChange, Stats.WillChangeNotification(StatTypes.CTR), stats);
    }

    // Skip unit when turn comes up.
    // Unsure if this still allows ctr value to increase. 
    void OnTurnCheck(object sender, object args)
    {
        BaseException exc = (BaseException)args;
        if (exc.defaultToggle == true)
            exc.FlipToggle();
    }

    // Stops ctr from increasing if under this status.
    void OnStatCounterWillChange(object sender, object args)
    {
        ValueChangeException exc = (ValueChangeException)args;
        if (exc.toValue > exc.fromValue)
            exc.FlipToggle();
    }
}