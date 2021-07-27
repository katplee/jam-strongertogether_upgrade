using UnityEngine.AddressableAssets;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;
using System;

public class SpriteManager : MonoBehaviour
{
    /*
     * Things which are different:
     * address (sprite sheet address)
     * animAvailableKeyFrames
     * spriteLoader
     */

    public static event Action<string> OnTransferComplete;

    private static SpriteManager instance;
    public static SpriteManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<SpriteManager>();
            }
            return instance;
        }
    }

    #region Addresses
    private const string enemiesSpriteSheetAddress = "Sprites/ENEMIES.png";
    private const string playerSpriteSheetAddress = "Sprites/PLAYER.png";
    #endregion

    private string objectTag;
    public int refIndex = 0;
    private Queue<Sprite> animSprites = new Queue<Sprite>();
    private int animKeyFrameCount;
    private int animAvailableKeyFrames;
    private string sheetAddress;

    //private readonly Dictionary<AssetReference, AsyncOperationHandle<GameObject>> _asyncOperationHandles =
    //    new Dictionary<AssetReference, AsyncOperationHandle<GameObject>>();

    #region Enemy sprite manager
    private EnemySpriteLoader enemySpriteLoader;
    private PlayerSpriteLoader playerSpriteLoader;
    #endregion

    #region Player sprite manager
    private const int solo = 0;
    private const int fusedBase = 4;
    private const int fusedFire = 8;
    private const int fusedAir = 12;
    private const int fusedWater = 16;
    private const int fusedEarth = 20;
    #endregion

    public void LoadAndAssign(string targetObject)
    {
        InputParameters(targetObject);

        if (targetObject == "Player") { AssignRefIndex(refIndex * animAvailableKeyFrames); }

        Addressables.LoadAssetAsync<IList<Sprite>>(sheetAddress).Completed += (obj) =>
        {
            if (obj.Result == null)
            {
                Debug.LogError("Sheets not uploaded properly.");
                return;
            }

            for (int i = 0; i < animKeyFrameCount; i++)
            {
                if (i == animAvailableKeyFrames) { i = 0; }

                animSprites.Enqueue(obj.Result[(refIndex / animAvailableKeyFrames) * animAvailableKeyFrames + i]);

                if (animSprites.Count == animKeyFrameCount) { break; }
            }

            //assign sprites for animation
            AssignSprites(targetObject);
            OnTransferComplete?.Invoke(objectTag);
        };
    }

    private void InputParameters(string targetObject)
    {
        objectTag = targetObject;

        switch (targetObject)
        {
            case "Enemy":
                sheetAddress = enemiesSpriteSheetAddress;
                animAvailableKeyFrames = 3;
                animKeyFrameCount = 4;
                break;

            case "Player":
                sheetAddress = playerSpriteSheetAddress;
                animAvailableKeyFrames = 4;
                animKeyFrameCount = 5;
                break;

            default:
                break;
        }
    }

    public void AssignRefIndex(int index)
    {
        //called via Enemy.ReloadAsLastEnemy()
        //called in FightManager for player animation preview
        refIndex = index;
    }

    public void AssignPlayerSpriteLoader(PlayerSpriteLoader loader)
    {
        playerSpriteLoader = loader;
    }

    public void AssignEnemySpriteLoader(EnemySpriteLoader loader)
    {
        enemySpriteLoader = loader;
    }

    private void AssignSprites(string targetObject)
    {
        switch (targetObject)
        {
            case "Enemy":
                AssignEnemySprites();
                break;

            case "Player":
                AssignPlayerSprites();
                break;

            default:
                break;
        }
    }

    private void AssignEnemySprites()
    {
        if (enemySpriteLoader.Sprites == null) { enemySpriteLoader.GenerateList(); }

        if (enemySpriteLoader.Sprites.Count != 0) { enemySpriteLoader.Sprites.Clear(); }

        for (int i = 0; i < animKeyFrameCount; i++)
        {
            enemySpriteLoader.Sprites.Add(animSprites.Dequeue());
        }
    }

    private void AssignPlayerSprites()
    {
        if (playerSpriteLoader.Sprites == null) { playerSpriteLoader.GenerateList(); }

        if (playerSpriteLoader.Sprites.Count != 0) { playerSpriteLoader.Sprites.Clear(); }

        for (int i = 0; i < animKeyFrameCount; i++)
        {
            playerSpriteLoader.Sprites.Add(animSprites.Dequeue());
        }
    }
}
