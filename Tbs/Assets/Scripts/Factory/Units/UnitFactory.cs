using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class UnitFactory
{
    public static GameObject Create(string name, int level)
    {
        UnitRecipe recipe = Resources.Load<UnitRecipe>("Unit Recipies/" + name);
        if(recipe == null)
        {
            Debug.LogError("No Unit Recipe for name: " + name);
            return null;
        }
        
        return Create(recipe, level);
    }

    
    public static GameObject Create(UnitRecipe recipe, int level)
    {
        GameObject obj = InstantiatePrefab("Unit Models/" + recipe.model);
        obj.name = recipe.name;
        obj.AddComponent<Unit>();
        AddStats(obj);
        AddLocomotion(obj, recipe.locomotion);
        obj.AddComponent<Status>();
        obj.AddComponent<Equipment>();
        AddJob(obj, recipe.job);
        AddRank(obj, level);
        obj.AddComponent<Health>();
        obj.AddComponent<Mana>();


        
        // Should replace this with add weapon
        AddAttack(obj, recipe.attack);

        AddAbilityCatalog(obj, recipe.abilityCatalog);

        AddAlliance(obj, recipe.alliances);

        AddAttackPattern(obj, recipe.strategy);
        // Created unit.
        return obj;
    }


    // Prefabs need to be located in resources.
    static GameObject InstantiatePrefab(string name)
    {
        GameObject prefab = Resources.Load<GameObject>(name);
        if(prefab == null)
        {
            Debug.LogError("No Prefab for name: " + name);
            // Returns a model
            return new GameObject(name);
        }

        GameObject instance = GameObject.Instantiate(prefab);
        return instance;
    }


    #region Component Adders
    static void AddStats(GameObject obj)
    {
        Stats s = obj.AddComponent<Stats>();
        s.SetValue(StatTypes.LVL, 1, false);
    }

    static void AddJob(GameObject obj, string name)
    {
        GameObject instance = InstantiatePrefab("Jobs/" + name);
        instance.transform.SetParent(obj.transform);
        Job job = instance.GetComponent<Job>();
        job.Employ();
        job.LoadDefaultStats();
    }

    static void AddLocomotion(GameObject obj, Locomotions type)
    {
        switch (type)
        {
            case Locomotions.Walk:
                obj.AddComponent<WalkMovement>();
                break;
            case Locomotions.Fly:
                obj.AddComponent<FlyMovement>();
                break;
            case Locomotions.Teleport:
                obj.AddComponent<TeleportMovement>();
                break;
        }
    }

    static void AddAlliance(GameObject obj, Alliances type)
    {
        Alliance alliance = obj.AddComponent<Alliance>();
        alliance.type = type;
    }

    static void AddRank(GameObject obj, int level)
    {
        Rank rank = obj.AddComponent<Rank>();
        rank.Init(level);
    }

    static void AddAttack(GameObject obj, string name)
    {
        GameObject instance = InstantiatePrefab("Abilities/" + name);
        instance.transform.SetParent(obj.transform);
    }

    static void AddAbilityCatalog(GameObject obj, string name)
    {
        GameObject main = new GameObject("Ability Catalog");
        main.transform.SetParent(obj.transform);
        main.AddComponent<AbilityCatalog>();

        AbilityCatalogRecipe recipe =
            Resources.Load<AbilityCatalogRecipe>("Ability Catalog Recipes/" + name);

        if(recipe == null)
        {
            Debug.LogError("No Ability Catalog Recipe Found: " + name);
            return;
        }

        for(int i = 0; i < recipe.categories.Length; ++i)
        {
            GameObject category = new GameObject(recipe.categories[i].name);
            category.transform.SetParent(main.transform);

            for(int j = 0; j < recipe.categories[i].entries.Length; ++j)
            {
                string abilityName = string.Format("Abilities/{0}/{1}",
                    recipe.categories[i].name, recipe.categories[i].entries[j]);
                GameObject ability = InstantiatePrefab(abilityName);
                ability.name = recipe.categories[i].entries[j];
                ability.transform.SetParent(category.transform);
            }
        }
    }

    static void AddAttackPattern(GameObject obj, string name)
    {
        Driver driver = obj.AddComponent<Driver>();
        if(string.IsNullOrEmpty(name))
        {
            driver.normal = Drivers.Human;
        }
        else
        {
            driver.normal = Drivers.Computer;
            GameObject instance = InstantiatePrefab("Attack Pattern/" + name);
            instance.transform.SetParent(obj.transform);
        }
    }

    #endregion
}