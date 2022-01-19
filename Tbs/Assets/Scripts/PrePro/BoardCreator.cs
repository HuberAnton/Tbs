using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif


public class BoardCreator : MonoBehaviour
{
    [SerializeField]
    private GameObject m_tileViewPreFab;
    [SerializeField]
    private GameObject m_tileSelectionIndicatorPrefab;
    [SerializeField]
    private GameObject m_emptyTile;
    [SerializeField]
    private GameObject m_tileset;
    [SerializeField]
    private int selectedTile;


    // This is a "Lazy Loading" design pattern.
    // So the property checks if the objects exits
    // which both marker and _marker are the same.
    // If it doesn't it creates the objects.
    // So it always exists when needed. Kinda nifty.
    Transform marker
    {
        get
        {
            if(_marker == null)
            {
                // Not fond of this instantiating style. Shouldn't matter.
                GameObject instance = Instantiate(m_tileSelectionIndicatorPrefab) as GameObject;
                _marker = instance.transform;
                // The BoardCreator will need to be at 0,0,0 otherwise marker is incorrect.
                // Offest the markers 

            }
            return _marker;
        }
    }
    Transform _marker;

    // This is exactley what I needed in my 1st prototype.
    // Combines both the model and view together since you
    // always have one causing an effect on the other.
    public Dictionary<Point, Tile> m_tiles = new Dictionary<Point, Tile>();


    // Bounds for grid gen.
    // Probably should exist on the levels?
    [SerializeField]
    private int m_width = 10;
    [SerializeField]
    private int m_depth = 10;
    [SerializeField]
    private int m_height = 8;

    // For making adjustments to the board at this position.
    [SerializeField]
    private Point m_pos;

    [SerializeField]
    private LevelData m_levelData;


    // Raises tiles based on a random rectange using the maximums above by 1 step.
    // If nothing on space creates a new tile instead
    public void GrowArea()
    {
        Rect r = RandomRect();
        GrowRect(r);
    }
    // Lowers tiles based on a random rectange using the maximums above by 1 step.
    // Removes those of 0 height.
    public void ShrinkArea()
    {
        Rect r = RandomRect();
        ShrinkRect(r);
    }

    //Generates a random recangle struct to be used for random generation.
    Rect RandomRect()
    {
        int x = UnityEngine.Random.Range(0, m_width);
        int y = UnityEngine.Random.Range(0, m_depth);
        int w = UnityEngine.Random.Range(1, m_width - x + 1);
        int h = UnityEngine.Random.Range(1, m_depth - y + 1);
        return new Rect(x, y, w, h);
    }


    // Cycles through rectangle to raise or create new tiles.
    void GrowRect(Rect a_rect)
    {
        for (int y = (int)a_rect.yMin; y < (int)a_rect.yMax; ++y)
        {
            for(int x = (int)a_rect.xMin; x < (int)a_rect.xMax; ++x)
            {
                Point p = new Point(x, y);
                GrowSingle(p);
            }
        }
    }

    // Same as above but shrinks 
    void ShrinkRect(Rect a_rect)
    {
        for(int y = (int)a_rect.yMin; y < (int)a_rect.yMax; ++y)
        {
            for ( int x = (int)a_rect.xMin; x < (int)a_rect.xMax;++x)
            {
                Point p = new Point(x, y);
                ShrinkSingle(p);
            }
        }
    }

    // Creates a new tile of the type stored in tileViewPreFab.
    // Will need to create a different tile here if you want different
    // terrain. Eg: Pass in an int and creat that number from an additional 
    // tile atlas.
    Tile Create()
    {
        GameObject instance = Instantiate(m_tileViewPreFab) as GameObject;
        instance.transform.parent = transform;
        return instance.GetComponent<Tile>();
    }

    // Decides if you need a new tile or to raise tile.
    // All tiles raised will be the same as the one below.
    Tile GetOrCreate(Point p)
    {
        if(m_tiles.ContainsKey(p))
        {
            return m_tiles[p];
        }

        Tile t = Create();
        t.Load(p, 0);
        m_tiles.Add(p, t);

        return t;
    }


