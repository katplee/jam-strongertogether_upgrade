using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIDragonPanel : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private static UIDragonPanel instance;
    public static UIDragonPanel Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<UIDragonPanel>();
            }
            return instance;
        }
    }

    private Animator animator;

    private DragonData selectedDragon;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        //for animations of dragonPanel
        animator.SetBool("mouseOn", true);

        //for animations of player
        PlayerSpriteLoader.Instance.SetAnimParameter("panelMouseOn", true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        //if there is a dragon selected, subpanel will not retract until fusion with dragon is complete
        if (UIDragonSubPanel.Instance.IsSelected) { return; }

        //for animations of dragonPanel
        animator.SetBool("mouseOn", false);

        //for animations of dragonSubPanel
        UIDragonSubPanel.Instance.ClearSubPanel();
        UIDragonSubPanel.Instance.ToggleInteractability(false);

        //for easy tracking of pointer movement
        PlayerSpriteLoader.Instance.SetAnimParameter("panelMouseOn", false);

        //set the player sprite to the original prior to mouse hover
        PlayerSpriteLoader.Instance.ResetSprite();
    }

    public void ParentParameters()
    {
        RectTransform rect = GetComponent<RectTransform>();
    }
}
