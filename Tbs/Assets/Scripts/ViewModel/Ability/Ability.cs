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



    public bool CanPerform()
    {
        // By default can perfrom effect.
        BaseException exc = new BaseException(true);
        // Checks to see if anything stopping this
        // ability from being performed.
        this.PostNotification(CanPerformCheck, exc);
        return exc.toggle;
    }


    // All operations for an attack.
    public IEnumerator PerformCR(List<Tile> targets)
    {
        // If can't perform should do some kind of sound and animation
        // then move to next ability.
        if (!CanPerform())
        {
            yield return StartCoroutine(Fail());
            // Nothing passed in. I suppose you would call
            // CanPerform when getting ui.
            // Also Ai would call this when deciding what ability
            // it can use under situations.
            this.PostNotification(FailedNotification);
            yield return null;
        }


        // If can perform should do a bunch of pre effects
        // Eg sound, animations and effects for the ability
        // then apply the damage

        AbilityAnimation  ab =  GetComponentInChildren<AbilityAnimation>();
        if (ab)
        {
            yield return StartCoroutine(ab.ApplyAnimation());
        }
        else
        {
            Debug.LogError("No animation on ability found.");
        }


        for (int i = 0; i < targets.Count; ++i)
        {
            Perform(targets[i]);
        }

        // Would this be an animation triggering event?
        // A ui effect?
        this.PostNotification(DidPerfromNotificaiton);

        yield return null;
    }

    // Old version of perfrom.
    // WIll abpply the damage
    public void Perform(List<Tile> targets)
    {
        if (!CanPerform())
        {
            // Nothing passed in. I suppose you would call
            // CanPerform when getting ui.
            // Also Ai would call this when deciding what ability
            // it can use under situations.
            this.PostNotification(FailedNotification);
            return;
        }

        for (int i = 0; i < targets.Count; ++i)
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
            if(effect)
                effect.Apply(target);
        }
    }

    public bool IsTarget(Tile tile)
    {
        Transform obj = transform;
        for(int i = 0; i < obj.childCount; ++i)
        {
            AbilityEffectTarget targeter = obj.GetChild(i).GetComponent<AbilityEffectTarget>();
            if (targeter)
            {
                if (targeter.IsTarget(tile))
                    return true;
            }
        }
        return false;
    }


    IEnumerator Fail()
    {
        yield return null;
    }


    IEnumerator StartSequence()
    {
        yield return new WaitForSeconds(1);
    }


}
