using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseAbilityMenuState : BattleState
{
    protected string menuTitle;
    protected List<string> menuOptions;

    public override void Enter()
    {
        base.Enter();
        SelectTile(turn.actor.m_tile.m_pos);
        // If player load menu else skip.
        if(driver.Current == Drivers.Human)
            LoadMenu();
        tileCoordinateController.Show(m_board.m_tiles[m_pos]);
    }

    public override void Exit()
    {
        base.Exit();
        abilityMenuPanelController.Hide();
        tileCoordinateController.Hide();
    }

    protected override void OnFire(object sender, InfoEventArgs<int> e)
    {
        // fire 0
        if (e.m_info == 0)
            Confirm();
        // This is not quite ideal.
        // But lets see how it handles overwriting it.
        else
            Cancel();
    }

    protected override void OnMove(object sender, InfoEventArgs<Point> e)
    {
        if (e.m_info.m_x > 0 || e.m_info.m_y < 0)
            abilityMenuPanelController.Next();
        else
            abilityMenuPanelController.Previous();
    }

    protected abstract void LoadMenu();
    protected abstract void Confirm();
    protected abstract void Cancel();

}
