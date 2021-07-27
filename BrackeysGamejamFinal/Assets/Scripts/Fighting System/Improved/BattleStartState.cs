using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleStartState : StateMachineBehaviour
{
    public BattleState State { get { return BattleState.START; } }

    private FightManager FM;
    private PrefabManager PM;
    private TransformManager TM;
    private HUDManager HM;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Debug.Log("Battle state start!");
        FightManager.Instance.ChangeStateName(State);

        SetManagers();

        //instantiate the player prefab at the player transform
        //FM.PGO = Instantiate(PM.PPlayer, TM.TPlayer);
        FM.PGO = TM.TPlayer.transform.GetChild(0).gameObject;
        //add the player script and add the data
        FM.Player = FM.PGO.AddComponent<Player>();
        FM.Player.InitializeDeserialization();
        //update the player's HUD
        FM.PHUD = HM.HPlayer;
        FM.PHUD.UpdateHUD(FM.Player);

        //do the same for the enemy
        //FM.EGO = Instantiate(PM.PEnemy, TM.TEnemy);
        FM.EGO = TM.TEnemy.transform.GetChild(0).gameObject;
        FM.Enemy = FM.EGO.AddComponent<Enemy>();
        //assign the sprites for animation
        FM.Enemy.ReloadAsLastEnemy(); //ref index assignment is done here
        SpriteManager.Instance.LoadAndAssign("Enemy");
        //update the enemy's HUD
        FM.EHUD = HM.HEnemy;
        FM.EHUD.UpdateHUD(FM.Enemy);

        MoveToPlayerTurn(animator);
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
        FM = FightManager.Instance;
        PM = PrefabManager.Instance;
        TM = TransformManager.Instance;
        HM = HUDManager.Instance;
    }

    private void MoveToPlayerTurn(Animator animator)
    {
        animator.SetBool("hasStarted", true);
        animator.SetBool("isPlayerTurn", true);
    }
}
