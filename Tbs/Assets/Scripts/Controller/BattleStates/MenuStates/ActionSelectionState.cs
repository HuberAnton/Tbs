using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionSelectionState : BaseAbilityMenuState
{
    protected AbilityCatalog catalog;
    public static int category;



    // Shows and hides stat panels
    public override void Enter()
    {
        base.Enter();
        statPanelContoller.ShowPrimary(turn.actor.gameObject);
    }

    public override void Exit()
    {
        base.Exit();
        statPanelContoller.HidePrimary();
    }

    // Get the correct ability catalog
    // and display current abilites.
    protected override void LoadMenu()
    {
        catalog = turn.actor.GetComponentInChildren<AbilityCatalog>();
        GameObject containter = catalog.GetCategory(category);
        menuTitle = containter.name;

        int count = catalog.AbilityCount(containter);
        if (menuOptions == null)
            menuOptions = new List<string>(count);
        else
            menuOptions.Clear();

        bool[] locks = new bool[count];
        for(int i = 0; i < count; ++i)
        {
            Ability ability = catalog.GetAbility(category, i);
            AbilityMPCost cost = ability.GetComponent<AbilityMPCost>();
            if (cost)
                menuOptions.Add(string.Format("{0}: {1}", ability.name, cost.amount));
            else
                menuOptions.Add(ability.name);
            locks[i] = !ability.CanPerform();
        }
        abilityMenuPanelController.Show(menuTitle, menuOptions);
        for (int i = 0; i < count; ++i)
            abilityMenuPanelController.SetLocked(i, locks[i]);

    }


    protected override void Confirm()
    {
        turn.ability = catalog.GetAbility(category, abilityMenuPanelController.selection);
        m_owner.ChangeState<AbilityTargetState>();
    }

    protected override void Cancel()
    {
        m_owner.ChangeState<CategorySelectionState>();
    }

    void SetOptions(string[] options)
    {
        menuOptions.Clear();
        for (int i = 0; i < options.Length; ++i)
            menuOptions.Add(options[i]);
    }

}
