using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(RectTransform))]
public class LayoutAnchor : MonoBehaviour
{
    RectTransform m_recTransform;
    RectTransform m_parentRecTransform;

    private void Awake()
    {
        m_recTransform = transform as RectTransform;
        m_parentRecTransform = transform.parent as RectTransform;
        if(m_parentRecTransform == null)
        {
            Debug.LogError("This component requires a Rect Transform parentr to work.", gameObject);
        }

    }
        Vector2 GetPosition (RectTransform a_rt, TextAnchor a_anchor)
        {
            Vector2 retValue = Vector2.zero;

            switch(a_anchor)
            {
                case TextAnchor.LowerCenter:
                case TextAnchor.MiddleCenter:
                case TextAnchor.UpperCenter:
                    retValue.x += a_rt.rect.width * 0.5f;
                    break;
                case TextAnchor.LowerRight:
                case TextAnchor.MiddleRight:
                case TextAnchor.UpperRight:
                    retValue.x += a_rt.rect.width;
                    break;
            }

            switch (a_anchor)
            {
                case TextAnchor.MiddleLeft:
                case TextAnchor.MiddleCenter:
                case TextAnchor.MiddleRight:
                    retValue.y += a_rt.rect.height * 0.5f;
                    break;
                case TextAnchor.UpperLeft:
                case TextAnchor.UpperCenter:
                case TextAnchor.UpperRight:
                    retValue.y += a_rt.rect.height;
                    break;
            }

            return retValue;
        }


     // This is complete cancer
    public Vector2 AnchorPosition(TextAnchor a_myAnchor, TextAnchor a_pareentAnchor, Vector2 a_offset)
    {
        // Get offsets
        Vector2 myOffset = GetPosition(m_recTransform, a_myAnchor);
        Vector2 parentOffset = GetPosition(m_parentRecTransform, a_pareentAnchor);

        // Center anchor point
        Vector2 anchorCenter = new Vector2(Mathf.Lerp(m_recTransform.anchorMin.x, m_recTransform.anchorMax.x, m_recTransform.pivot.x),
            Mathf.Lerp(m_recTransform.anchorMin.y, m_recTransform.anchorMax.y, m_recTransform.pivot.y));

        Vector2 myAnchorOffset = new Vector2(m_parentRecTransform.rect.width * anchorCenter.x,
            m_parentRecTransform.rect.height * anchorCenter.y);

        Vector2 myPivotOffset = new Vector2(m_recTransform.rect.width * m_recTransform.pivot.x,
            m_recTransform.rect.height * m_recTransform.pivot.y);

        Vector2 pos = parentOffset - myAnchorOffset - myOffset + myPivotOffset + a_offset;
        pos.x = Mathf.RoundToInt(pos.x);
        pos.y = Mathf.RoundToInt(pos.y);

        return pos;
    }

    public void SnapToAnchorPosition(TextAnchor myAnchor, TextAnchor parentAnchor, Vector2 offset)
    {
        m_recTransform.anchoredPosition = AnchorPosition(myAnchor, parentAnchor, offset);
    }


    public Tweener MoveToAnchorPosition(TextAnchor a_myAnchor, TextAnchor a_parentAnchor, Vector2 a_offset)
    {
        return m_recTransform.AnchorTo(AnchorPosition(a_myAnchor, a_parentAnchor, a_offset));
    }

}
