using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Can also be thought of as classes.
public class Job : MonoBehaviour
{
    // So this creates an array or an enum? Nifty.
    // If I were to change or modify the stats enum
    // this script would fail so I woudn't end up with a 
    // silent error. Also these are stats attached to jobs.
    // I might want to do something similar with races if that
    // is ever a thing.
    public static readonly StatTypes[] statOrder = new StatTypes[]
    {
        StatTypes.MHP,
        StatTypes.MMP,
        StatTypes.ATK,
        StatTypes.DEF,
        StatTypes.MAT,
        StatTypes.MDF,
        StatTypes.SPD
    };

    public int[] baseStats = new int[statOrder.Length];
    public float[] growStats = new float[statOrder.Length];
    Stats stats;

    private void OnDestroy()
    {
        this.RemoveObserver(OnLvlChangeNotification,
            Stats.DidChangeNotification(StatTypes.LVL));
    }

    // The 'Activate' of the class.
    public void Employ()
    {
        stats = gameObject.GetComponentInParent<Stats>();
        this.AddObserver(OnLvlChangeNotification,
            Stats.DidChangeNotification(StatTypes.LVL), stats);

        // Should be stored on the class itself.
        // Potentially abilitys and viusal effects?
        Feature[] features = GetComponentsInChildren<Feature>();

        for(int i = 0; i < features.Length; ++i)
        {
            // Cycle through in order and apply all class
            // features.
            features[i].Activate(gameObject);
        }
    }

    // The 'Deactivate' of the class.
    public void UnEmploy()
    {
        Feature[] features = GetComponentsInChildren<Feature>();
        for(int i = 0; i < features.Length; ++i)
        {
            features[i].Deactivate();
        }

        this.RemoveObserver(OnLvlChangeNotification,
            Stats.DidChangeNotification(StatTypes.LVL), stats);

        stats = null;
    }

    // When unit is created.
    // May not be useful if race/potential is base stats.
    public void LoadDefaultStats()
    {
        // Applies all values attached to the job class.
        for(int i = 0; i < statOrder.Length;++i)
        {
            StatTypes type = statOrder[i];
            stats.SetValue(type, baseStats[i], false);
        }
        // Because these are always needed?
        // Oh when job class is changed it always sets the hp
        // and mp to max. Exploitable. Might be worth
        // leaving as is but reducing it if the max is lower than current.
        // No level calculation in here though.
        stats.SetValue(StatTypes.HP, stats[StatTypes.MHP], false);
        stats.SetValue(StatTypes.MP, stats[StatTypes.MMP], false);
    }

    // Level up occurs here.
    // Happens when level is modified at all.
    protected virtual void OnLvlChangeNotification(object sender, object args)
    {
        int oldValue = (int)args;
        int newValue = stats[StatTypes.LVL];

        for (int i = oldValue; i < newValue; ++i)
        {
            LevelUp();
        }
    }

    // Base level up.
    // Might consider it being a bit more
    // based on the characters potential.
    void LevelUp()
    {
        for(int i = 0; i < statOrder.Length; ++i)
        {
            StatTypes type = statOrder[i];
            // This is a fire emblem like level system.
            // Stat growth occurs in a fashion similar to fire emblem
            // as in int is a straight increase per level and float value
            // is rolled to see if you can gain an aditional of that stat.
            // Eg 3.2 in mhp = 3 per level + (20% chance + 1).
            int whole = Mathf.FloorToInt(growStats[i]);
            float fraction = growStats[i] - whole;

            int value = stats[type];
            value += whole;
            // Roll for additional stat.
            if (UnityEngine.Random.value > (1f - fraction))
                value++;

            stats.SetValue(type, value, false);
        }

        // I don't know if I like leveling up recovering all hp.
        stats.SetValue(StatTypes.HP, stats[StatTypes.MHP], false);
        stats.SetValue(StatTypes.MP, stats[StatTypes.MMP], false);
    }

}
