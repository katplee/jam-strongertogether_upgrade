using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
//using UnityEditor;
//using UnityEditor.Animations;
using UnityEngine;

public class EnemySpriteLoader : MonoBehaviour
{
    #region Animation
    public List<Sprite> Sprites { get; set; }
    private AnimationClip animClip;
    private float animKeyFrameRate = 5;
    private Animator animator;

    //private EditorCurveBinding spriteBinding = new EditorCurveBinding();
    #endregion

    #region Avatar/Sprite
    private SpriteRenderer spriteRenderer;
    private int enemyIndex;
    #endregion

   
    private void Awake()
    {
        SubscribeEvents();
    }

    private void Start()
    {
        //pass the enemy sprite loader to the sprite manager
        EnemySpriteManager.Instance.AssignEnemySpriteLoader(this);
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnDestroy()
    {
        UnsubscribeEvents();
    }

    /*
     * Removing all classes referring to the UnityEditor
     * 
    public void GenerateAnimClip()
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

        AssetDatabase.CreateAsset(animClip, "Assets/Animations & State Machines/Enemy/EnemyAttackReady.anim");
        AssetDatabase. SaveAssets();
        AssetDatabase.Refresh();

        SetAnimation();
    }
    */

    private void SetAnimation(int enemyIndex)
    {
        //AnimatorController controller = (AnimatorController)animator.runtimeAnimatorController;
        //AnimatorState state = controller.layers[0].stateMachine.states.FirstOrDefault(s => s.state.name.Equals("EnemyAttackReady")).state;
        //controller.SetStateEffectiveMotion(state, animClip);

        this.enemyIndex = enemyIndex;

        //set the animation in the blend tree
        animator.SetFloat("enemyIndex", enemyIndex);
        //animate
        animator.SetTrigger("animReady");
    }

    public void SetAvatar()
    {
        this.GetComponent<SpriteRenderer>().sprite = Sprites[0];
    }

    public void GenerateList()
    {
        Sprites = new List<Sprite>();
    }

    private void SubscribeEvents()
    {
        //EnemySpriteManager.OnTransferComplete += GenerateAnimClip;
        //EnemySpriteManager.OnTransferComplete += SetAvatar;
        EnemySpriteManager.OnTransferComplete += SetAnimation;
    }

    private void UnsubscribeEvents()
    {
        //EnemySpriteManager.OnTransferComplete -= GenerateAnimClip;
        //EnemySpriteManager.OnTransferComplete -= SetAvatar;
        EnemySpriteManager.OnTransferComplete -= SetAnimation;
    }
}


