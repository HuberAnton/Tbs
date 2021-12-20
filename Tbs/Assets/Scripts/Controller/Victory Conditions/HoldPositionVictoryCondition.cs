using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// This needs some thinking.
// When you move on to the correct position it should
// add a observer that fires when you end turn
// which then increases the value and checks if win or lose.
// So the tiles are posting notifications each turn if a unit
// is on it?

// Might be better to have a component on each tile that posts
// at the end of each round if something is on them.

public class HoldPositionVictoryCondition : BaseVictoryCondition
{
    // Need to add more observers as well.
    // Will only check these on damage.
    // Need these to check on oponent turn.

    protected override void OnEnable()
    {
        base.OnEnable();
        this.AddObserver(OnMoveToPosition, TurnOrderController.TurnCompletedNotification);
        // Add observer for when target reaches position.
        // If they move check if they have reached 
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        this.RemoveObserver(OnMoveToPosition, TurnOrderController.TurnCompletedNotification);
    }


    class EnemyGoal
    {
        public Tile position;
        public int turnsInGoal;
    }

    // How many turns for the enemy to be in the position
    public readonly int turnLimit;
    [SerializeField]
    private EnemyGoal[] enemyGoal;

    // Checks if the enemy is on a winning tile and increases the value.
    // If target reached enemy wins.
    private void OnMoveToPosition(object sender, object args)
    {
        // If it is an enemy check if its position is any of the defence positions.
        if (bc.m_currentUnit.GetComponent<Alliance>().type == Alliances.Enemy)
        {
            for (int i = 0; i < enemyGoal.Length; ++i)
            {
                // Get the tile and check if current unit is movi
                if (bc.m_currentUnit.m_tile == enemyGoal[i].position)
                {
                    enemyGoal[i].turnsInGoal++;
                    if (enemyGoal[i].turnsInGoal == turnLimit)
                    {
                        Victor = Alliances.Enemy;
                        return;
                    }
                }
                
                // Check if the tile currently has an enemy on it
                // If not reduce it to 0.
                {

                }
            }
        }
    }
}
