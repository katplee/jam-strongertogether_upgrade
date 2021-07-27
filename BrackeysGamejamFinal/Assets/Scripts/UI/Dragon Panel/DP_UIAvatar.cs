using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.AddressableAssets;

public class DP_UIAvatar : UIObject
{
    private DP_UIDragon parent;

    private Image image;

    #region Addresses
    private const string dragonSpriteSheetAddress = "Sprites/DRAGONS.png";
    private const int baseDragonIndex = 22;
    private const int fireDragonIndex = 13;
    private const int waterDragonIndex = 17;
    private const int earthDragonIndex = 20;
    private const int airDragonIndex = 37;
    #endregion

    public override string Label
    {
        get { return GetType().Name; }
    }

    private void Start()
    {
        image = GetComponent<Image>();

        if (transform.parent.TryGetComponent(out parent))
        {
            parent.DeclareThis(Label, this);
        }
    }

    public void ChangeAvatar()
    {
        Addressables.LoadAssetAsync<IList<Sprite>>(dragonSpriteSheetAddress).Completed += (obj) =>
        {
            if (obj.Result == null)
            {
                Debug.LogError("Sheets not uploaded properly.");
                return;
            }

            switch (UIDragonSubPanel.Instance.Type)
            {
                case DragonType.BASE:
                    image.sprite = obj.Result[baseDragonIndex];
                    break;

                case DragonType.FIRE:
                    image.sprite = obj.Result[fireDragonIndex];
                    break;

                case DragonType.WATER:
                    image.sprite = obj.Result[waterDragonIndex];
                    break;
                
                case DragonType.EARTH:
                    image.sprite = obj.Result[earthDragonIndex];
                    break;
                
                case DragonType.AIR:
                    image.sprite = obj.Result[airDragonIndex];
                    break;
                
            }
        };         
    }
}
