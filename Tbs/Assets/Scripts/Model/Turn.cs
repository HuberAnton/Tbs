using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turn
{
    public Unit actor;
    public bool hasUnitMoved;
    public bool hasUnitActed;
    public bool lockMove;
    Tile startTile;
    Directions startDir;
    public Ability ability;
    public List<Tile> targets;
    public PlanOfAttack plan;

    public Driver driver;
    public Alliances acitveAlliance;

    // Should probably make these ques?

    public List<Unit> aiDrivenUnacted = new List<Unit>();
    public List<Unit> playerDrivenUnacted = new List<Unit>();
    public List<Unit> playerDrivenActed = new List<Unit>();

    public bool endTurn;

    public int completedTurns = 0;

    public const string AllUnitsCompletedActionNotificaiton = "Turn.AllUnitsCompletedAction";



    // Changing unit.
    public void Change(Unit current)
    {
        if(actor != null && driver.Current == Drivers.Human)
        {
            actor.RemoveObserver(EndTurnCheck, TurnOrderController.TurnCompletedNotification);
        }
        actor = current;

        hasUnitMoved = false;
        hasUnitActed = false;
        lockMove = false;
        startTile = actor.m_tile;
        startDir = actor.m_direction;
        plan = null;
        driver = current.GetComponent<Driver>();
        if(driver.Current == Drivers.Human)
         actor.AddObserver(EndTurnCheck, TurnOrderController.TurnCompletedNotification);
    }

    public void Change(Alliances alliance, List<Unit> units)
    {
        // Active alliance for ui
        acitveAlliance = alliance;
        // Cycle through units in alliance

        playerDrivenUnacted.Clear();
        aiDrivenUnacted.Clear();
        playerDrivenActed.Clear();

        for (int i = 0;i < units.Count; ++i)
        {
            // Sort units by how they are driven in the alligence.
            Driver driver = units[i].GetComponent<Driver>();
            if(driver.Current == Drivers.Human)
            {
                playerDrivenUnacted.Add(units[i]);
            }
            else
            {
                aiDrivenUnacted.Add(units[i]);
            }
        }
        endTurn = false;
    }


    //// May be redundant.
    //public IEnumerator CycleUnit()
    //{
    //    while (true)
    //    {
    //        for (int i = 0; i < unitList.Count - 1; ++i)
    //        {
    //            // Units should be 
    //            driver = unitList[i].GetComponent<Driver>();
    //            yield return unitList[i];

    //        }


    //        // for ai this will auto move.

    //        // post all units completed actions.
    //        // This will cause a popup for the player to move to 
    //        // next alliance.
    //        this.PostNotification(AllUnitsCompletedActionNotificaiton);
    //    }
    //        // All units cycled.
    //        // Move to next turn.
    //}

    public void UndoMove()
    {
        hasUnitMoved = false;
        actor.Place(startTile);
        actor.m_direction = startDir;
        actor.Match();
    }

    public bool CanPerformAction(int cost)
    {
        return actor.GetComponent<Stats>()[StatTypes.AP] >= cost;
    }

    public Unit GetNextAiUnit()
    {
        if (aiDrivenUnacted.Count > 0)
        {
            Unit unit = aiDrivenUnacted[0];
            aiDrivenUnacted.Remove(unit);
            return unit;
        }
        else
            return null;
    }

    // Shouldn't be like this.
    // Instead should post that a 
    public Unit GetNextPlayerUnit()
    {
        if (playerDrivenUnacted.Count > 0)
        {
            Unit unit = playerDrivenUnacted[0];
            return unit;
        }
        else
            return null;
    }

    // Unit is the sender. No args
    public void EndTurnCheck(object sender, object args)
    {
        Unit unit = (Unit)sender;
        if (!playerDrivenActed.Contains(unit))
        {
            playerDrivenActed.Add(unit);
        }

        if(playerDrivenActed.Count == playerDrivenUnacted.Count)
        {
            endTurn = true;
        }
    }


}