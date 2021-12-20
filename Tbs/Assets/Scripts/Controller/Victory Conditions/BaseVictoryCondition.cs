using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// How to win or lose a level.
public abstract class BaseVictoryCondition : MonoBehaviour
{
    // If this is changed the map with end.
    public Alliances Victor
    {
        get { return victor; }
        protected set { victor = value; }
    }
    Alliances victor = Alliances.None;

    protected BattleController bc;

    protected virtual void Awake()
    {
        bc = GetComponent<BattleController>();
    }

    // Note these listen to ANY hp change.
    // So it might be better to have a "Stats.DidDeplete" event to check once
    // a unit runs out of that hp.
    protected virtual void OnEnable()
    {
        this.AddObserver(OnHpDidChangeNotification, Stats.DidChangeNotification(StatTypes.HP));
    }

    protected virtual void OnDisable()
    {
        this.RemoveObserver(OnHpDidChangeNotification, Stats.DidChangeNotification(StatTypes.HP));
    }

    // Basic scenario
    // If any unit takes damage check for game over state.
    // Works for team wipe scenario and kill unit scenario.
    protected virtual void OnHpDidChangeNotification(object sender, object args)
    {
        CheckForGameOver(); 
    }

    protected virtual bool IsDefeated(Unit unit)
    {
        Health health = unit.GetComponent<Health>();
        // Checks value has reched the min, not a 0 value.
        if (health)
            return health.MinHp == health.HP;

        // A soft backup. Incase the wrapper does not exist.
        // Always 0.
        Stats stats = unit.GetComponent<Stats>();
        return stats[StatTypes.HP] == 0;
    }

    // Check if a certain alliance is defeated.
    // It still has to cycle through all units.
    // It should really only be cycling through 
    // an alliances units.
    protected virtual bool PartyDefeated(Alliances type)
    {
        for(int i = 0; i < bc.units.Count; ++i)
        {
            Alliance a = bc.units[i].GetComponent<Alliance>();
            // No alliance attached == no win condition.
            if (a == null)
                continue;

            if (a.type == type && !IsDefeated(bc.units[i]))
                return false;
        }
        return true;
    }

    protected virtual void CheckForGameOver()
    {
        if (PartyDefeated(Alliances.Hero))
            Victor = Alliances.Enemy;
    }
}