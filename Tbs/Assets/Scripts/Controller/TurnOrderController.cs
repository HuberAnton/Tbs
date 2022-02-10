using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnOrderController : MonoBehaviour
{
    // Turn activation occurs when units
    // CTR stat reaches this value.
    const int turnActivation = 1000;
    // - this value whenever unit activates 
    const int turnCost = 500;
    // - this value on move
    const int moveCost = 300;
    // - this value on action
    const int actionCost = 200;

    // Cycles to the next activateable alliance.
    bool endTurn = false;

    // For adding observers.
    // Makes the process much s
    public const string RoundBeginNotificaiton = "TurnOrderController.roundBegan";
    public const string TurnCheckNotification = "TurnOrderController.turnCheck";
    public const string TurnCompletedNotification = "TurnOrderController.turnCompleted";
    public const string RoundEndedNotificaiton = "TurnOrderController.roundEnded";
    public const string TurnBeganNotification = "TurnOrderController.TurnBeganNotification";


    // In case there are triggers attached to specific alliances turns beginning
    static Dictionary<Alliances, string> _allianceTurnBeginNotifcation = new Dictionary<Alliances, string>();
    public static string AllianceTurnBeginNotificaiton(Alliances alliance)
    {
        if (!_allianceTurnBeginNotifcation.ContainsKey(alliance))
            _allianceTurnBeginNotifcation.Add(alliance, string.Format("TurnOrderController.{0}BeginTurn", alliance.ToString()));
        return _allianceTurnBeginNotifcation[alliance];
    }

    static Dictionary<Alliances, string> _allianceTurnEndNotifcation = new Dictionary<Alliances, string>();
    public static string AllianceTurnEndNotificaiton(Alliances alliance)
    {
        if (!_allianceTurnEndNotifcation.ContainsKey(alliance))
            _allianceTurnEndNotifcation.Add(alliance, string.Format("TurnOrderController.{0}EndTurn", alliance.ToString()));
        return _allianceTurnEndNotifcation[alliance];
    }

    // Old turn order controller code
    // Based around the ctr version.
    public IEnumerator Round()
    {
        // For shorthand.
        BattleController bc = GetComponent<BattleController>();


        // A 'round'
        while (true)
        {
            // Begin round effects.
            this.PostNotification(RoundBeginNotificaiton);
            List<Unit> units = new List<Unit>(bc.units);


            // Cycles through all units and ads speed to Ctr stat.
            for (int i = 0; i < units.Count; ++i)
            {
                Stats s = units[i].GetComponent<Stats>();
                // So speed value makes the unit act quicker.

                // Remember that this way of incrementing
                // allows for excpetions to trigger.
                // Check Stats property for more info.
                s[StatTypes.CTR] += s[StatTypes.SPD];
            }
            // Sort each unit based on speed.
            // Note that this is soring a copy of the list.
            units.Sort((a, b) => GetCounter(a).CompareTo(GetCounter(b)));

            // Cycles through all units until 
            // to see if they can activate this 'round'
            for (int i = units.Count - 1; i >= 0; --i)
            {
                // Will only activate a unit if 1000 ctr.
                // otherwise will skip.
                if (CanTakeTurn(units[i]))
                {
                    // Activate unit
                    bc.turn.Change(units[i]);
                    // Any effects to occur at the units turn start
                    // occur now before control is gained.
                    units[i].PostNotification(TurnBeganNotification);

                    // Should wait here until movenext is called on
                    // this coroutine
                    // If you access the corrountine it will return the unit.
                    yield return units[i];
                    // After unit activated
                    // reduce its ctr based on
                    // its actions
                    int cost = turnCost;

                    if (bc.turn.hasUnitMoved)
                        cost += moveCost;
                    if (bc.turn.hasUnitActed)
                        cost += actionCost;

                    Stats s = units[i].GetComponent<Stats>();
                    s.SetValue(StatTypes.CTR, s[StatTypes.CTR] - cost, false);
                    // Unit specific turn completion notifications.
                    // For any effects on unit at its wait.
                    units[i].PostNotification(TurnCompletedNotification);
                }
            }
            // End of round for any end of round effects.
            // Only occurs after all units have been checked
            // to see if can activate.
            this.PostNotification(RoundEndedNotificaiton);
        }
    }

    // Uses exception system to check
    // if can take trun or not.
    bool CanTakeTurn(Unit target)
    {
        // Makes an exception that will be true if ctr above 1000
        BaseException exc = new BaseException(GetCounter(target) >= turnActivation);
        // Checks if there are any exception that would modify
        // the units ability to act. 
        target.PostNotification(TurnCheckNotification, exc);
        return exc.toggle;
    }

    // Just a wrapper to get ctr stat quickly.
    int GetCounter(Unit target)
    {
        return target.GetComponent<Stats>()[StatTypes.CTR];
    }

    Driver GetDriver(Unit target)
    {
        return target.GetComponent<Driver>();
    }


    // Cycles through each alliance.
    // 
    public IEnumerator CurrentTurn()
    {
        BattleController bc = GetComponent<BattleController>();

        while (true)
        {
            this.PostNotification(RoundBeginNotificaiton);

            // Set the alliances units to the current units.
            // Add the appropriate action points back to the unit.
            foreach (KeyValuePair<Alliances, List<Unit>> kvp in bc.alliances)
            {
                // Cycle through each unit in the alliance and apply max ap.
                for (int i = 0; i < kvp.Value.Count; ++i)
                {
                    Stats s = kvp.Value[i].GetComponent<Stats>();
                    // Will ensure modifiers take effect.
                    s[StatTypes.AP] = s[StatTypes.APMAX];
                }

                // Changes alliance
                bc.turn.Change(kvp.Key, kvp.Value);
                AllianceTurnBeginNotificaiton(kvp.Key);

                Unit nextUnit;

                while (!bc.turn.endTurn)
                {
                    nextUnit = bc.turn.GetNextUnit();
                    if (nextUnit)
                    {
                        nextUnit.PostNotification(TurnBeganNotification);
                        bc.turn.Change(nextUnit);
                        yield return nextUnit;
                        nextUnit.PostNotification(TurnCompletedNotification);
                    }
                }

                AllianceTurnEndNotificaiton(kvp.Key);
            }

            // Nothing else should be handled by the turn controller as
            // it has set the correct units to be activated this round.

            this.PostNotification(RoundEndedNotificaiton);
        }
    }
}