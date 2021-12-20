using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif


// A recipe for ability categories and the attached abilities
[CreateAssetMenu(fileName = " ability_category_default", menuName = "Ability/New Ability Category", order = 0)]
public class AbilityCatalogRecipe : ScriptableObject
{
    [System.Serializable]
    public class Category
    {
        [Header("Name of folder in Resources/Ability . Eg: White magic.")]
        public string name;
        [Header("Ability name. Eg: Cure, Raise")]
        public string[] entries;
    }
    public Category[] categories;
}
