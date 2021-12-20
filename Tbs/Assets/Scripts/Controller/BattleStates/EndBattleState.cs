using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// Ideally a return to the world map.
// Final state before map deloads.
// Possibly have a bunch of ui pop up
// with the outcome of the level.
public class EndBattleState : BattleState
{
    public override void Enter()
    {
        base.Enter();
        SceneManager.LoadScene(0);
    }
}
