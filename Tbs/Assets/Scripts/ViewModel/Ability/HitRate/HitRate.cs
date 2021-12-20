using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class HitRate : MonoBehaviour
{
    //  Notification strings for this abstract class.

    // Both of these have 
    public const string AutomaticHitCheckNotification =
        "HitRate.AutomaticHitCheckNotification";

    public const string AutomaticMissCheckNotification =
        "HitRate.AutomaticMissCheckNotification";


    // Observers will need to pass an Info class including
    // Attaker unit, defend unit, and calculated hitchance.
    public const string StatusCheckNotification =
        "HitRate.StatusCheckNotification";

    // To return hit chance.
    public abstract int Calculate(Tile target);


    // For ai
    // If the abilitys hit rate is effected by angle.
    public virtual bool IsAngleBased { get { return true; } }


    // Called by abilitys.
    public virtual bool RollForHit(Tile target)
    {
        int roll = UnityEngine.Random.Range(0, 101);
        int chance = Calculate(target);
        return roll <= chance;
    }

    // In case there are any cases where auto hits would fail.
    protected virtual bool AutomaticHit(Unit attacker, Unit target)
    {
        MatchException exc = new MatchException(attacker, target);
        this.PostNotification(AutomaticHitCheckNotification, exc);

        // Modified by listeners.
        return exc.toggle;
    }

    // Same as above but looking for something that would null an
    // auto miss. Note that this doesn't mean it will auto hit.
    protected virtual bool AutomaticMiss(Unit attacker, Unit target)
    {
        MatchException exc = new MatchException(attacker, target);
        this.PostNotification(AutomaticMissCheckNotification);

        return exc.toggle;
    }

    protected virtual int AdjustForStatusEffects(Unit attacker, Unit target, int rate)
    {
        Info<Unit, Unit, int> args = new Info<Unit, Unit, int>(attacker, target, rate);
        // Pass in attacker defender and current hit rate.
        this.PostNotification(StatusCheckNotification, args);
        return args.arg2;
    }

    protected virtual int Final(int evade)
    {
        // 100 is assumed hit chance is
        // 100% 
        return 100 - evade;
    }

}
