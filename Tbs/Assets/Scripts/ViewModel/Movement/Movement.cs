using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Movement : MonoBehaviour
{
    public int m_range { get { return stats[StatTypes.MOV] * stats[StatTypes.AP]; } }
    public int m_jumpHeight { get { return stats[StatTypes.JMP]; } }
    protected Stats stats;

    protected Unit m_unit;
    protected Transform m_jumper;

    public abstract IEnumerator Traverse(Tile tile);

    protected virtual void Awake()
    {
        m_unit = GetComponent<Unit>();
        m_jumper = transform.Find("Jumper");

    }

    protected virtual void Start()
    {
        stats = GetComponent<Stats>();
    }

    public virtual List<Tile> GetTilesInRange(Board a_board)
    {
        List<Tile> retValue = a_board.Search(m_unit.m_tile, ExpandSearch);
        Filter(retValue);
        return retValue;
    }

    protected virtual bool ExpandSearch(Tile from, Tile to)
    {
        return (from.m_distance + 1) <= m_range;
    }

    protected virtual void Filter (List<Tile> a_tiles)
    {
        for (int i = a_tiles.Count - 1; i >= 0; --i)
        {
            if (a_tiles[i].m_content != null)
            {
                a_tiles.RemoveAt(i);
            }
        }
    }

    // Common method of movement.
    // Currently has causes some strange rotation
    // issues for both the cpu (not rotating when it makes its first move)
    // and player (Rotating clockwise when couterclockwise is quicker)
    protected virtual IEnumerator Turn (Directions a_dir)
    {
        TransformLocalEulerTweener t =
            (TransformLocalEulerTweener)transform.RotateToLocal(a_dir.ToEuler(), 0.25f, EasingEquations.EaseInOutQuad);

        if(Mathf.Approximately(t.startTweenValue.y, 0f) && Mathf.Approximately(t.endTweenValue.y, 270f))
        {
            t.startTweenValue = new Vector3(t.startTweenValue.x, 360f, t.startTweenValue.z);
        }
        else if(Mathf.Approximately(t.startTweenValue.y, 270) && Mathf.Approximately(t.endTweenValue.y, 0))
        {
            //t.endTweenValue = new Vector3(t.startTweenValue.x, 360f, t.startTweenValue.z);
            t.startTweenValue = new Vector3(t.startTweenValue.x, 360f, t.startTweenValue.z);
        }

        m_unit.m_direction = a_dir;


        while(t != null)
            yield return null;
    }

    // Default values.
    // Should be overritten by the different movement types.
    public void CalculateAndApplyMoveCost(Tile target)
    {
       int total = (int)Mathf.Ceil((float)target.m_distance / (float)stats[StatTypes.MOV]);
       stats[StatTypes.AP] -= total;
    }

}
