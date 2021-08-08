using UnityEngine.AddressableAssets;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;
using System;

public class PlayerSpriteManager : MonoBehaviour
{
    /*
     * Things which are different:
     * address (sprite sheet address)
     * animAvailableKeyFrames
     * spriteLoader
     */

    public static event Action<int> OnTransferComplete;

    private static PlayerSpriteManager instance;
    public static PlayerSpriteManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<PlayerSpriteManager>();
            }
            return instance;
        }
    }

    #region Addresses
    private const string playerSpriteSheetAddress = "Sprites/PLAYER.png";
    #endregion

    public Sprite[] spriteSheet;
    private int refIndex = 0;
    private Queue<Sprite> animSprites = new Queue<Sprite>();
    private int animKeyFrameCount;
    private int animAvailableKeyFrames;
    private string sheetAddress;

    //private readonly Dictionary<AssetReference, AsyncOperationHandle<GameObject>> _asyncOperationHandles =
    //    new Dictionary<AssetReference, AsyncOperationHandle<GameObject>>();

    #region Player sprite manager
    private PlayerSpriteLoader playerSpriteLoader;
    private const int solo = 0;
    private const int fusedBase = 1;
    private const int fusedFire = 2;
    private const int fusedAir = 3;
    private const int fusedWater = 4;
    private const int fusedEarth = 5;
    private List<int> spriteList = new List<int>()
    {
        solo,
        fusedBase,
        fusedFire,
        fusedWater,
        fusedEarth,
        fusedAir
    };
    #endregion

    public void LoadAndAssign()
    {
        InputParameters();

        /*
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

                animSprites.Enqueue(obj.Result[spriteList[refIndex] * animAvailableKeyFrames + i]);

                if (animSprites.Count == animKeyFrameCount) { break; }
            }

            //assign sprites for animation
            AssignPlayerSprites();
            OnTransferComplete?.Invoke(refIndex);
        };
        */

        for (int i = 0; i < animKeyFrameCount; i++)
        {
            if (i == animAvailableKeyFrames) { i = 0; }

            animSprites.Enqueue(spriteSheet[spriteList[refIndex] * animAvailableKeyFrames + i]);

            if (animSprites.Count == animKeyFrameCount) { break; }
        }

        //assign sprites for animation
        AssignPlayerSprites();
        OnTransferComplete?.Invoke(refIndex);
    }

    private void InputParameters()
    {
        sheetAddress = playerSpriteSheetAddress;
        animAvailableKeyFrames = 4;
        animKeyFrameCount = 1;
    }

    public void AssignRefIndex(int index)
    {
        //called in BattleStateStart for starting player animation preview
        //called in FightManager for player animation preview
        refIndex = index;
    }

    public void AssignPlayerSpriteLoader(PlayerSpriteLoader loader)
    {
        playerSpriteLoader = loader;
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
