using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CategorySelectionState : BaseAbilityMenuState
{
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

    protected override void LoadMenu()
    {
        // Probably should load these acitons in 
        // based on the unit.
        if (menuOptions == null)
        {
            menuOptions = new List<string>();
        }
        else
            menuOptions.Clear();

        menuTitle = "Action";
        menuOptions.Add("Attack");

        AbilityCatalog catalog = turn.actor.GetComponentInChildren<AbilityCatalog>();
        bool[] locks = new bool[catalog.transform.childCount];
        for(int i = 0; i < catalog.CategoryCount();++i)
        {
            // Name of the gameobject is name of the category.
            if (catalog.GetCategory(i).transform.childCount > 0)
            {
                menuOptions.Add(catalog.GetCategory(i).name);
                locks[i] = true;
            }
                // Skip if no abilities under taht category.
        }
        abilityMenuPanelController.Show(menuTitle, menuOptions);
        
    }


    protected override void Confirm()
    {
        if (abilityMenuPanelController.selection == 0)
            Attack();
        else
            // -1 because attack is not a category.
            SetCategory(abilityMenuPanelController.selection - 1);
    }

    // Maybe it's worth remembering what previous state was?
    // If that exists go back to that otherwise go into
    // explore state?
    // Would be on the base class.
    protected override void Cancel()
    {
        m_owner.ChangeState<CommandSelectionState>();
    }

    void Attack()
    {
        // Attack is not in an ability category
        // So gets the top most ability which is attack.
        turn.ability = turn.actor.GetComponentInChildren<Ability>();
        m_owner.ChangeState<AbilityTargetState>();
    }

    void SetCategory(int index)
    {
        ActionSelectionState.category = index;
        m_owner.ChangeState<ActionSelectionState>();
    }

}
