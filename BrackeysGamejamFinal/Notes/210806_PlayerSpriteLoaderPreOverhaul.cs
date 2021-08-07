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

        //set the animator bool to off (work around.. check this)
        //referencing the addressables for the sprites setting cause the error
        //animator.SetBool("panelMouseOn", false);
    }

    private void OnDestroy()
    {
        UnsubscribeEvents();
    }

    public void GenerateAnimClip(bool isBase = false)
    {
        if (!isBase)
        {
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

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
        else
        {
            animClip = AssetDatabase.LoadMainAssetAtPath("Assets/Animations/Player/BasePlayerAttackReady.anim") as AnimationClip;
        }

        if (isBase)
        {
            //set the sprite/avatar
            //SetSelection();
            //spriteRenderer.sprite = selectedAvatar;
        }

        SetAnimation(isBase);
    }

    private void SetAnimation(bool isBase = false)
    {
        AnimatorController controller = (AnimatorController)animator.runtimeAnimatorController;
        //AnimatorController controller = AssetDatabase.LoadAssetAtPath<AnimatorController>(AssetDatabase.GetAssetPath(runtimeController));

        AnimatorState state = isBase ?
            controller.layers[0].stateMachine.states.FirstOrDefault(s => s.state.name.Equals("PlayerAttackReady")).state :
            controller.layers[0].stateMachine.states.FirstOrDefault(s => s.state.name.Equals("PlayerAttackReadyPreview")).state;

        //get animator parameters before rigging animator
        bool panelMouseOn = animator.GetBool("panelMouseOn");
        
        //rig animator
        //controller.SetStateEffectiveMotion(state, animClip);

        //controller.AddParameter("panelMouseOn", AnimatorControllerParameterType.Bool);

        //set animator parameters back to pre-rigged state
        animator.SetBool("panelMouseOn", true);
        animator.SetTrigger("animReady");
    }

    private void SetInitialAnimation()
    {
        //check if there is a dragon following you


        //if there is a dragon following you, the starting animation should be of player fused with dragon

        //if there is no dragon following you, the starting animation is the regular one

    }

    private void SetInitialSprite()
    {
        //initial sprite is always the regular one

        //check if there is a dragon following you

        //if there is a dragon following you, the player's sprite should eventually convert to the one fused with dragon

        //if there is no dragon following you, the player's sprite will not change
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
        GenerateAnimClip();

        AnimatorController controller = (AnimatorController)animator.runtimeAnimatorController;
        AnimatorState state = controller.layers[0].stateMachine.states.FirstOrDefault(s => s.state.name.Equals("PlayerAttackReady")).state;
        controller.SetStateEffectiveMotion(state, animClip);

        //set dragon sub panel isSelected to false
        UIDragonSubPanel.Instance.IsSelected = false;
        UIDragonPanel.Instance.OnPointerExit(null);
    }

    private void SubscribeEvents()
    {
        PlayerSpriteManager.OnTransferComplete += GenerateAnimClip;
    }

    private void UnsubscribeEvents()
    {
        PlayerSpriteManager.OnTransferComplete -= GenerateAnimClip;
    }
}


