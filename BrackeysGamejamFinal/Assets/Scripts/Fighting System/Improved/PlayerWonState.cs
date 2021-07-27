using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWonState : StateMachineBehaviour
{
    public BattleState State { get { return BattleState.WON; } }

    private ActionManager AM;
    private FightManager FM;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Debug.Log("Player won state start!");
        FightManager.Instance.ChangeStateName(State);

        SetManagers();
        SetButtonAccess();
        DisplayFightOverUI();
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
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
        FM = FightManager.Instance;
    }

    private void SetButtonAccess()
    {
        //player cannot attack
        //player can leave
        //player cannot fuse with a dragon
        AM.SetAllInteractability(false);

        //for testing purposes.. delete this after the fight over UI has been improved.
        AM.ALeave.SetInteractability(true);
    }

    private void DisplayFightOverUI()
    {
        //the fight ends here
        //UI should contain all buttons the player can choose from, including the leave button
        //maybe show some statistics of the player? win rate, etc.
        //show the inventory and provide actions the player can do with his/her dragons

        //and then, reflect changes in enemy stats
        FM.Enemy.AssignAsLastEnemy();

        //temporarily return to the basic scene automatically
        FM.OnLeave();

    }

}
