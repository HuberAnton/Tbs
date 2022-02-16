using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System;

// For reference to automate.
// http://theliquidfire.com/2014/12/25/bestiary-management-and-scriptable-objects/
// Revist this big time to automate the 
// process when you are happy with the
// underlying systems.
// Jobs, Races, Potentials will all need parsers
// and when a character is created it will have 
// 1 of each component to define it's overall stats
// and growth.
// Note that stats are directly tied to job stats.
// Would be better if the current job class impacted
// some of the growth stats which would then be applied
// to the character along with potentials.


// Big note though.
// A character will have a job, with it's modifiers and stats
// a race with it's modifiers and stats and a potential with
// it's modifiers and stats. Crazy.
public static class JobParser
{
    // Values from the spreadsheet.
    // Hardcoded somewhat.
    // Will need to find where it is in 
    // the spreadsheet to solve this.
    static readonly int EvadeValue = 8;
    static readonly int ResistanceValue = 9;
    static readonly int MoveValue = 10;
    static readonly int JumpValue = 11;
    static readonly int ApValue = 12;
    static readonly int ApMaxValue = 13;

    // This would be what should occur when the 
    // engine detects a change to the file.
    // As in have a function that runs in editor
    // which calls this.
    [MenuItem("Pre Production/Parse Jobs")]
    public static void Parse()
    {
        CreateDirectories();
        ParseStartingStats();
        ParseGrowthStats();
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }

    // Make save location for asset
    static void CreateDirectories()
    {
        if (!AssetDatabase.IsValidFolder("Assets/Resources/Jobs"))
            AssetDatabase.CreateFolder("Assets/Resources", "Jobs");
    }

    // Find file and copy all lines strings to an array.
    static void ParseStartingStats()
    {
        string readPath = string.Format("{0}/Settings/JobStartingStats.csv", Application.dataPath);
        string[] readText = File.ReadAllLines(readPath);
        // Start from 1 to skip the names of each collumn
        for(int i = 1; i < readText.Length; ++i)
        {
            // Pass in single line
            ParseStartingStats(readText[i]);
        }
    }

    // Cycles through each class.
    static void ParseStartingStats(string line)
    {
        // New entry into elemnts after every ',' to divide data.
        // Refers to data conatained in the spreadsheet.
        string[] elements = line.Split(',');
        // Finds the asset of the job name or creates
        // it if it doesn't exist yet.
        GameObject obj = GetOrCreate(elements[0]);
        Job job = obj.GetComponent<Job>();
        for(int i = 1; i < Job.statOrder.Length + 1; ++i)
        {
            // Save over the jobs base stats with new values.
            job.baseStats[i - 1] = Convert.ToInt32(elements[i]);
        }

        
        // These values are directly related to the 
        // job. Leveling will not change anything.
        StatModifierFeature move = GetFeature(obj, StatTypes.MOV);
        move.amount = Convert.ToInt32(elements[MoveValue]);

        StatModifierFeature jump = GetFeature(obj, StatTypes.JMP);
        jump.amount = Convert.ToInt32(elements[JumpValue]);

        StatModifierFeature res = GetFeature(obj, StatTypes.RES);
        res.amount = Convert.ToInt32(elements[ResistanceValue]);

        StatModifierFeature evd = GetFeature(obj, StatTypes.EVD);
        evd.amount = Convert.ToInt32(elements[EvadeValue]);

        StatModifierFeature ap = GetFeature(obj, StatTypes.AP);
        ap.amount = Convert.ToInt32(elements[ApValue]);

        StatModifierFeature apmax = GetFeature(obj, StatTypes.APMAX);
        apmax.amount = Convert.ToInt32(elements[ApMaxValue]);
    }

    // Same as Starting stats except using growth spreadsheet instead.
    // Should fix this.
    static void ParseGrowthStats()
    {
        string readPath = string.Format("{0}/Settings/JobGrowthStats.csv",
            Application.dataPath);
        string[] readText = File.ReadAllLines(readPath);
        for(int i = 1; i < readText.Length; ++i)
        {
            ParseGrowthStats(readText[i]);
        }
    }

    static void ParseGrowthStats(string line)
    {
        string[] elements = line.Split(',');
        GameObject obj = GetOrCreate(elements[0]);
        Job job = obj.GetComponent<Job>();

        for(int i = 1; i < elements.Length; ++i)
        {
            // Since these are floats slightly different convert.
            job.growStats[i - 1] = Convert.ToSingle(elements[i]);        
        }
        // Note no jump or move as features of the class.
    }

    static StatModifierFeature GetFeature(GameObject obj, StatTypes type)
    {
        // Get the smf attached to the job.
        StatModifierFeature[] smf = obj.GetComponents<StatModifierFeature>();
        for(int i = 0; i < smf.Length; ++i)
        {
            // Loop through and return the feature assoiciated with the stat.
            if (smf[i].type == type)
                return smf[i];
        }
        // If there is no smf attached to the job of that type create one.
        StatModifierFeature feature = obj.AddComponent<StatModifierFeature>();

        feature.type = type;
        return feature;
    }

    static GameObject GetOrCreate(string jobName)
    {
        string fullpath = string.Format("Assets/Resources/Jobs/{0}.prefab", jobName);
        // Load prefab as a game object as the path location.
        GameObject obj = AssetDatabase.LoadAssetAtPath<GameObject>(fullpath);
        // If it doesn't exist create an 
        if (obj == null)
            obj = Create(fullpath);

        return obj;
    }

    // Creates a Job prefab
    static GameObject Create(string fullPath)
    {
        // Create a new object with the job component.
        GameObject instance = new GameObject("temp");
        instance.AddComponent<Job>();
        // Create a prefab of the object at the location passed in
        GameObject prefab = PrefabUtility.SaveAsPrefabAsset(instance, fullPath);
        // Destroy the version of it in the world.
        GameObject.DestroyImmediate(instance);
        return prefab;
    }

}
