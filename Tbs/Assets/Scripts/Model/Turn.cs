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
    public List<Unit> unitList;

    // Changing unit.
    public void Change(Unit current)
    {
        actor = current;
        hasUnitMoved = false;
        hasUnitActed = false;
        lockMove = false;
        startTile = actor.m_tile;
        startDir = actor.m_direction;
        plan = null;
    }

    public void Change(Alliances alliance, List<Unit> units)
    {
        acitveAlliance = alliance;
        unitList = units;
        driver = unitList[0].GetComponent<Driver>();
        plan = null;
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

}

// Need to convert this class to a new variant of turn
// Should have an alliance field. Turn order controller changes that.
// Actor will need to be removed?
// Change should store a list of units.