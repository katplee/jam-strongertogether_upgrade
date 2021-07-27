using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTurnState : StateMachineBehaviour
{
    public BattleState State { get { return BattleState.PLAYERTURN; } }

    private ActionManager AM;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        FightManager.Instance.ChangeStateName(State);
        Debug.Log("Player turn state start!");

        SubscribeEvents();
        SetManagers();
        SetButtonAccess();
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        UnsubscribeEvents();
    }

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}

    private void SetManagers()
    {
        AM = ActionManager.Instance;
    }

    private void SetButtonAccess()
    {
        //player can attack
        //player can leave
        //player can fuse with a dragon
        AM.SetAllInteractability(true);
    }

    private void FireButtonResponse(string buttonName)
    {
        switch (buttonName)
        {
            case "Attack":
                AnimatorManager.Animator.SetBool("isAttacking", true);
                break;

            case "Fuse":
                //change the avatar
                //change the default animation
                PlayerSpriteLoader.Instance.FinalizeSelection();

                //change HP stats as a result of fusion with dragon
                FightManager.Instance.FuseDragonHP();

                //change animator parameters
                MoveToPlayerTurn();
                break;

            default:
                break;
        }
    }

    private void SubscribeEvents()
    {
        ActionManager.OnButtonPress += FireButtonResponse;
    }

    private void UnsubscribeEvents()
    {
        ActionManager.OnButtonPress -= FireButtonResponse;
    }

    private void MoveToPlayerTurn()
    {
        AnimatorManager.Animator.SetBool("isPlayerTurn", false);
        AnimatorManager.Animator.SetBool("isAttacking", false);
    }
}
