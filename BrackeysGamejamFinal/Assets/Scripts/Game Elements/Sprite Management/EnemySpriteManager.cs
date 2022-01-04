using UnityEngine.AddressableAssets;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;
using System;

public class EnemySpriteManager : MonoBehaviour
{
    /*
     * Things which are different:
     * address (sprite sheet address)
     * animAvailableKeyFrames
     * spriteLoader
     */

    public static event Action<int> OnTransferComplete;

    private static EnemySpriteManager instance;
    public static EnemySpriteManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<EnemySpriteManager>();
            }
            return instance;
        }
    }

    #region Addresses
    private const string enemiesSpriteSheetAddress = "Sprites/ENEMIES.png";
    #endregion

    private int refIndex = 0;
    private Queue<Sprite> animSprites = new Queue<Sprite>();
    private int animKeyFrameCount;
    private int animAvailableKeyFrames;
    private string sheetAddress;

    #region Enemy sprite manager
    private EnemySpriteLoader enemySpriteLoader;
    #endregion

    public void LoadAndAssign()
    {
        InputParameters();

        int adjustedIndex = (refIndex / animAvailableKeyFrames);

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

                animSprites.Enqueue(obj.Result[adjustedIndex * animAvailableKeyFrames]);

                if (animSprites.Count == animKeyFrameCount) { break; }
            }

            //assign sprites for animation
            AssignEnemySprites();
            OnTransferComplete?.Invoke(adjustedIndex);
        };
    }

    private void InputParameters()
    {
        sheetAddress = enemiesSpriteSheetAddress;
        animAvailableKeyFrames = 3;
        animKeyFrameCount = 4;
    }

    public void AssignRefIndex(int index)
    {
        //called via Enemy.ReloadAsLastEnemy()
        //called in FightManager for player animation preview
        refIndex = index;
    }

    public void AssignEnemySpriteLoader(EnemySpriteLoader loader)
    {
        enemySpriteLoader = loader;
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
}
