using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class EndFacingState : BattleState
{
    Directions startDir;

    public override void Enter()
    {
        base.Enter();
        startDir = turn.actor.m_direction;
        SelectTile(turn.actor.m_tile.m_pos);
        m_owner.facingIndicator.gameObject.SetActive(true);
        m_owner.facingIndicator.SetDirection(startDir);
        if (driver.Current == Drivers.Computer)
            StartCoroutine(ComputerControl());
    }

    public override void Exit()
    {
        base.Exit();
        m_owner.facingIndicator.gameObject.SetActive(false);
    }

    protected override void OnMove(object sender, InfoEventArgs<Point> e)
    {
        // Since the event argument is just a point with a max range of -1 - 1 
        // on both axis you just apply the direction to 
        turn.actor.m_direction = e.m_info.GetDirections();
        m_owner.facingIndicator.SetDirection(e.m_info.GetDirections());
        turn.actor.Match();
    }

    protected override void OnFire(object sender, InfoEventArgs<int> e)
    {
        switch (e.m_info)
        {
            case 0:
                m_owner.ChangeState<SelectUnitState>();
                break;
            case 1:
                turn.actor.m_direction = startDir;
                turn.actor.Match();
                m_owner.ChangeState<CommandSelectionState>();
                break;
        }
    }

    IEnumerator ComputerControl()
    {
        yield return new WaitForSeconds(0.5f);
        turn.actor.m_direction = m_owner.cpu.DetermineEndFacingDirection();
        turn.actor.Match();
        m_owner.facingIndicator.SetDirection(turn.actor.m_direction);
        yield return new WaitForSeconds(0.5f);
        m_owner.ChangeState<SelectUnitState>();
    }

}