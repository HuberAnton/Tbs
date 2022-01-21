using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveSequenceState : BattleState
{
    public override void Enter()
    {
        base.Enter();
        // Need to add an event to skip sequence.
        StartCoroutine("Sequence");
    }

    IEnumerator Sequence()
    {
        Movement m = turn.actor.GetComponent<Movement>();
        yield return StartCoroutine(m.Traverse(m_owner.m_currentTile));
        // Needs to be converted to a subtraciton of ap.
        //turn.hasUnitMoved = true;

        m_owner.ChangeState<CommandSelectionState>();
    }

}
