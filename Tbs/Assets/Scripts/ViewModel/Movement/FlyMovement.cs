using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyMovement : Movement
{
    public override IEnumerator Traverse(Tile tile)
    {

        float dist = Mathf.Sqrt(Mathf.Pow(tile.m_pos.m_x - m_unit.m_tile.m_pos.m_x, 2) +
            Mathf.Pow(tile.m_pos.m_y - m_unit.m_tile.m_pos.m_y, 2));

        m_unit.Place(tile);

        float y = Tile.m_stepHeight * 10;

        float duration = (y - m_jumper.position.y) * 0.5f;
        Tweener tweener = m_jumper.MoveToLocal(new Vector3(0, y, 0), duration,
            EasingEquations.EaseInOutQuad);
        while (tweener != null)
            yield return null;

        Directions dir;
        Vector3 toTile = (tile.m_center - transform.position);
        if (Mathf.Abs(toTile.x) > Mathf.Abs(toTile.z))
            dir = toTile.x > 0 ? Directions.East : Directions.West;
        else
            dir = toTile.z > 0 ? Directions.North : Directions.South;
        yield return StartCoroutine(Turn(dir));


        duration = dist * 0.5f;
        tweener = transform.MoveTo(tile.m_center, duration,
            EasingEquations.EaseInOutQuad);
        while (tweener != null)
            yield return null;

        duration = (y - tile.m_center.y) * 0.5f;
        tweener = m_jumper.MoveToLocal(Vector3.zero, 0.5f, EasingEquations.EaseInOutQuad);
        while (tweener != null)
            yield return null;

    }
}
