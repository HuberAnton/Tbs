using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Equipment : MonoBehaviour
{
    // Short hand for notification center calls.
    public const string EquippedNotification = "Equiment.EquippedNotification";
    public const string UnEquippedNotification = "Equipment.UnEquippedNotifiction";

    // Interface list? Why is this?
    public IList<Equippable> items { get { return _items.AsReadOnly(); } }
    [SerializeField]
    List<Equippable> _items = new List<Equippable>();


    public void Equip(Equippable item, EquipSlots slots)
    {
        UnEquip(slots);

        _items.Add(item);
        // The item will follow the parent.
        // Take note as this may cause issues
        // with art.*************ISSUE
        item.transform.SetParent(transform);
        item.slots = slots;
        item.OnEquip();

        this.PostNotification(EquippedNotification, item);
    }

    public void UnEquip(Equippable item)
    {
        item.OnUnEquip();
        item.slots = EquipSlots.None;
        item.transform.SetParent(transform);
        _items.Remove(item);

        this.PostNotification(UnEquippedNotification, item);
    }

    public void UnEquip(EquipSlots slots)
    {
        for(int i =_items.Count - 1; i >= 0; --i)
        {
            Equippable item = _items[i];
            if ((item.slots & slots) != EquipSlots.None)
                UnEquip(item);
        }
    }


    public Equippable GetItem(EquipSlots slots)
    {
        for (int i = _items.Count - 1; i >= 0; --i)
        {
            Equippable item = _items[i];
            if ((item.slots & slots) != EquipSlots.None)
                return item;
        }
        return null;
    }

}