    // Modiies the scale value of tile by the
    // tiles step height.
    void GrowSingle(Point p)
    {
        Tile t = GetOrCreate(p);
        if(t.m_height < m_height)
        {
            t.Grow();
        }
    }

    // Removes top tile or nothing if tile does not 
    // exist.
    void ShrinkSingle(Point p)
    {
        if(!m_tiles.ContainsKey(p))
        {
            return;
        }

        Tile t = m_tiles[p];
        t.Shrink();

        if(t.m_height <= 0)
        {
            m_tiles.Remove(p);
            DestroyImmediate(t.gameObject);
        }
    }

    // Used with editor.
    // Raises the tile at the co-ordinates m_x and m_y 
    public void Grow()
    {
        GrowSingle(m_pos);
    }

    // Used with editor.
    // Shrinks the tile at the co-ordinates m_x and m_y 
    public void Shrink()
    {
        ShrinkSingle(m_pos);
    }


    // Moves the m_selectedTilePrefab into the place of the last single modified tile.
    // Potential problem****************
    // This might need an update if using irregularily sized markers.
    // Or having markers that need to extend multiple tiles.
    public void UpdateMarker()
    {
        Tile t = m_tiles.ContainsKey(m_pos) ? m_tiles[m_pos] : null;
        marker.localPosition = t != null ? t.m_center : new Vector3(m_pos.m_x, 0, m_pos.m_y);
        // offset for the markers position and rotation since tied to the board creator.
        marker.localPosition += this.transform.position;
        // Need some information about the tile here.
        //marker.localRotation = this.transform.rotation;
    }

    // Clears the level. Save if wanting to keep current level.
    public void Clear()
    {
        for (int i = transform.childCount - 1; i >= 0; --i)
            DestroyImmediate(transform.GetChild(i).gameObject);
        m_tiles.Clear();
    }


    // Save the created level into the provided path
    // as a LevelData scriptable object.

#if UNITY_EDITOR
    public void Save()
    {
        string filePath = Application.dataPath + "/Resources/Levels";
        if(!Directory.Exists(filePath))
        {
            CreateSaveDirectory();
        }

        LevelData board = ScriptableObject.CreateInstance<LevelData>();
        board.m_tiles = new List<Vector3>(m_tiles.Count);
        board.m_tileData = new List<TileData>(m_tiles.Count);
        foreach(Tile t in m_tiles.Values)
        {
            board.m_tiles.Add(new Vector3(t.m_pos.m_x, t.m_height, t.m_pos.m_y));
            board.m_tileData.Add(new TileData(new Vector3(t.m_pos.m_x, t.m_height, t.m_pos.m_y), 0, 0, null));
        }
        // Why both argurments into the string format by only 1 used?
        string filesName = string.Format("Assets/Resources/Levels/{1}.asset", filePath, name);
        AssetDatabase.CreateAsset(board, filesName);
    }

    void CreateSaveDirectory()
    {
        string filePath = Application.dataPath + "/Resources";
        if(!Directory.Exists(filePath))
        {
            AssetDatabase.CreateFolder("Assets", "Resources");
        }
        filePath += "/Levels";
        if(!Directory.Exists(filePath))
        {
            AssetDatabase.CreateFolder("Assets/Resources", "Levels");
        }
        AssetDatabase.Refresh();
    }
#endif

    // Recreates the grid of m_levelData
    public void Load()
    {
        Clear();
        if(m_levelData == null)
        {
            return;
        }

        foreach(Vector3 v in m_levelData.m_tiles)
        {
            Tile t = Create();
            t.Load(v);
            m_tiles.Add(t.m_pos, t);
        }
    }

    
    // Places the selected tile from the atlas
    // into the position. No change to height
    public void Fill()
    {

    }

    // Places the selected tile from the atlas
    // into all positions selected. No change to height.
    public void FillArea()
    {

    }
}
