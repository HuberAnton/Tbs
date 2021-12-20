using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    // ISSUE 
    // When you want different tiles you will need
    // to change this gameobject to an atlas.
    [SerializeField]
    GameObject m_tilePrefab;
    public Dictionary<Point, Tile> m_tiles = new Dictionary<Point, Tile>();


    // For use in tile searching.
    Point[] m_dirs = new Point[4]
    {
        new Point(0,1),
        new Point(0,-1),
        new Point(1,0),
        new Point(-1,0)
    };

    public Point min { get { return _min; } }
    public Point max { get { return _max; } }
    Point _min;
    Point _max;

    Color selectedTileColor = new Color(0, 1, 1, 1);
    Color defaultTileColor = new Color(1, 1, 1, 1);


    public void Load(LevelData m_data)
    {
        _min = new Point(int.MaxValue, int.MaxValue);
        _max = new Point(int.MinValue, int.MinValue);

        for (int i = 0; i < m_data.m_tiles.Count;i++)
        {
            GameObject instance = Instantiate(m_tilePrefab) as GameObject;
            Tile t = instance.GetComponent<Tile>();
            t.Load(m_data.m_tiles[i]);
            m_tiles.Add(t.m_pos, t);

            // For setting the max and min of the board space.
            // I wonder if this should just be done at the end of the 
            // loop?
            _min.m_x = Mathf.Min(_min.m_x, t.m_pos.m_x);
            _min.m_y = Mathf.Min(_min.m_y, t.m_pos.m_y);
            _max.m_x = Mathf.Max(_max.m_x, t.m_pos.m_x);
            _max.m_y = Mathf.Max(_max.m_y, t.m_pos.m_y);
            instance.transform.SetParent(this.gameObject.transform);
        }
    }


    // Func is a delegate? Do some more study on that.
    // It's used as the search function.
    public List<Tile> Search(Tile start, Func<Tile, Tile, bool> addTile, bool targetStart = true)
    {
        List<Tile> retValue = new List<Tile>();
        // I might need an extra argument to check
        // if you are targeting the start square
        if(targetStart)
         retValue.Add(start);

        ClearSeach();
        Queue<Tile> checkNext = new Queue<Tile>();
        Queue<Tile> checkNow = new Queue<Tile>();

        start.m_distance = 0;
        checkNow.Enqueue(start);

        while (checkNow.Count > 0)
        {
            Tile t = checkNow.Dequeue();
            for (int i = 0;i < 4; ++i)
            {
                Tile next = GetTile(t.m_pos + m_dirs[i]);

                if (next == null || next.m_distance <=  t.m_distance + 1)
                    continue;

                if(addTile(t,next))
                {
                    next.m_distance = t.m_distance + 1;
                    next.m_previous = t;
                    checkNext.Enqueue(next);
                    retValue.Add(next);
                }

                // Cheeky. Now becomes next
                if (checkNow.Count == 0)
                    SwapReference(ref checkNow, ref checkNext);
            }

        }


        return retValue;
    }

    // Duplicate to test offsets.
    public List<Tile> Search(Tile start, int offset, Func<Tile, Tile, bool> addTile, bool targetStart = true)
    {
        List<Tile> retValue = new List<Tile>();
        if(targetStart)
            retValue.Add(start);

        ClearSeach();
        Queue<Tile> checkNext = new Queue<Tile>();
        Queue<Tile> checkNow = new Queue<Tile>();

        // start at offset position -- Wouldn't work would just start the
        // search from the offset tiles. Might be useful though.
        start.m_distance = 0 /*+ offset*/;
        checkNow.Enqueue(start);

        while (checkNow.Count > 0)
        {
            Tile t = checkNow.Dequeue();
            for (int i = 0; i < 4; ++i)
            {
                Tile next = GetTile(t.m_pos + m_dirs[i]);

                if (next == null || next.m_distance <= t.m_distance + 1)
                    continue;

                if (addTile(t, next))
                {
                    next.m_distance = t.m_distance + 1;
                    next.m_previous = t;
                    checkNext.Enqueue(next);
                    // Does all the normal adjustments for correct 
                    // tiles but skips if not greater
                    // than the offset. Works for 0 case.
                    // Distance should not hit negative as +1 above.
                    if (next.m_distance > offset)
                        retValue.Add(next);
                }

                // Cheeky. Now becomes next
                if (checkNow.Count == 0)
                    SwapReference(ref checkNow, ref checkNext);
            }

        }


        return retValue;
    }

    void ClearSeach()
    {
        // This is better but just want to leave both in
        // just for my own sanity. Of course I need
        // to actuall get the tiles out of it.
        // So I need to put in the points?
        // Probably check if a point is in by cycling
        // through each x,z up to the level max bounds?
        // 
        //for(int i = 0; i < m_tiles.Count; ++i)
        //{
        //    m_tiles.[i].m_previous = null;
        //    m_tiles[i].m_distance = int.MaxValue;
        //}

        foreach (Tile t in m_tiles.Values)
        {
            t.m_previous = null;
            t.m_distance = int.MaxValue;
        }
    }

    // Checks if the point has an associated tile in the dictionary
    // and returns it, otherwise null.
    public Tile GetTile(Point p)
    {
        return m_tiles.ContainsKey(p) ? m_tiles[p] : null;
    }

    // Heh I feel this is to cut down on the above loop.
    // 
    void SwapReference( ref Queue<Tile> a, ref Queue<Tile> b)
    {
        Queue<Tile> temp = a;
        a = b;
        b = temp;
    }


    public void SelectedTiles (List<Tile> a_tiles)
    {
        for (int i = a_tiles.Count - 1; i >= 0; --i)
        {
            a_tiles[i].GetComponent<Renderer>().material.SetColor("_Color", selectedTileColor);
        }
    }

    public void DeSelectTiles (List<Tile> a_tiles)
    {
        for(int i = a_tiles.Count - 1; i >= 0; --i)
        {
            a_tiles[i].GetComponent<Renderer>().material.SetColor("_Color", defaultTileColor);
        }

    }
}
