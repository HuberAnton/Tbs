using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityTargetState : BattleState
{
    List<Tile> tiles;
    AbilityRange ar;

    public override void Enter()
    {
        base.Enter();
        ar = turn.ability.GetComponent<AbilityRange>();
        SelectTiles();
        statPanelContoller.ShowPrimary(turn.actor.gameObject);
        // If can be targeted check if you should
        // update the secondary panel.
        if (ar.directionOrientation == DirectionOreinationMode.MovementOnly || ar.directionOrientation == DirectionOreinationMode.RotationAndMovement)
            RefreshSecondaryStatPanel(m_pos);

        if (driver.Current == Drivers.Computer)
            StartCoroutine(ComputerHighlightTarget());
        tileCoordinateController.Show(m_board.m_tiles[m_pos]);
    }

    public override void Exit()
    {
        base.Exit();
        m_board.DeSelectTiles(tiles);
        statPanelContoller.HidePrimary();
        statPanelContoller.HideSecondary();
        tileCoordinateController.Hide();
    }


    protected override void OnMove(object sender, InfoEventArgs<Point> e)
    {
        e.m_info.GetAdjustedPoint();
        // If you are expeced to face the
        // direction to use the attack rotate towards
        // while selecting
        // Should be a switch but has some repeating code.
        if (ar.directionOrientation == DirectionOreinationMode.RotationOnly)
        {
            ChangeDirection(e.m_info);
        }
        else if(ar.directionOrientation == DirectionOreinationMode.RotationAndMovement)
        {
            SelectTile(e.m_info + m_pos);
            RefreshSecondaryStatPanel(m_pos);

            // - the x and y of the unit and point and look in which ever direciton is greater
            var difference = turn.actor.m_tile.m_pos - m_pos;
            var result = Mathf.Abs(difference.m_x) >= Mathf.Abs(difference.m_y) ? new Point(-difference.m_x,0) : new Point(0,-difference.m_y);

            ChangeDirection(result);
        }
        else // movement only.
        {
            SelectTile(e.m_info + m_pos);
            RefreshSecondaryStatPanel(m_pos);
        }
        tileCoordinateController.UpdateCoordinates(m_board.m_tiles[m_pos]);
    }

    protected override void OnFire(object sender, InfoEventArgs<int> e)
    {
        // Checking for button pressed.
        // Refers to unity input system buttons.
        if(e.m_info == 0)
        {
            // Checks if you need to face to use an action
            // or if the tile exists at all on the board.
            // Irrelevant?
            if(ar.directionOrientation == DirectionOreinationMode.RotationOnly || tiles.Contains(m_board.GetTile(m_pos)))
                m_owner.ChangeState<ConfirmAbilityTargetState>();
        }
        // Any other press will result in backing out.
        else
        {
            m_owner.ChangeState<CategorySelectionState>();
        }
    }
    void ChangeDirection(Point p)
    {
        Directions dir = p.GetDirections();
        if(turn.actor.m_direction != dir)
        {
            m_board.DeSelectTiles(tiles);
            turn.actor.m_direction = dir;
            turn.actor.Match();
            SelectTiles();
        }
    }

    void SelectTiles()
    {
        tiles = ar.GetTilesInRange(m_board);
        m_board.SelectedTiles(tiles);
    }


    IEnumerator ComputerHighlightTarget()
    {
        yield return new WaitForSeconds(0.5f);
        if (ar.directionOrientation == DirectionOreinationMode.RotationOnly)
        {
            ChangeDirection(turn.plan.attackDirection.GetNormal());

            // Rotate and wait.
            yield return new WaitForSeconds(0.25f);
        }
        else
        {
            Point cursorPos = m_pos;
            Point offset = new Point(0, 0);
            while(cursorPos + offset != turn.plan.fireLocation)
            {
                // Skip if it exits feature
                //if(skipbutton) cursorPos = turn.plan.fireLocation;
                if (cursorPos.m_x + offset.m_x < turn.plan.fireLocation.m_x) offset.m_x++;
                else if (cursorPos.m_x + offset.m_x > turn.plan.fireLocation.m_x) offset.m_x--;
                else if (cursorPos.m_y + offset.m_y < turn.plan.fireLocation.m_y) offset.m_y++;
                else if (cursorPos.m_y + offset.m_y > turn.plan.fireLocation.m_y) offset.m_y--;
                SelectTile(cursorPos + offset);
                yield return new WaitForSeconds(0.25f);
            }
        }
        yield return new WaitForSeconds(0.5f);
        m_owner.ChangeState<ConfirmAbilityTargetState>();
    }
}