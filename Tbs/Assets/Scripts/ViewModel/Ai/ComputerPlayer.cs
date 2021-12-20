using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ComputerPlayer : MonoBehaviour
{
    BattleController bc;
    // The controlled unit
    Unit actor { get { return bc.turn.actor; } }

    Alliance alliance { get { return actor.GetComponent<Alliance>(); } }

    Unit nearestFoe;

    private void Awake()
    {
        bc = GetComponent<BattleController>();
    }

    public PlanOfAttack Evaluate()
    {
        
        PlanOfAttack poa = new PlanOfAttack();

        // Check for pattern, if none do first ability found.
        AttackPattern pattern = actor.GetComponentInChildren<AttackPattern>();
        if (pattern)
            pattern.Pick(poa);
        else
            DefaultAttackPattern(poa);

        if (IsPositionIndependent(poa))
            PlanPositionIndependent(poa);
        else if (IsDirectionIndependent(poa))
            PlanDirectionIndependent(poa);
        else
            PlanDirectionDependent(poa);

        if (poa.ability == null)
            MoveTowardsOpponent(poa);
        
        return poa;
    }

    // Checks if ability is dependant on range.
    bool IsPositionIndependent(PlanOfAttack poa)
    {
        AbilityRange range = poa.ability.GetComponent<AbilityRange>();
        return range.PositionOriented == false;
    }

    // Choose a movement option when ability dependant on range.
    void PlanPositionIndependent(PlanOfAttack poa)
    {
        List<Tile> moveOptions = GetMoveOptions();
        Tile tile = moveOptions[Random.Range(0, moveOptions.Count)];
        // Huh so is this set both move location and fire location to tile pos?
        poa.moveLocation = poa.fireLocation = tile.m_pos;
    }

    // Check if ability is dependant on facing.
    bool IsDirectionIndependent(PlanOfAttack poa)
    {
        AbilityRange range = poa.ability.GetComponent<AbilityRange>();
        return !range.directionOrientation;
    }

    void PlanDirectionIndependent(PlanOfAttack poa)
    {
        Tile startTile = actor.m_tile;
        Dictionary<Tile, AttackOption> map = new Dictionary<Tile, AttackOption>();
        AbilityRange ar = poa.ability.GetComponent<AbilityRange>();
        List<Tile> moveOptions = GetMoveOptions();

        for(int i = 0; i < moveOptions.Count;++i)
        {
            Tile moveTile = moveOptions[i];
            actor.Place(moveTile);
            List<Tile> fireOptions = ar.GetTilesInRange(bc.m_board);
            for (int j = 0; j < fireOptions.Count; ++j)
            {
                Tile fireTile = fireOptions[j];
                AttackOption ao = null;
                if(map.ContainsKey(fireTile))
                {
                    ao = map[fireTile];
                }
                else
                {
                    ao = new AttackOption();
                    map[fireTile] = ao;
                    ao.target = fireTile;
                    ao.direction = actor.m_direction;
                    RateFireLocation(poa, ao);
                }

                ao.AddMoveTarget(moveTile);
            }
        }

        actor.Place(startTile);
        List<AttackOption> list = new List<AttackOption>(map.Values);
        PickBestOption(poa, list);
    }

    void PlanDirectionDependent(PlanOfAttack poa)
    {
        Tile startTile = actor.m_tile;
        Directions startDirection = actor.m_direction;
        List<AttackOption> list = new List<AttackOption>();
        List<Tile> moveOptions = GetMoveOptions();

        // Cycle through and place unit at all
        // move options.
        for(int i = 0; i < moveOptions.Count; ++i)
        {
            Tile moveTile = moveOptions[i];
            actor.Place(moveTile);

            // Cycle through each direction at the
            // new location and then evaluate how effective
            for(int j = 0; j < 4;++j)
            {
                // Cast j into the direction enums to
                // cycle through all of them
                actor.m_direction = (Directions)j;
                AttackOption ao = new AttackOption();
                ao.target = moveTile;
                ao.direction = actor.m_direction;
                RateFireLocation(poa, ao);
                ao.AddMoveTarget(moveTile);
                list.Add(ao);
            }
        }

        // Reset unit.
        actor.Place(startTile);
        actor.m_direction = startDirection;
        PickBestOption(poa, list);
    }

    // Get move options
    // May need to review this if ai tries to access
    // tiles that aren't accessible.
    List<Tile> GetMoveOptions()
    {
        return actor.GetComponent<Movement>().GetTilesInRange(bc.m_board);
    }

    void RateFireLocation(PlanOfAttack poa, AttackOption option)
    {
        AbilityArea area = poa.ability.GetComponent<AbilityArea>();
        List<Tile> tiles = area.GetTilesInArea(bc.m_board, option.target.m_pos);
        option.areaTargets = tiles;
        option.isCasterMatch = IsAbilityTargetMatch(poa, actor.m_tile);

        for(int i = 0; i < tiles.Count; ++i)
        {
            Tile tile = tiles[i];
            if (actor.m_tile == tiles[i] || !poa.ability.IsTarget(tile))
                continue;

            bool isMatch = IsAbilityTargetMatch(poa, tile);
            option.AddMark(tile, isMatch);
        }
    }

    bool IsAbilityTargetMatch(PlanOfAttack poa, Tile tile)
    {
        bool isMatch = false;
        if (poa.target == Targets.Tile)
            isMatch = true;
        else if(poa.target != Targets.None)
        {
            Alliance other = tile.m_content.GetComponentInChildren<Alliance>();
            if (other != null && alliance.IsMatch(other, poa.target))
                isMatch = true;
        }

        return isMatch;
    }

    void PickBestOption(PlanOfAttack poa, List<AttackOption> list)
    {
        int bestScore = 1;
        List<AttackOption> bestOptions = new List<AttackOption>();
        for(int i = 0; i < list.Count; ++i)
        {
            AttackOption option = list[i];
            int score = option.GetScore(actor, poa.ability);
            if(score > bestScore)
            {
                bestScore = score;
                bestOptions.Clear();
                bestOptions.Add(option);
            }
            else if(score == bestScore)
            {
                bestOptions.Add(option);
            }
        }

        if(bestOptions.Count == 0)
        {
            poa.ability = null;
            return;
        }

        List<AttackOption> finalPicks = new List<AttackOption>();
        bestScore = 0;
        for(int i = 0; i <bestOptions.Count; ++i)
        {
            AttackOption option = bestOptions[i];
            int score = option.bestAngleBasedScore;
            if(score > bestScore)
            {
                bestScore = score;
                finalPicks.Clear();
                finalPicks.Add(option);
            }
            else if(score == bestScore)
            {
                finalPicks.Add(option);
            }
        }

        AttackOption choice = finalPicks[UnityEngine.Random.Range(0, finalPicks.Count)];
        poa.fireLocation = choice.target.m_pos;
        poa.attackDirection = choice.direction;
        poa.moveLocation = choice.bestMoveTile.m_pos;
    }

    // Passes in a funciton to the boards search to
    // return the nearest foe.
    void FindNearestFoe()
    {
        nearestFoe = null;
        
        bc.m_board.Search(actor.m_tile, 
            delegate (Tile arg1, Tile arg2)
            {
                if (nearestFoe == null && arg2.m_content != null)
                {
                    Alliance other = arg2.m_content.GetComponentInChildren<Alliance>();
                    if (other != null && alliance.IsMatch(other, Targets.Foe))
                    {
                        Unit unit = other.GetComponent<Unit>();
                        Stats stats = unit.GetComponent<Stats>();
                        if (stats[StatTypes.HP] > 0)
                        {
                            nearestFoe = unit;
                            return true;
                        }
                    }
                }
                return nearestFoe == null;
            });
    }

    // Cycles through moveable tiles to find one
    // that is near closest enemy.
    void MoveTowardsOpponent(PlanOfAttack poa)
    {
        List<Tile> moveOptions = GetMoveOptions();
        FindNearestFoe();
        if(nearestFoe != null)
        {
            Tile toCheck = nearestFoe.m_tile;
            while(toCheck != null)
            {
                if(moveOptions.Contains(toCheck))
                {
                    poa.moveLocation = toCheck.m_pos;
                    return;
                }
                toCheck = toCheck.m_previous;
            }
        }

        poa.moveLocation = actor.m_tile.m_pos;
    }

    public Directions DetermineEndFacingDirection()
    {
        Directions dir = (Directions)UnityEngine.Random.Range(0, 4);
        FindNearestFoe();
        if(nearestFoe != null)
        {
            Directions start = actor.m_direction;
            for(int i = 0; i< 4; ++i)
            {
                actor.m_direction = (Directions)i;
                if(nearestFoe.GetFacing(actor) == Facings.Front)
                {
                    dir = actor.m_direction;
                    break;
                }
            }
        actor.m_direction = start;
        }
        return dir;
    }



    // Will by default do the first ability found.
    // In most cases this should be attack.
    void DefaultAttackPattern(PlanOfAttack poa)
    {
        poa.ability = actor.GetComponentInChildren<Ability>();
        poa.target = Targets.Foe;
    }

    // Random facing.
    //public Directions DetermineEndFacingDirection()
    //{
    //    return (Directions)UnityEngine.Random.Range(0, 4);
    //}

}