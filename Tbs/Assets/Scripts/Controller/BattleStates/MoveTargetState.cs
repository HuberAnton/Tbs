using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTargetState : BattleState
{
    List<Tile> tiles;
    public override void Enter()
    {
        base.Enter();
        SelectTile(m_pos);
        Movement mover = turn.actor.GetComponent<Movement>();
        tiles = mover.GetTilesInRange(m_board);
        m_board.SelectedTiles(tiles);
        RefreshPrimaryStatPanel(m_pos);


        // Ai cursor movement
        if (driver.Current == Drivers.Computer)
            StartCoroutine(ComputerHighlightedMoveTarget());

        tileCoordinateController.Show(m_board.m_tiles[m_pos]);

    }


    public override void Exit()
    {
        base.Exit();
        m_board.DeSelectTiles(tiles);
        tiles = null;
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
        if(e.m_info == 0)
        {
            if (tiles.Contains(m_owner.m_currentTile))
                m_owner.ChangeState<MoveSequenceState>();
        }
        else
        {
            m_owner.ChangeState<CommandSelectionState>();
        }
    }

    IEnumerator ComputerHighlightedMoveTarget()
    {
        yield return new WaitForSeconds(0.5f);
        Point cursorPos = m_pos;
        Point offset = new Point(0, 0);
        while (cursorPos + offset != turn.plan.moveLocation)
        {
            // Moves the cursor towards the ai's target.
          

            // Should allow for a check to see if skip button pushed.
            //if(skipbutton) cursorPos = turn.plan.moveLocation;
            if (cursorPos.m_x + offset.m_x < turn.plan.moveLocation.m_x) offset.m_x++;
            else if (cursorPos.m_x + offset.m_x > turn.plan.moveLocation.m_x) offset.m_x--;
            else if (cursorPos.m_y + offset.m_y < turn.plan.moveLocation.m_y) offset.m_y++;
            else if (cursorPos.m_y + offset.m_y > turn.plan.moveLocation.m_y) offset.m_y--;

            SelectTile(cursorPos + offset);
            tileCoordinateController.UpdateCoordinates(m_board.m_tiles[m_pos]);
            yield return new WaitForSeconds(0.25f);
        }
        yield return new WaitForSeconds(0.5f);
        m_owner.ChangeState<MoveSequenceState>();
    }
}
