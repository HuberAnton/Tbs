using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Base class to be used with items?
// Could this also be used with terrain?
public abstract class Feature : MonoBehaviour
{
    protected GameObject _target { get; private set; }

    // I suppose this will be used with Deactivate
    // to reenable.
    // A temporary feature on the unit.
    // Might be a weapon, might be stat modifier
    // might be an ability.
    public void Activate(GameObject target)
    {
        if(_target == null)
        {
            _target = target;
            OnApply();
        }
    }

    // General use case?
    // Unequip? Status effect? Move off tile?
    public void Deactivate()
    {
        if(_target != null)
        {
            OnRemove();
            _target = null;
        }
    }

    // First time application? 
    // Wait this would be a permanent feature.
    // Maybe used with level ups adding stats or abilites.
    public void Apply(GameObject target)
    {
        _target = target;
        OnApply();
        _target = null;
    }

    protected abstract void OnApply();
    protected virtual void OnRemove() { }
}
