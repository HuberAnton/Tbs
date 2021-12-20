using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConversationController : MonoBehaviour
{
    // These will be the panel layours
    // Potentially I should inherit from this
    // class and then have seperate controllers 
    // for out of battle or special dialogues.
    [SerializeField] ConversationPanel leftPanel;
    [SerializeField] ConversationPanel rightPanel;

    Canvas canvas;

    IEnumerator conversation;
    Tweener transition;

    // I should go back to the ui anchors
    // and do the same as these.
    const string ShowTop = "Show Top";
    const string ShowBottom = "Show Bottom";
    const string HideTop = "Hide Top";
    const string HideBottom = "Hide Bottom";


    public static event EventHandler completeEvent;

    private void Start()
    {
        canvas = GetComponentInChildren<Canvas>();
        if (leftPanel.panel.CurrentPosition == null)
            leftPanel.panel.SetPosition(HideBottom, false);
        if (rightPanel.panel.CurrentPosition == null)
            rightPanel.panel.SetPosition(HideBottom, false);
        canvas.gameObject.SetActive(false);
    }

    public void Show(ConverstaionData data)
    {
        canvas.gameObject.SetActive(true);
        conversation = Sequence(data);
        conversation.MoveNext();
    }

    public void Next()
    {
        if (conversation == null || transition != null)
            return;

        conversation.MoveNext();
    }


    IEnumerator Sequence(ConverstaionData data)
    {

            for (int i = 0; i < data.list.Count; ++i)
            {
                SpeakerData sd = data.list[i];

                // This is a funcky way of saying which
                // panel to use depending on anchor.


                // Decideds either left or right panel.
                ConversationPanel currentPanel = (sd.anchor == TextAnchor.UpperLeft || sd.anchor == TextAnchor.MiddleLeft
                     || sd.anchor == TextAnchor.LowerLeft) ? leftPanel : rightPanel;
                // 
                IEnumerator presenter = currentPanel.Display(sd);
                presenter.MoveNext();



                string show, hide;


                // Either top or botom panel.
                if (sd.anchor == TextAnchor.UpperLeft || sd.anchor == TextAnchor.UpperCenter || sd.anchor == TextAnchor.UpperRight)
                {
                    show = ShowTop;
                    hide = HideTop;
                }
                else
                {
                    show = ShowBottom;
                    hide = HideBottom;
                }


                currentPanel.panel.SetPosition(hide, false);
                MovePanel(currentPanel, show);

                yield return null;
                while (presenter.MoveNext())
                    yield return null;

                MovePanel(currentPanel, hide);
                // I'm really not sure about this one.
                // Need to do some more look into delegates.
                // Is it just adding an action to an event in the 
                // tweener class to do once it completes?
                transition.completedEvent += delegate (object sender, EventArgs e)
                {
                    conversation.MoveNext();
                };
                yield return null;
            }

            canvas.gameObject.SetActive(false);
            if (completeEvent != null)
            {
                completeEvent(this, EventArgs.Empty);
            }
        

    }

    void MovePanel(ConversationPanel obj, string pos)
    {
        transition = obj.panel.SetPosition(pos, true);
        transition.duration = 0.5f;
        transition.equation = EasingEquations.EaseOutQuad;
    }


    
}
