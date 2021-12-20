using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TileCoordinateController : MonoBehaviour
{
    const string ShowKey = "Show";
    const string HideKey = "Hide";



    [SerializeField] Text xNumber;
    [SerializeField] Text zNumber;
    [SerializeField] Text heightNumber;
    [SerializeField] Panel panel;
    [SerializeField] GameObject canvas;
    Tweener transition;



    private void Start()
    {
        panel.SetPosition(HideKey, false);
        canvas.SetActive(false);
        xNumber.text = "XX";
        zNumber.text = "ZZ";
        heightNumber.text = "YY";
    }

    // To be added 
    // Current point passed in
    public void UpdateCoordinates(Tile tile)
    {
        xNumber.text = tile.m_pos.m_x.ToString();
        zNumber.text = tile.m_pos.m_y.ToString();
        heightNumber.text = tile.m_height.ToString();
    }

    // Shown during battle.
    // Except during cutscene movement
    public void Show(Tile tile)
    {
        // Move left
        UpdateCoordinates(tile);
        canvas.SetActive(true);
        SetPanelPos(ShowKey);
    }

    public void Hide()
    {
        // Move right, offscreen
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
        transition.duration = 0.2f;
        transition.equation = EasingEquations.EaseInOutQuad;
    }
}
