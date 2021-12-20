using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Ui for hit success rate.
// Probably don't need this version of it.

public class HitSuccessIndicator : MonoBehaviour
{
    const string ShowKey = "Show";
    const string HideKey = "Hide";

    [SerializeField] Canvas canvas;
    [SerializeField] Panel panel;
    [SerializeField] Image arrow;
    [SerializeField] Text label;
    Tweener transition;

    private void Start()
    {
        panel.SetPosition(HideKey, false);
        canvas.gameObject.SetActive(false);
    }

    public void SetState(int chance, int amount)
    {
        // Will this evaluate incorrectly without brackets?
        arrow.fillAmount = (chance / 100f);
        label.text = string.Format("{0}% {1}pt(s)", chance, amount);
    }

    public void Show()
    {
        canvas.gameObject.SetActive(true);
        SetPanelPos(ShowKey);
    }

    public void Hide()
    {
        SetPanelPos(HideKey);
        transition.completedEvent += delegate (object sender, System.EventArgs e)
        {
            canvas.gameObject.SetActive(false);
        };
    }

    void SetPanelPos(string pos)
    {
        if (transition != null && transition.IsPlaying)
            transition.Stop();

        transition = panel.SetPosition(pos, true);
        transition.duration = 0.5f;
        transition.equation = EasingEquations.EaseInOutQuad;
    }

}
