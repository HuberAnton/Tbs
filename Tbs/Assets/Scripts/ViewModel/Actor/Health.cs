using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// A wrapper for hp.
// Might reference this in ui instead.
public class Health : MonoBehaviour
{
    public int HP
    {
        get { return stats[StatTypes.HP]; }
        set { stats[StatTypes.HP] = value; }
    }

    public int MHP
    {
        get { return stats[StatTypes.MHP]; }
        set { stats[StatTypes.MHP] = value; }
    }
    Stats stats;
    public int MinHp = 0;
    //Unit unit;
    private void Awake()
    {
        stats = GetComponent<Stats>();
    }

    private void OnEnable()
    {
        this.AddObserver(OnHPWillChange, Stats.WillChangeNotification(StatTypes.HP), stats);
        this.AddObserver(OnMHPDidChange, Stats.DidChangeNotification(StatTypes.MHP), stats);

    }

    private void OnDisable()
    {
        this.RemoveObserver(OnHPWillChange, Stats.WillChangeNotification(StatTypes.HP));
        this.RemoveObserver(OnMHPDidChange, Stats.DidChangeNotification(StatTypes.MHP));
    }



    // Ensures Hp does not go beyond max or min.
    void OnHPWillChange(object sender, object args)
    {
        ValueChangeException vce = (ValueChangeException)args;
        vce.AddModifier(new ClampValueModifier(int.MaxValue, MinHp, stats[StatTypes.MHP]));
    }


    void OnMHPDidChange(object sender, object args)
    {
        // Will increase hp when MHP is adjusted.
        //int oldMHP = (int)args;
        //if (MHP > oldMHP)
        //    HP += MHP - oldMHP;
        //else
        // If HP above MHP set to max.
            HP = Mathf.Clamp(HP, MinHp, MHP);
    }



}
