using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Fpr positioning and movement of stat panels.
public class StatPanelContoller : MonoBehaviour
{
    // Shorthand
    const string ShowKey = "Show";
    const string HideKey = "Hide";

    [SerializeField]
    StatPanel primaryPanel;
    [SerializeField]
    StatPanel secondaryPanel;

    Tweener primaryTransition;
    Tweener secondaryTransition;


    // Auto hide stat panels on start.
    private void Start()
    {
        if (primaryPanel.panel.CurrentPosition == null)
            primaryPanel.panel.SetPosition(HideKey, false);
        if (secondaryPanel.panel.CurrentPosition == null)
            secondaryPanel.panel.SetPosition(HideKey, false);
    }


    // Show and hide functions for the state controller.
    public void ShowPrimary(GameObject obj)
    {
        primaryPanel.Display(obj);
        // I suppose it's passed as a ref as it's a coroutine?
        // Mainly done to decrease code duplication.
        MovePanel(primaryPanel, ShowKey, ref primaryTransition);
    }

    public void HidePrimary()
    {
        MovePanel(primaryPanel, HideKey, ref primaryTransition);
    }

    public void ShowSecondary(GameObject obj)
    {
        secondaryPanel.Display(obj);

        MovePanel(secondaryPanel, ShowKey, ref secondaryTransition);
    }

    public void HideSecondary()
    {
        MovePanel(secondaryPanel, HideKey, ref secondaryTransition);
    }

    void MovePanel(StatPanel obj, string pos, ref Tweener t)
    {
        // Returns a position from the Panel class dictionary.
        // So it would be pre defined.
        Panel.Position target = obj.panel[pos];
        // If it hasn't reached target when this is called.
        // Deals with duplicate calls to same position.
        if(obj.panel.CurrentPosition != target)
        {
            // 
            if (t != null && t != null)
                t.Stop();
            // Makes the tweener move towards the position.
            // Pass false if snap wanted.
            t = obj.panel.SetPosition(pos, true);
            // Time to destination
            t.duration = 0.5f;
            // Tween 'shape'.
            t.equation = EasingEquations.EaseOutQuad;
        }
    }

}
