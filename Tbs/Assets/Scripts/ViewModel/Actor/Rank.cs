using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Handles experience 
// and experience distribution.
// If you want to change the rate thigns level
// It happens here.
public class Rank : MonoBehaviour
{
    public const int minLevel = 1;
    public const int maxLevel = 99;
    public const int maxExperience = 999999;
    
    Stats stats;

    public int LVL
    {
        get { return stats[StatTypes.LVL]; }
    }

    public int EXP
    {
        get { return stats[StatTypes.EXP]; }
        set { stats[StatTypes.EXP] = value; }
    }

    // Has an impact on experience gained from doing actions?
    public float LevelPercent
    {
        get { return (float)(LVL - minLevel) / (float)(maxLevel - minLevel); }
    }

    private void Awake()
    {
        stats = GetComponent<Stats>();
    }

    // Add and remove observers.
    private void OnEnable()
    {
        this.AddObserver(OnExpWillChange,
            Stats.WillChangeNotification(StatTypes.EXP), stats);
        this.AddObserver(OnExpDidChange,
            Stats.DidChangeNotification(StatTypes.EXP), stats);
    }
    
    private void OnDisable()
    {
        this.RemoveObserver(OnExpWillChange,
            Stats.WillChangeNotification(StatTypes.EXP), stats);
        this.RemoveObserver(OnExpDidChange,
            Stats.DidChangeNotification(StatTypes.EXP), stats);
    }

    // Consider allowing xp down by removing modifier
    // value.
    void OnExpWillChange(object sender, object args)
    {
        ValueChangeException vce = args as ValueChangeException;
        vce.AddModifier(new ClampValueModifier(int.MaxValue, EXP, maxExperience));
    }

    void OnExpDidChange(object sender, object args)
    {
        stats.SetValue(StatTypes.LVL, LevelForExperience(EXP), false);
    }


    // This is where you would work out the xp calculation per level.

    // Default is to increase required amount of xp
    // based on your level. I'm not super keen.
    // A new level should be earnt at 100 xp and xp
    // gained should be modified by both the difference
    // between combatants levels and the result of the action.
    // Eg  difference/2 * baseXpvlue * (fail default critical value)
    // Should look at some xp models in similar games.
    public static int ExperienceForLevel(int level)
    {
        float levelPercent = Mathf.Clamp01((float)(level - minLevel) / (float)
            (maxLevel - minLevel));
        return (int)EasingEquations.EaseInQuad(0, maxExperience, levelPercent);
    }

    public static int LevelForExperience(int EXP)
    {
        // Get max level 
        // Count down from max level
        // If Expereince is greater or exqual
        // to that levels experience return that level.
        // Means multiple levels can occur.
        // It does look rather strange though. Then again
        // it doesn't take much to count down and it will be 
        // even simpler if exp is straight up 100 per level.
        int lvl = maxLevel;
        for (; lvl >= minLevel; --lvl)
            if (EXP >= ExperienceForLevel(lvl))
                break;
        return lvl;
    }

    public void Init(int level)
    {
        stats.SetValue(StatTypes.LVL, level, false);
        stats.SetValue(StatTypes.EXP, ExperienceForLevel(level), false);
    }

}
