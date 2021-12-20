using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameObjectExtensions
{
    // This ensures that when an object is being destroyed,
    // a status for example
    // and a new 1 needs to be added it makes sure to create
    // one instead of attaching itself to the 1 that is about to be
    // destroyed.
    // I think.
    public static T AddChildComponent<T> (this GameObject obj) where T : MonoBehaviour
    {
        GameObject child = new GameObject(typeof(T).Name);
        child.transform.SetParent(obj.transform);
        return child.AddComponent<T>();
    }
}
