using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Keeps track of when status should be removed
public class Status : MonoBehaviour
{
    public const string AddedNotification = "Status.AddedNotification";
    public const string RemovedNotification = "Status.RemovedNotification";

    // In order to add a status
    // You need both an effect (poisen, haste ect) and a condition (duration, statvalue)
    public U Add<T, U>() where T : StatusEffect where U : StatusCondition
    {
        T effect = GetComponentInChildren<T>();

        if(effect == null)
        {
            // Note this is the exteneded version.
            effect = gameObject.AddChildComponent<T>();
             
            this.PostNotification(AddedNotification, effect);
        }
        // Returns the condition that is added to this object.
        return effect.gameObject.AddChildComponent<U>();
    }

    // Remove is called by the status condition itself.
    public void Remove(StatusCondition target)
    {
        StatusEffect effect = target.GetComponentInParent<StatusEffect>();

        target.transform.SetParent(null);
        Destroy(target.gameObject);

        StatusCondition condition = effect.GetComponentInChildren<StatusCondition>();
        if(condition == null)
        {
            effect.transform.SetParent(null);
            Destroy(effect.gameObject);
            this.PostNotification(RemovedNotification, effect);
        }
    }

}
