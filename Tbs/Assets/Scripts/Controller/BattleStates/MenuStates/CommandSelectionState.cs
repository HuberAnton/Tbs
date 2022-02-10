using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandSelectionState : BaseAbilityMenuState
{
    // Shows and hides stat panels
    public override void Enter()
    {
        base.Enter();
        statPanelContoller.ShowPrimary(turn.actor.gameObject);
        if (driver.Current == Drivers.Computer)
            StartCoroutine(ComputerTurn());
    }

    public override void Exit()
    {
        base.Exit();
        statPanelContoller.HidePrimary();
    }

    protected override void LoadMenu()
    {
        if(menuOptions == null)
        {
            // Need a place to store menuOptions.
            // Actuall it's better being here tied to the state
            // as the other menu states will do the same.
            menuTitle = "Commands";
            // How large the menu is.
            // I imagine this is used with the pooling.
            menuOptions = new List<string>(3);
            menuOptions.Add("Move");
            menuOptions.Add("Action");
            menuOptions.Add("Wait");
        }
        abilityMenuPanelController.Show(menuTitle, menuOptions);
        abilityMenuPanelController.SetLocked(0, !turn.CanPerformAction(1));
        abilityMenuPanelController.SetLocked(1, !turn.CanPerformAction(1));
    }


    // If you add or modify the amount of
    // menu options you should adjust this.
    // Or find a better way possibly.
    protected override void Confirm()
    {
        switch(abilityMenuPanelController.selection)
        {
            // Move
            case 0:
                m_owner.ChangeState<MoveTargetState>();
                break;
            // Action
            case 1:
                m_owner.ChangeState<CategorySelectionState>();
                break;
            // Wait
            case 2:
                turn.completedAction = true;
                m_owner.ChangeState<EndFacingState>();
                break;
        }
    }

    protected override void Cancel()
    {
        if(turn.hasUnitMoved && !turn.lockMove)
        {
            turn.UndoMove();
            abilityMenuPanelController.SetLocked(0, false);
            SelectTile(turn.actor.m_tile.m_pos);
        }
        else
        {
            m_owner.ChangeState<ExploreState>();
        }
    }

    // Computer turn.
    IEnumerator ComputerTurn()
    {
        if(turn.plan == null)
        {
            turn.plan = m_owner.cpu.Evaluate();
            turn.ability = turn.plan.ability;
        }

        // Pause to give the perception of 'thinking'
        yield return new WaitForSeconds(1f);

        // Will this force the ai to make decisions in this order?

        if (turn.hasUnitMoved == false && turn.plan.moveLocation != turn.actor.m_tile.m_pos)
            m_owner.ChangeState<MoveTargetState>();
        else if (turn.hasUnitActed == false && turn.plan.ability != null)
            m_owner.ChangeState<AbilityTargetState>();
        else
            m_owner.ChangeState<EndFacingState>();
    }
}