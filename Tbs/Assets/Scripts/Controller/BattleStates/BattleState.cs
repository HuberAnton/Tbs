using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BattleState : State
{
    protected BattleController m_owner;

    // References to the battlecontroller.
    // I suppose you could just call this on the owner?
    // Most likely this is done for protection sake.
    public CameraRig m_camera { get { return m_owner.m_cameraRig; } }
    public Board m_board { get { return m_owner.m_board; } }
    public LevelData m_levelData { get { return m_owner.m_levelData; } }
    public Transform m_tileSelectionIndicator { get { return m_owner.m_tileSelectionIndicator; } }
    public Point m_pos { get { return m_owner.m_pos; } set { m_owner.m_pos = value; } }
    public AbilityMenuPanelController abilityMenuPanelController { get { return m_owner.abilityMenuPanelController; } }
    public Turn turn { get { return m_owner.turn; } }
    public List<Unit> units { get { return m_owner.units; } }

    public StatPanelContoller statPanelContoller { get { return m_owner.statPanelController; } }

    public HitSuccessIndicator hitSuccessIndicator { get { return m_owner.hitSuccessIndicator; } }

    public TileCoordinateController tileCoordinateController { get { return m_owner.tileCoordinateController; } }

    // Ai or player controlled.
    protected Driver driver;

    public override void Enter()
    {
        // No actor means null driver.
        driver = (turn.actor != null) ? turn.actor.GetComponent<Driver>() : null;
        base.Enter();
    }


    protected virtual void Awake()
    {
        m_owner = GetComponent<BattleController>();
    }

    protected override void AddListeners()
    {
        // No input checks.
        // Might need to add a skip event instead to hurry up movement.
        if (driver == null || driver.Current == Drivers.Human)
        {
            InputController.moveEvent += OnMove;
            InputController.fireEvent += OnFire;
        }
        else
        {
            // Code for skipping ai
            InputController.fireEvent += OnSkip;
        }
    }

    protected override void RemoveListeners()
    {
        if (driver == null || driver.Current == Drivers.Human)
        {
            InputController.moveEvent -= OnMove;
            InputController.fireEvent -= OnFire;
        }
        else
        {
            // Code for skipping ai
            InputController.fireEvent -= OnSkip;
        }
    }

    protected virtual void OnMove(object sender, InfoEventArgs<Point> e)
    {

    }

    protected virtual void OnFire(object sender, InfoEventArgs<int> e)
    {

    }

    protected virtual void OnSkip(object sender, InfoEventArgs<int> e)
    {
        Debug.Log("Skip Test");
    }


    protected virtual void SelectTile(Point p)
    {
        if(m_pos == p || ! m_board.m_tiles.ContainsKey(p))
        {
            return;
        }

        m_pos = p;
        m_tileSelectionIndicator.localPosition = m_board.m_tiles[p].m_center;
    }

    protected virtual Unit GetUnit(Point p)
    {
        // Pass in a point to get the tile. If it exists.
        Tile t = m_board.GetTile(p);
        // If nothing is located on the tile return null
        GameObject content = t != null ? t.m_content : null;
        // Would probably need to do some amount of parsing here
        // to make sure it gets the write thing on the location.
        // Works for now.
        return content != null ? content.GetComponent<Unit>() : null;
    }

    protected virtual void RefreshPrimaryStatPanel(Point p)
    {
        Unit target = GetUnit(p);
        if (target != null)
            statPanelContoller.ShowPrimary(target.gameObject);
        else
            statPanelContoller.HidePrimary();
    }

    protected virtual void RefreshSecondaryStatPanel(Point p)
    {
        Unit target = GetUnit(p);
        if (target != null)
            statPanelContoller.ShowSecondary(target.gameObject);
        else
            statPanelContoller.HideSecondary();
    }

    protected virtual bool DidPlayerWin()
    {
        return m_owner.GetComponent<BaseVictoryCondition>().Victor == Alliances.Hero;
    }

    protected virtual bool IsBattleOver()
    {
        return m_owner.GetComponent<BaseVictoryCondition>().Victor != Alliances.None;
    }


}
