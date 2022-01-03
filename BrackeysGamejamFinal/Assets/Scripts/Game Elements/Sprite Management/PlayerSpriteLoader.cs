using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
//using UnityEditor;
//using UnityEditor.Animations;
using UnityEngine;

public class PlayerSpriteLoader : MonoBehaviour
{
    private static PlayerSpriteLoader instance;
    public static PlayerSpriteLoader Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<PlayerSpriteLoader>();
            }
            return instance;
        }
    }

    #region Animation
    public List<Sprite> Sprites { get; set; }
    private float animKeyFrameRate = 5;
    private Animator animator;

    #endregion

    #region Avatar/Sprite
    private Sprite selectedAvatar = null;
    private SpriteRenderer spriteRenderer;
    private int dragonIndex;
    private Sprite origPlayerSprite;
    #endregion

    private void Awake()
    {
        SubscribeEvents();
    }

    private void Start()
    {
        //pass the player enemy sprite loader to the sprite manager
        PlayerSpriteManager.Instance.AssignPlayerSpriteLoader(this);
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        RestartAnimation();
    }

    private void OnDestroy()
    {
        UnsubscribeEvents();
    }

    public void GenerateList()
    {
        Sprites = new List<Sprite>();
    }

    public void SetAnimParameter(string paramName, bool boolValue = false)
    {
        switch (paramName)
        {
            case "animReady":
                animator.SetTrigger(paramName);
                break;

            case "panelMouseOn":
                animator.SetBool(paramName, boolValue);
                break;

            default:
                break;
        }
    }

    private void SetAnimation(int dragonIndex)
    {
        this.dragonIndex = dragonIndex;

        //save player sprite prior to fuse preview animation
        origPlayerSprite = spriteRenderer.sprite;

        //set the animation in the blend tree
        animator.SetFloat("dragonIndex", dragonIndex);
        //animate
        animator.SetTrigger("animReady");

        //set the player sprite temporarily to the new sprite
        SetSelection();
    }

    public void SetSelection()
    {
        selectedAvatar = Sprites[0];
        spriteRenderer.sprite = selectedAvatar;
    }

    public void ClearSelection()
    {
        selectedAvatar = null;
    }

    public void ResetSprite()
    {
        spriteRenderer.sprite = origPlayerSprite;
        origPlayerSprite = null;
    }

    public bool FinalizeSelection()
    {
        if (!UIDragonSubPanel.Instance.IsSelected) { return false; }

        spriteRenderer.sprite = selectedAvatar;

        //animator.SetFloat("dragonIndex", dragonIndex); this is not needed, I think..
        animator.SetTrigger("animReady");

        //set dragon sub panel isSelected to false
        UIDragonSubPanel.Instance.IsSelected = false;
        UIDragonPanel.Instance.OnPointerExit(null);

        return true;
    }

    private void SubscribeEvents()
    {
        PlayerSpriteManager.OnTransferComplete += SetAnimation;
    }

    private void UnsubscribeEvents()
    {
        PlayerSpriteManager.OnTransferComplete -= SetAnimation;
    }

    private void RestartAnimation()
    {
        //check if there was a dragon flying when player went to fight with enemy
        InventorySave inventorySave = InventorySave.Instance.LoadInventoryData();
        InventoryData inventory = inventorySave.inventory;

        PlayerSpriteManager.Instance.AssignRefIndex(inventory.flyingDragonIndex);
        PlayerSpriteManager.Instance.LoadAndAssign();
    }
}


