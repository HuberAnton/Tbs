using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


// For positioning of text and images of individual statPanels
public class StatPanel : MonoBehaviour
{
    public Panel panel;
    public Sprite allyBackground;
    public Sprite enemyBackground;
    public Image background;
    public Image avatar;
    public Text nameLabel;
    public Text hpLabel;
    public Text mpLabel;
    public Text lvlLabel;

    public void Display(GameObject obj)
    {
        Alliance alliance = obj.GetComponent<Alliance>();
        // Only between 2 background sprites. better to use a switch

        switch(alliance.type)
        {
            case Alliances.Hero:
            case Alliances.Neutral:
                background.sprite = allyBackground;
                break;
            case Alliances.Enemy:
            default: // None
                background.sprite = enemyBackground;
                break;
        }


        // Currently will be name in hierarchy.
        nameLabel.text = obj.name;
        Stats stats = obj.GetComponent<Stats>();

        // Sanity check,
        // Should put an else condition for error.
        if(stats)
        {
            hpLabel.text = string.Format("HP {0} / {1}", stats[StatTypes.HP], stats[StatTypes.MHP]);
            mpLabel.text = string.Format("MP {0} / {1}", stats[StatTypes.MP], stats[StatTypes.MMP]);
            lvlLabel.text = string.Format("LV. {0}", stats[StatTypes.LVL]);
        }
    }

}
