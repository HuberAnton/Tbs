using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

// A scriptable object to store units for creation.
[CreateAssetMenu(fileName = " Unit_default", menuName = "Unit/New Unit Recipe", order = 0)]
public class UnitRecipe : ScriptableObject
{
    [Header("Eg:Model/(modelName) or Ability/(folderName/abilityName)")]
    [Header("Will look for asset at string location.")]
    public string model;    
    public string job;      // All stats and growths come from here.
    public string attack;   // Basic ability attack. May not need.
    public string abilityCatalog;   // Abilies. Not tied to equipment, jobs or levels.
    public string strategy;         // Human or ai controlled with a strategy.
    public Locomotions locomotion;  // How unit move.
    public Alliances alliances;     // Which team unit belongs to.
}
