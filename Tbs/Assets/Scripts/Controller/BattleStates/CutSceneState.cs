using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutSceneState : BattleState
{
    ConversationController conversationController;
    ConverstaionData data;

    protected override void Awake()
    {
        base.Awake();
        conversationController = m_owner.GetComponentInChildren<ConversationController>();

    }


    protected override void OnDestroy()
    {
        base.OnDestroy();

    }

    public override void Enter()
    {
        // If no conversation data game will crash.
        // Might be better to have a default for a catch case.
        // Data will be stored with a level name folder of the actual level.
        // eg data = Resources.Load<ConverstaionData>("/{0}/Conversations/OutroSceneWin", level.name);
        base.Enter();
        // Ending
        if (IsBattleOver())
        {
            if (DidPlayerWin())
                data = Resources.Load<ConverstaionData>("Conversations/OutroSceneWin");
            else
                data = Resources.Load<ConverstaionData>("Conversations/OutroSceneLose");
        }
        // Intro
        else
        {
            data = Resources.Load<ConverstaionData>("Conversations/IntroScene");
        }
        //if (data == null)
        //    data = Resources.Load<ConverstaionData>("Conversations/PlaceHolder");
        conversationController.Show(data);
    }

    public override void Exit()
    {
        base.Exit();
        if (data)
            Resources.UnloadAsset(data);
    }

    protected override void AddListeners()
    {
        base.AddListeners();
        ConversationController.completeEvent += OnCompleteConversation; 
    }

    protected override void RemoveListeners()
    {
        base.RemoveListeners();
        ConversationController.completeEvent -= OnCompleteConversation;
    }

    protected override void OnFire(object sender, InfoEventArgs<int> e)
    {
        base.OnFire(sender, e);
        conversationController.Next();
    }

   
    void OnCompleteConversation(object sender, System.EventArgs e)
    {
        // Check if battle is over
        if (IsBattleOver())
        {
            m_owner.ChangeState<EndFacingState>();
        }
        else
        {
            // This is mostly for testing I should just make another state.
            if (units.Count > 0)
                m_owner.ChangeState<SelectUnitState>();
            else
                m_owner.ChangeState<ExploreState>();
        }
    }
}
