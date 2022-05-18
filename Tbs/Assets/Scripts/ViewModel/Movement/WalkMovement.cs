using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkMovement : Movement
{
    protected override bool ExpandSearch(Tile from, Tile to)
    {
        // Jumping check, If greater than height wont work.
        // just looking at this it should allow for drops.
        if ((Mathf.Abs(from.m_height - to.m_height) > m_jumpHeight))
            return false;

        // If something on tile skip...
        // Might need to check for flags of conent
        // here.
        if (to.m_content != null)
            return false;

        return base.ExpandSearch(from, to);
    }

    // Ai currently has an issue where the marker does not
    // sit at the movement destination resulting in strange 
    // rotations and movements.
    // Player can not move into position the enemy has previously
    // moved towards. The traversal is not the problem.
    public override IEnumerator Traverse(Tile tile)
    {
        // Places the unit at the position
        // of the first movement tile.
        m_unit.Place(tile);

        List<Tile> targets = new List<Tile>();
        // Loop checks to see if current tile is null if not it adds it to the
        // list of 'targets' which are the tiles to move to and then changes the
        // current tile to the previous tile set by the movement 
        while (tile != null)
        {

            targets.Insert(0, tile);
            // previous is set in the 
            tile = tile.m_previous;
        }

        for (int i = 1; i < targets.Count; ++i)
        {

            Tile from = targets[i - 1];
            Tile to = targets[i];

            Directions dir = from.GetDirection(to);

            // This is basically the actions that need to 
            // take place for the unit to reach the goal.

            // If not facing tile rotate
            if (m_unit.m_direction != dir)
                yield return StartCoroutine(Turn(dir));

            // If height the same walk to tile
            if (from.m_height == to.m_height)
                yield return StartCoroutine(Walk(to));
            else
                yield return StartCoroutine(Jump(to));

        }

        AnimationController.Play(m_unit, "Idle");
        yield return null;
    }

    // The actual methods of movement.
    // If you want to modify the movemnt it happens here.
    // Need to look into the tweenings class more.
    IEnumerator Walk(Tile a_target)
    {
        AnimationController.Play(m_unit, "Walk");
        Tweener tweener = transform.MoveTo(a_target.m_center, 0.5f, EasingEquations.Linear);
        while (tweener != null)
            yield return null;
    }

    IEnumerator Jump(Tile a_to)
    {
        AnimationController.Play(m_unit, "Jump");
        Tweener tweener = transform.MoveTo(a_to.m_center, 0.5f, EasingEquations.Linear);

        Tweener t2 = m_jumper.MoveToLocal(new Vector3(0, Tile.m_stepHeight * 2f, 0),
            tweener.duration / 2f, EasingEquations.EaseOutQuad);
        t2.loopCount = 1;
        t2.loopType = EasingControl.LoopType.PingPong;

        while (tweener != null)
            yield return null;

    }


}
