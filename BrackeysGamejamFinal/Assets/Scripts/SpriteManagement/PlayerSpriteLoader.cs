using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Animations;
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
    private const string objectTag = "Player";
    public List<Sprite> Sprites { get; set; }
    private AnimationClip animClip;
    private float animKeyFrameRate = 5;
    private Animator animator;

    private EditorCurveBinding spriteBinding = new EditorCurveBinding();
    #endregion

    #region Avatar/Sprite
    private Sprite selectedAvatar = null;
    private AnimationClip selectedAnimation = null;
    private SpriteRenderer spriteRenderer;
    #endregion

    private void Start()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        //pass the enemy sprite loader to the sprite manager
        SpriteManager.Instance.AssignPlayerSpriteLoader(this);

        //set the animator bool to off (work around.. check this)
        animator.SetBool("panelMouseOn", false);

        SubscribeEvents();
    }

    private void OnDestroy()
    {
        UnsubscribeEvents();
    }

    public void GenerateAnimClip(string tag)
    {
        if (tag != objectTag) { return; }

        animClip = new AnimationClip();
        animClip.frameRate = 20; // fps

        spriteBinding.type = typeof(SpriteRenderer);
        spriteBinding.path = "";
        spriteBinding.propertyName = "m_Sprite";

        ObjectReferenceKeyframe[] spriteKeyFrames = new ObjectReferenceKeyframe[Sprites.Count];

        for (int i = 0; i < (Sprites.Count); i++)
        {
            spriteKeyFrames[i] = new ObjectReferenceKeyframe();

            if (i == Sprites.Count - 1)
            {
                spriteKeyFrames[i].time = spriteKeyFrames[i - 1].time + (8 / animClip.frameRate);
            }
            else
            {
                spriteKeyFrames[i].time = (i / animClip.frameRate) * animKeyFrameRate;
            }
            spriteKeyFrames[i].value = Sprites[i];
        }

        AnimationUtility.SetObjectReferenceCurve(animClip, spriteBinding, spriteKeyFrames);
        
        if (!UIDragonSubPanel.Instance.IsSelected)
        {
            AssetDatabase.CreateAsset(animClip, "Assets/Animations/Player/PlayerAttackReadyPreview.anim");
        }
        else
        {
            AssetDatabase.CreateAsset(animClip, "Assets/Animations/Player/PlayerAttackReady.anim");
        }

        //AssetDatabase.CreateAsset(animClip, "Assets/Animations/Player/PlayerAttackReadyPreview.anim");
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        SetAnimation();
    }

    private void SetAnimation()
    {
        AnimatorController controller = (AnimatorController)animator.runtimeAnimatorController;
        AnimatorState state = controller.layers[0].stateMachine.states.FirstOrDefault(s => s.state.name.Equals("PlayerAttackReadyPreview")).state;
        controller.SetStateEffectiveMotion(state, animClip);
        animator.SetTrigger("animReady");
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

    public void SetSelection()
    {
        selectedAvatar = Sprites[0];
        //selectedAnimation = animClip;
    }

    public void ClearSelection()
    {
        selectedAvatar = null;
        //selectedAnimation = null;
    }

    public void FinalizeSelection()
    {
        //set the sprite/avatar
        spriteRenderer.sprite = selectedAvatar;

        //set the default player attack ready anim to the new anim
        GenerateAnimClip("Player");

        AnimatorController controller = (AnimatorController)animator.runtimeAnimatorController;
        AnimatorState state = controller.layers[0].stateMachine.states.FirstOrDefault(s => s.state.name.Equals("PlayerAttackReady")).state;
        controller.SetStateEffectiveMotion(state, animClip);

        //set dragon sub panel isSelected to false
        UIDragonSubPanel.Instance.IsSelected = false;
        UIDragonPanel.Instance.OnPointerExit(null);
    }

    private void SubscribeEvents()
    {
        SpriteManager.OnTransferComplete += GenerateAnimClip;
    }

    private void UnsubscribeEvents()
    {
        SpriteManager.OnTransferComplete -= GenerateAnimClip;
    }
}


