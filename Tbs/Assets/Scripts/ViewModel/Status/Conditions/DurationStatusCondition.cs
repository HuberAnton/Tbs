using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DurationStatusCondition : StatusCondition
{
    public int duration = 10;

    private void OnEnable()
    {
        this.AddObserver(OnNewTurn, TurnOrderController.RoundBeginNotificaiton);
    }

    private void OnDisable()
    {
        this.RemoveObserver(OnNewTurn, TurnOrderController.RoundBeginNotificaiton);
    }


    void OnNewTurn(object sender, object args)
    {
        duration--;
        if (duration <= 0)
            Remove();
    }

}
