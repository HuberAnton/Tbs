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

    public List<Unit> actedUnits = new List<Unit>();
    public List<Unit> unactedUnits = new List<Unit>();

    public bool completedAction = false;
    public bool endTurn;

    public const string AllUnitsCompletedActionNotificaiton = "Turn.AllUnitsCompletedAction";

    // Changing unit.
    public void Change(Unit current)
    {
        actor.RemoveObserver(EndTurnCheck, TurnOrderController.TurnCompletedNotification);
        actor = current;
        actor.AddObserver(EndTurnCheck, TurnOrderController.TurnCompletedNotification);
        hasUnitMoved = false;
        hasUnitActed = false;
        lockMove = false;
        startTile = actor.m_tile;
        startDir = actor.m_direction;
        plan = null;
        driver = current.GetComponent<Driver>();
    }

    public void Change(Alliances alliance, List<Unit> units)
    {
        // Active alliance for ui
        acitveAlliance = alliance;
        // Cycle through units in alliance

        actedUnits.Clear();
        unactedUnits.Clear();

        // Add ai units to front
        for(int i = 0; i < units.Count; ++i)
        {
            if (units[i].GetComponent<Driver>().Current == Drivers.Computer)
                unactedUnits.Insert(0, units[i]);
            else
                unactedUnits.Add(units[i]);
        }
        endTurn = false;
    }

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

    public Unit GetNextUnit()
    {
        if(unactedUnits.Count > 0)
        {
            return unactedUnits[0];
        }
        else
        {
            endTurn = true;
            return null;
        }
    }

    // Checks if a unit can be activated again or not.
    public void EndTurnCheck(object sender, object args)
    {
        Unit unit = (Unit)sender;
        if(unit.GetComponent<Driver>().Current == Drivers.Computer || completedAction == true || unit.GetComponent<Stats>()[StatTypes.AP] == 0)
        {
            // No longer activatable
            actedUnits.Add(unit);
            unactedUnits.Remove(unit);
        }
    }


}