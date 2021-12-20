using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// A wrapper for mp.
// Might reference this in ui instead.
public class Mana : MonoBehaviour
{
    public int MP
    {
        get { return stats[StatTypes.MP]; }
        set { stats[StatTypes.MP] = value; }
    }

    public int MMP
    {
        get { return stats[StatTypes.MMP]; }
        set { stats[StatTypes.MMP] = value; }
    }
    Stats stats;
    Unit unit;
    private void Awake()
    {
        stats = GetComponent<Stats>();
        unit = GetComponent<Unit>();
    }

    private void OnEnable()
    {
        this.AddObserver(OnMPWillChange, Stats.WillChangeNotification(StatTypes.MP), stats);
        this.AddObserver(OnMMPDidChange, Stats.DidChangeNotification(StatTypes.MMP), stats);
        this.AddObserver(OnTurnBegin, TurnOrderController.TurnBeganNotification, unit);
    }

    private void OnDisable()
    {
        this.RemoveObserver(OnMPWillChange, Stats.WillChangeNotification(StatTypes.MP));
        this.RemoveObserver(OnMMPDidChange, Stats.DidChangeNotification(StatTypes.MMP));
        this.RemoveObserver(OnTurnBegin, TurnOrderController.TurnBeganNotification, unit);
    }


    // Ensures Mp does not go beyond max or min.
    void OnMPWillChange(object sender, object args)
    {
        ValueChangeException vce = (ValueChangeException)args;
        vce.AddModifier(new ClampValueModifier(int.MaxValue, 0, stats[StatTypes.MHP]));
    }

    // When MMP is changed due to equipment, or buff.
    void OnMMPDidChange(object sender, object args)
    {
        // Will increase hp when MMP is adjusted.
        //int oldMMP = (int)args;
        //if (MHP > oldMHP)
        //    HP += MHP - oldMMP;
        //else
        // If MP above MMP set to max.
        MP = Mathf.Clamp(MP, 0, MMP);
    }

    // Every turn will recover an amount
    // of mp based on max.
    void OnTurnBegin(object sender, object args)
    {
        if(MP < MMP)
        {
            // Recovers 10% reounded down to a min of 1
            // each turn as long as not at max.
            // Note that this will also trigger OnMPWillChange.
            MP += Mathf.Max(Mathf.FloorToInt(MMP * 0.1f), 1);
        }
    }


}
