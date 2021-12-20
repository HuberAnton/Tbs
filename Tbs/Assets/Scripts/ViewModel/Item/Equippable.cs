using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Is to be attached to an item prefab.
public class Equippable : MonoBehaviour
{
    public EquipSlots defaultSlots;
    public EquipSlots secondarySlots;
    public EquipSlots slots;
    bool _isEquipped;

    public void OnEquip()
    {
        // If an item is equipped in the slot skip?
        // Should swap them but lets not worry about that
        // yet
        if (_isEquipped)
            return;

        _isEquipped = true;

        //
        Feature[] features = GetComponentsInChildren<Feature>();
        for(int i = 0; i < features.Length; ++i)
        {
            features[i].Activate(gameObject);
        }
    }

    // Funky name.
    // Suppose it keeps to the standard.
    // Woundn't make much sense being offequip.
    public void OnUnEquip()
    {
        if (!_isEquipped)
            return;

        _isEquipped = false;

        Feature[] features = GetComponentsInChildren<Feature>();
        for(int i = 0; i < features.Length; ++i)
        {
            features[i].Deactivate();
        }
    }

}
