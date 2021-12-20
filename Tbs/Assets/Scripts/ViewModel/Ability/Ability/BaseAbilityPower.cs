using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseAbilityPower : MonoBehaviour
{
    protected abstract int GetBaseAttack();
    protected abstract int GetBaseDefence(Unit target);
    protected abstract int GetPower();

    private void OnEnable()
    {
        this.AddObserver(OnGetBaseAttack, DamageAbilityEffect.GetAttackNotification);
        this.AddObserver(OnGetBaseDefence, DamageAbilityEffect.GetDefenceNotificaiton);
        this.AddObserver(OnGetPower, DamageAbilityEffect.GetPowerNotification);
    }



    void OnGetBaseAttack(object sender, object args)
    {
        if (IsMyEffect(sender))
        {
            // Attacker, target, modifiers.
            var info = (Info<Unit, Unit, List<ValueModifier>>)args;
            // I wonder about this. Something that isn't a unit
            // attacking... I suppose it wouldn't attack
            // but apply damage.
            if (info.arg0 != GetComponentInParent<Unit>())
                return;

            // Adds it to the list of modifiers.
            info.arg2.Add(new AddValueModifier(0, GetBaseAttack()));
        }
    }

    void OnGetBaseDefence(object sender, object args)
    {
        if (IsMyEffect(sender))
        {
            var info = (Info<Unit, Unit, List<ValueModifier>>)args;

            if (info.arg0 != GetComponentInParent<Unit>())
                return;

            info.arg2.Add(new AddValueModifier(0, GetBaseDefence(info.arg1)));
        }
    }

    void OnGetPower(object sender, object args)
    {
        if (IsMyEffect(sender))
        {
            var info = (Info<Unit, Unit, List<ValueModifier>>)args;
            if (info.arg0 != GetComponentInParent<Unit>())
                return;

            info.arg2.Add(new AddValueModifier(0, GetPower()));
        }
    }

    // Used to check if the modifier applies to this ability.
    // Honestly not good.
    // If you have more abilities then you need to ignore more
    // and more of these.
    // Other option is to have each ability have it's own 
    bool IsMyEffect(object sender)
    {
        MonoBehaviour obj = (MonoBehaviour)sender;
        return (obj != null && obj.transform.parent == transform);
    }

}