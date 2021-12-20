using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Any status that needs to be auto applied to a unit
// will occur in here. 
// Eg: hp = 0 result in ko status being added.
public class AutoStatusController : MonoBehaviour
{
    private void OnEnable()
    {
        this.AddObserver(OnHpDidChangeNotification, Stats.DidChangeNotification(StatTypes.HP));

    }

    private void OnDisable()
    {
        this.RemoveObserver(OnHpDidChangeNotification, Stats.DidChangeNotification(StatTypes.HP));
    }

    void OnHpDidChangeNotification(object sender, object args)
    {
        Stats stats = (Stats)sender;
        if(stats[StatTypes.HP] == 0)
        {
            Status status = stats.GetComponentInChildren<Status>();
            StatComparisonCondition c = status.Add<KnockOutStatusEffect, StatComparisonCondition>();
            c.Init(StatTypes.HP, 0, c.EqualTo);
        }
    }
}
