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
        // Get the attack effects
        // ability.Perform(m_owner.m_targets);
        // add an ability

        // Play animations/
        // Particles/Projectiles. ect
        yield return null;
        // Apply ability effects
        // This is where you should look for the
        // attack on the character.
        ApplyAbility();

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

    // These should be called by the ablilites themselevs.
    // Leaing these hear for a moment.
    void OnApply(object sender)
    {

    }
    void OnComplete(object sender)
    {

    }

}
