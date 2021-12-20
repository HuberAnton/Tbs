using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Each child needs to be a category(empty object)
// then the abilitys underneath.
public class AbilityCatalog : MonoBehaviour
{
    public int CategoryCount()
    {
        return transform.childCount;
    }

    public GameObject GetCategory(int index)
    {
        if (index < 0 || index >= transform.childCount)
            return null;
        return transform.GetChild(index).gameObject;
    }

    // To be called by ui?
    // To work out how many entries.
    public int AbilityCount(GameObject category)
    {
        return category != null ? category.transform.childCount : 0;
    }

    public Ability GetAbility(int categoryIndex, int abilityIndex)
    {
        GameObject category = GetCategory(categoryIndex);
        // If no categories, categories with children or outside of index range. 
        if (category == null || abilityIndex < 0 || abilityIndex >= category.transform.childCount)
            return null;
        return category.transform.GetChild(abilityIndex).GetComponent<Ability>();
    }
}
