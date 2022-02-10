using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Top layer of an ability.
// Has several child components which make up the actual
// effects, duration, power, range(of target and effect) and hitrate.
// When creating an ability add these children.
public class Ability : MonoBehaviour
{
    public const string CanPerformCheck = "Ability.CanPerformCheck";
    public const string FailedNotification = "Ability.FailedNotificaiton";
    public const string DidPerfromNotificaiton = "Ability.DidPerformNotificaiton";
    //public const string 


    public bool CanPerform()
    {
        // By default can perfrom effect.
        BaseException exc = new BaseException(true);
        // Checks to see if anything stopping this
        // ability from being performed.
        this.PostNotification(CanPerformCheck, exc);
        return exc.toggle;
    }

    // 
    public void Perform(List<Tile> targets)
    {
        if(!CanPerform())
        {
            // Nothing passed in. I suppose you would call
            // CanPerform when getting ui.
            // Also Ai would call this when deciding what ability
            // it can use under situations.
            this.PostNotification(FailedNotification);
            return;
        }

        for(int i = 0; i < targets.Count; ++i)
        {
            Perform(targets[i]);
        }

        // Would this be an animation triggering event?
        // A ui effect?
        this.PostNotification(DidPerfromNotificaiton);
    }


    void Perform(Tile target)
    {
        for(int i = 0; i < transform.childCount; ++i)
        {
            Transform child = transform.GetChild(i);
            BaseAbilityEffect effect = child.GetComponent<BaseAbilityEffect>();
            effect.Apply(target);
        }
    }

    public bool IsTarget(Tile tile)
    {
        Transform obj = transform;
        for(int i = 0; i < obj.childCount; ++i)
        {
            AbilityEffectTarget targeter = obj.GetChild(i).GetComponent<AbilityEffectTarget>();
            if (targeter.IsTarget(tile))
                return true;
        }
        return false;
    }

}
