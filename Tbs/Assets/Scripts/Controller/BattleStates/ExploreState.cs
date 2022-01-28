using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExploreState : BattleState
{
    public override void Enter()
    {
        base.Enter();
        // If highlighting a unit, display stat panel.
        RefreshPrimaryStatPanel(m_pos);

        // Update tile ui.
        tileCoordinateController.Show(m_board.m_tiles[m_pos]);
    }

    public override void Exit()
    {
        base.Exit();
        statPanelContoller.HidePrimary();
        tileCoordinateController.Hide();
    }

    protected override void OnMove(object sender, InfoEventArgs<Point> e)
    {
        e.m_info.GetAdjustedPoint();
        SelectTile(e.m_info + m_pos);
        RefreshPrimaryStatPanel(m_pos);
        tileCoordinateController.UpdateCoordinates(m_board.m_tiles[m_pos]);
    }

    protected override void OnFire(object sender, InfoEventArgs<int> e)
    { 
        // Firekey 0 in unity input controller.
        if (e.m_info == 0)//units.Count > 0)
        {
            Unit unit = GetUnit(m_pos);
            if (unit != null)
            {
                if (turn.unactedUnits.Contains(unit))
                {
                    turn.Change(unit);
                    m_owner.ChangeState<CommandSelectionState>();
                }
            }




            // Set the currently selected unit to the active unit in bc
        }

    }

}
