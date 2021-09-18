using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUDUpdateState : StateMachineBehaviour
{
    /*
     * Animator parameters that are called in this state:
     * ..hp_value
     * ..hp_bar
     * ..armor_value
     * ..armor_bar
     * ..player
     */

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //update the HUD
        GameObject HUD = UIUpdater.Instance.gameObject;
        UIBattleHUD HUDComponent = HUD.GetComponent<UIBattleHUD>();
        HUDComponent.UpdateHUD(Player.Instance);

        SetStateToDefault(animator);
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
    }

    private void SetStateToDefault(Animator animator)
    {
        animator.SetBool("hp_value", false);
        animator.SetBool("hp_bar", false);
        animator.SetBool("armor_value", false);
        animator.SetBool("armor_bar", false);
        animator.SetBool("player", false);
    }
}
