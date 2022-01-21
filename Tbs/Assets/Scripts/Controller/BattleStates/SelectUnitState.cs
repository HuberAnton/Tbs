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

    // Cycle through all units that have ability points and choose the best
    // and go to command selection state.
    // Keep returning until all units have completed actions.
    IEnumerator ComputerTurn()
    {
        for (int i = 0; i < units.Count - 1; ++i)
        {
            // Maybe shuffle the unit list?
            // Have each enemy unit act as per normal.
        }
        return null;
    }

    IEnumerator ChangeCurrentAlliance()
    {
        if(alliances.Count > 0)//should be 1
        {
            m_owner.round.MoveNext();
            // Halt a frame to make sure transition is completed.
            yield return null;
            if (turn.driver.normal == Drivers.Computer)
            {
                // Needs to change to ai decision state.
                // It will then make it's decision about who to
                // move and act with as per normal state movement.
                // Once it has completed it's movement with a unit it
                // will go back to that state to check if any more units
                // are going to be moved. If not then comes back to this 
                // state.
                turn.actor = turn.unitList[0];
                // Temp.
                m_owner.ChangeState<CommandSelectionState>();
            }
            else
            {
                SelectTile(turn.unitList[0].m_tile.m_pos);
                m_owner.ChangeState<ExploreState>();
            }
        }
        else
        {
            // Check win condition.
        }
    }

}