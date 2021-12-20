using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectUnitState : BattleState
{
    int index = -1;

    public override void Enter()
    {
        base.Enter();
        StartCoroutine("ChangeCurrentUnit");
    }

    public override void Exit()
    {
        base.Exit();
        statPanelContoller.HidePrimary();
    }

    IEnumerator ChangeCurrentUnit()
    {
        if (units.Count > 0)
        {

            m_owner.round.MoveNext();
            SelectTile(turn.actor.m_tile.m_pos);
            // Refresh here as the turn change swaps then
            // pulls up correct data. Rather than display
            // last turns unit.
            RefreshPrimaryStatPanel(m_pos);
            yield return null;
            m_owner.ChangeState<CommandSelectionState>();
        }
        else
            m_owner.ChangeState<ExploreState>();
    }

}
