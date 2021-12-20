using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleController : StateMachine
{
    // Everything that a battle controller will need.
    // Everything rule related will happen here I imagine.
    public CameraRig m_cameraRig;
    public Board m_board;
    public LevelData m_levelData;
    public Transform m_tileSelectionIndicator;
    public Point m_pos;

    public GameObject m_heroPrefab;
    public Unit m_currentUnit;
    public Tile m_currentTile { get { return m_board.GetTile(m_pos); } }

    public FacingIndicator facingIndicator;
    public Turn turn = new Turn();
    public List<Unit> units = new List<Unit>();

    // Round flow.
    public IEnumerator round;

    // Ui
    public AbilityMenuPanelController abilityMenuPanelController;
    public StatPanelContoller statPanelController;
    public HitSuccessIndicator hitSuccessIndicator;
    public BattleMessageController battleMessageController;
    public TileCoordinateController tileCoordinateController;
    // Ai

    public ComputerPlayer cpu;

    private void Start()
    {
        ChangeState<InitBattleState>();
    }
}