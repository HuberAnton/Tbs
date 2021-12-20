using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Enter this state from abiity target state.
// Highlights effected area of ability.
// May want some of this effect layered on top of the
// target state and if so might skip this state all together. 
public class ConfirmAbilityTargetState : BattleState
{
    List<Tile> tiles;
    AbilityArea aa;
    int index = 0;

    AbilityEffectTarget[] targeters;

    public override void Enter()
    {
        base.Enter();
        aa = turn.ability.GetComponent<AbilityArea>();
        tiles = aa.GetTilesInArea(m_board, m_pos);
        m_board.SelectedTiles(tiles);
        FindTargets();
        RefreshPrimaryStatPanel(turn.actor.m_tile.m_pos);
        if (turn.targets.Count > 0)
        {
            // Does not show hit success for ai.
            // Might want to leave this in?
            if (driver.Current == Drivers.Human)
                hitSuccessIndicator.Show();
            SetTarget(0);
        }

        if (driver.Current == Drivers.Computer)
            StartCoroutine(ComputerDisplayAbilitySelection());
    }

    public override void Exit()
    {
        base.Exit();
        m_board.DeSelectTiles(tiles);
        statPanelContoller.HidePrimary();
        statPanelContoller.HideSecondary();
        hitSuccessIndicator.Hide();
    }

    // Cycles through targets in attack area.
    // If no targets will not be able to move around.
    protected override void OnMove(object sender, InfoEventArgs<Point> e)
    {
        if (e.m_info.m_y > 0 || e.m_info.m_x > 0)
            SetTarget(index + 1);
        else
            SetTarget(index - 1);

    }

    
    protected override void OnFire(object sender, InfoEventArgs<int> e)
    {
        if(e.m_info == 0)
        {
            // This makes it only do the action
            // if something is targeted.
            if(turn.targets.Count > 0)
            {
                m_owner.ChangeState<PerformAbilityState>();
            }

        }
        else
        {
            m_owner.ChangeState<AbilityTargetState>();
        }
    }

    void FindTargets()
    {
        turn.targets = new List<Tile>();
        targeters = turn.ability.GetComponentsInChildren<AbilityEffectTarget>();
        for(int i = 0; i < tiles.Count; ++i)
        {
            if (IsTarget(tiles[i], targeters))
                turn.targets.Add(tiles[i]);
        }
    }

    bool IsTarget(Tile tile, AbilityEffectTarget[] list)
    {
        for (int i = 0; i < list.Length; ++i)
        {
            if (list[i].IsTarget(tile))
                return true;
        }
        return false;
    }

    // Allows you to cycle through all
    // targets in range of an attack to see
    // impact of ability on them.
    void SetTarget(int target)
    {
        // Index is adjsuted in on move
        index = target;
        if (index < 0)
            index = turn.targets.Count - 1;
        if (index >= turn.targets.Count)
            index = 0;

        if (turn.targets.Count > 0)
        {
            RefreshSecondaryStatPanel(turn.targets[index].m_pos);
            UpdateHitSuccessIndicator();
        }
    }


    void UpdateHitSuccessIndicator()
    {
        int chance = 0;
        int amount = 0;
        Tile target = turn.targets[index];
        // Cycle through targets.
        // For ui.
        for(int i = 0; i < targeters.Length; ++i)
        {
            if(targeters[i].IsTarget(target))
            {
                HitRate hitRate = targeters[i].GetComponent<HitRate>();
                chance = hitRate.Calculate(target);

                BaseAbilityEffect effect = targeters[i].GetComponent<BaseAbilityEffect>();
                amount = effect.Predict(target);
                break;
            }
        }
        hitSuccessIndicator.SetState(chance, amount);
    }


    // Ai turn.
    IEnumerator ComputerDisplayAbilitySelection()
    {
        m_owner.battleMessageController.Display(turn.ability.name);
        yield return new WaitForSeconds(2f);
        m_owner.ChangeState<PerformAbilityState>();
    }


    //int CalculateHitRate()
    //{
    //    //Unit target = turn.targets[index].m_content.GetComponent<Unit>();
    //    Tile target = turn.targets[index];

    //    HitRate hr = turn.ability.GetComponentInChildren<HitRate>();
    //    return hr.Calculate(target);
    //}

    //int EstimateDamage()
    //{
    //    // No damage calculation yet.
    //    return 50;
    //}
}
