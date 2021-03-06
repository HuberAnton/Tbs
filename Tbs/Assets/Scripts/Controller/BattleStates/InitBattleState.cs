using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitBattleState : BattleState
{
    public override void Enter()
    {
        base.Enter();
        StartCoroutine(Init());
    }

    // Cooroutine to ensure other states can not occur
    // before all setup complete.
    IEnumerator Init()
    {
        // Create tiles and features on tiles.
        m_board.Load(m_levelData);
        // Set valid tile selected.
        Point p = new Point((int)m_levelData.m_tiles[0].x, (int)m_levelData.m_tiles[0].z);
        SelectTile(p);
        // Spawning units should occur in load.
        SpawnTestUnits();
        // Spawning victory condition should be in load.
        AddVictoryCondition();
        // Attach the round controller.
        m_owner.round = m_owner.gameObject.AddComponent<TurnOrderController>().CurrentTurn();
        yield return null;
        // Should check if cutscene data exists for level.
        // and load and store the data to be used in the cutscene state.
        if (true)
        {
            m_owner.ChangeState<CutSceneState>();
        }
        else
        {
            // Move to regular game mode.
            m_owner.ChangeState<SelectUnitState>();
        }
    }


    // Just keeping this here for now.
    // Spawns prior to unit factory
    //void SpawnTestUnits()
    //{
    //    // Creates examples of units
    //    // Make sure you spell your prefabs right lmao.
    //    string[] jobs = new string[] { "Rogue", /*"Warrior",*/ "Wizard" };

    //    for (int i = 0; i < jobs.Length; ++i)
    //    {
    //        // Visual component and container of all components
    //        GameObject instance = Instantiate(m_owner.m_heroPrefab) as GameObject;

    //        // Add stats component - Unset at this point except for 1 being lowest level.
    //        Stats s = instance.AddComponent<Stats>();
    //        s[StatTypes.LVL] = 1;

    //        // Load, Instantiate and parent job. Since jobs are something
    //        // to be swapped into and out of by a single unit rather than all.
    //        GameObject jobPrefab = Resources.Load<GameObject>("Jobs/" + jobs[i]);
    //        GameObject jobInstance = Instantiate(jobPrefab) as GameObject;
    //        jobInstance.transform.SetParent(instance.transform);

    //        // Now that jobs exist on the character activate the
    //        // class and apply the default stats.
    //        Job job = jobInstance.GetComponent<Job>();
    //        job.Employ();
    //        job.LoadDefaultStats();

    //        // Get a point and place a visual representation in the position.
    //        Point p = new Point((int)m_levelData.m_tiles[i].x, (int)m_levelData.m_tiles[i].z);
    //        Unit unit = instance.GetComponent<Unit>();
    //        unit.Place(m_board.GetTile(p));
    //        unit.Match();

    //        // Add movement component.
    //        instance.AddComponent<WalkMovement>();
    //        units.Add(unit);

    //        // Optional leveling up to test stats.
    //        Rank rank = instance.AddComponent<Rank>();
    //        //rank.Init(10);
    //        instance.AddComponent<Health>();
    //        instance.AddComponent<Mana>();
    //        instance.gameObject.name = jobs[i];
    //    }
    //}

    void SpawnTestUnits()
    {
        // Name of recipes go in here.
        // Level should contain these normally.
        string[] recipes = new string[]
            {
                "Knight",
                "Villan",
                "Hero Ai",
                "Villan"
            };

        // Gets all the tiles of the current loaded level.
        List<Tile> locations = new List<Tile>(m_board.m_tiles.Values);
        



        // Each unit 
        for(int i = 0; i < recipes.Length; ++i)
        {
            // Create a instance of a recipe at a random level range
            int level = UnityEngine.Random.Range(9, 12);
            GameObject instance = UnitFactory.Create(recipes[i], level);
            // Get a random tile on the map
            int random = UnityEngine.Random.Range(0, locations.Count);
            Tile randomTile = locations[random];

            // Remove from the list of tiles that is available for spawning.
            locations.RemoveAt(random);

            // Place unit on tile
            Unit unit = instance.GetComponent<Unit>();
            unit.Place(randomTile);
            // Give unit random direciton
            unit.m_direction = (Directions)(UnityEngine.Random.Range(0, 4));
            unit.Match();

            Alliances al = unit.GetComponent<Alliance>().type;

            // If a list of this alliance doesn't exist create
            // a new one. Unsure how this will work using flags.
            
            if (alliances.ContainsKey(al))
            {
                // Add the unit to the current list.
                alliances[al].Add(unit);
            }
            else
            {
                List<Unit> units = new List<Unit>();
                units.Add(unit);
                alliances.Add(al, units);
            }
            // Should order units by driver.
            units.Add(unit);

            AnimationController.AddAnimations(unit.gameObject);
        }
        // Have unit marker start at unit 0 of above loop.
        if (units.Count > 0)
            SelectTile(units[0].m_tile.m_pos);
        else
            SelectTile(new Point(0,0));

    }


    // Should be attached to leveldata
    //
    void AddVictoryCondition()
    {
        DefeatTargetVictoryCondition vc =
            m_owner.gameObject.AddComponent<DefeatTargetVictoryCondition>();
        Unit enemy = units[units.Count - 1];
        vc.target = enemy;
        Health health = enemy.GetComponent<Health>();
        health.MinHp = 10;
    }

}