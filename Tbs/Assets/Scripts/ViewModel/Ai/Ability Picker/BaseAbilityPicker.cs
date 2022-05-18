using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseAbilityPicker : MonoBehaviour
{
    protected Unit owner;
    protected AbilityCatalog ac;

    private void Start()
    {
        owner = GetComponentInParent<Unit>();
        ac = owner.GetComponentInChildren<AbilityCatalog>();
    }

    public abstract void Pick(PlanOfAttack plan);

    protected Ability Find(string abilityName)
    {
        for(int i = 0; i < ac.transform.childCount; ++i)
        {
            Transform category = ac.transform.GetChild(i);

            // Old version
            //Transform child = category.Find(abilityName);
            //Transform child;
            for (int j = 0; j < category.childCount; ++j)
            {
                // String compare bad. Maybe just check using ints.
                // Eg: Category x, Ability y 
                // if x > category.count = category 0 ability 0 with error message;
                // if y > ability.count = category x ability 0 with error message.
                if(category.GetChild(j).GetComponent<Ability>().abilityName == abilityName)
                {
                    return category.GetChild(j).GetComponent<Ability>();
                }
            }
            //if (child != null)
            //{
            //    return child.GetComponent<Ability>();
            //}
        }

        return null;
    }

    protected Ability Default()
    {
        return owner.GetComponentInChildren<Ability>();
    }

}
