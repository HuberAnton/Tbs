using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectUnitState : BattleState
{
    int index = -1;

    public override void Enter()
    {
        base.Enter();
        //StartCoroutine("ChangeCurrentUnit");
        StartCoroutine("ChangeCurrentAlliance");
        //ChangeCurrentAlliance();
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

    IEnumerator ChangeCurrentAlliance()
    {
        if(alliances.Count > 1)//should be 1
        {
            m_owner.round.MoveNext();
            // Halt a frame to make sure transition is completed.
            yield return null;
            if (turn.driver.Current == Drivers.Computer)
            {
                // This will bite me in the ass.
                //Unit activeUnit = turn.aiDrivenUnacted[0];
                //turn.aiDrivenUnacted.Remove(activeUnit);
                //turn.Change(activeUnit);
                m_owner.ChangeState<CommandSelectionState>();
            }
            else
            {
                //turn.Change(turn.playerDrivenUnacted[0]);
                SelectTile(turn.actor.m_tile.m_pos);
                m_owner.ChangeState<ExploreState>();
            }
        }
        else
        {
            // Check win condition.
        }
    }

}