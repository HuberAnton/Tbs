using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerformAbilityState : BattleState
{
    public override void Enter()
    {
        base.Enter();
        turn.hasUnitActed = true;
        // Can't revert movement after attack.
        if (turn.hasUnitMoved)
            turn.lockMove = true;

        StartCoroutine(Animate());
    }

    IEnumerator Animate()
    {    
        yield return StartCoroutine(turn.ability.PerformCR(turn.targets));
        // Apply ability effects
        // This is where you should look for the
        // attack on the character.
        //ApplyAbility();

        if (IsBattleOver())
            m_owner.ChangeState<CutSceneState>();
        else if (!UnitHasControl())
            m_owner.ChangeState<SelectUnitState>();
        else if (turn.hasUnitMoved)
            m_owner.ChangeState<EndFacingState>();
        else
            m_owner.ChangeState<CommandSelectionState>();
    }

    void ApplyAbility()
    {
        turn.ability.Perform(turn.targets);
    }
    

    bool UnitHasControl()
    {
        return turn.actor.GetComponentInChildren<KnockOutStatusEffect>() == null;
    }

}
