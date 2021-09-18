using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class UITameMenu : MonoBehaviour
{
    public static event Action OnTameMenuDisplay;

    private static UITameMenu instance;
    public static UITameMenu Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<UITameMenu>();
            }
            return instance;
        }
    }

    private RectTransform rect;
    private Player player;
    private bool isChoosing = false;

    private int flyingDragonIndex = 0;
    private GameObject flyingDragon = null;

    //Addressables-related variables
    #region Addresses
    /*
    private const string flyBaseDragonPrefabAddress = "Prefabs/FLY_BASE.prefab";
    private const string flyFireDragonPrefabAddress = "Prefabs/FLY_FIRE.prefab";
    private const string flyWaterDragonPrefabAddress = "Prefabs/FLY_WATER.prefab";
    private const string flyEarthDragonPrefabAddress = "Prefabs/FLY_EARTH.prefab";
    private const string flyAirDragonPrefabAddress = "Prefabs/FLY_AIR.prefab";
    private List<string> addressList = new List<string>()
    {
        flyBaseDragonPrefabAddress,
        flyFireDragonPrefabAddress,
        flyWaterDragonPrefabAddress,
        flyEarthDragonPrefabAddress,
        flyAirDragonPrefabAddress
    };
    */
    public List<GameObject> prefabList = new List<GameObject>();
    #endregion
    
    private Dictionary<string, KeyValuePair<AssetReference, AsyncOperationHandle<GameObject>>> asyncOperationHandles =
        new Dictionary<string, KeyValuePair<AssetReference, AsyncOperationHandle<GameObject>>>();

    private void Start()
    {
        player = GetComponentInParent<Player>();

        //set panel to zero scale, but active
        rect = GetComponent<RectTransform>();
        rect.localScale = new Vector3(0f, 0f, 0f);
    }

    public void Spawn(int addressListIndex)
    {
        //the addressListIndex will be linked to the PlayerSpriteLoader, therefore, must be 1-based
        if (addressListIndex == flyingDragonIndex) { HideFlyingDragon(); return; }

        HideFlyingDragon();
        flyingDragonIndex = addressListIndex;

        //addressList is 0-based, hence the -1
        GameObject dragonPrefab = prefabList[addressListIndex - 1];

        SpawnPrefabFromLoadedReference(dragonPrefab);
    }

    private void SpawnPrefabFromLoadedReference(GameObject dragonPrefab)
    {
        GameObject instance = Instantiate(dragonPrefab, transform.root);
        flyingDragon = instance;
    }

    /*
    public void Spawn(int addressListIndex)
    {
        //the addressListIndex will be linked to the PlayerSpriteLoader, therefore, must be 1-based
        if (addressListIndex == flyingDragonIndex) { HideFlyingDragon(); return; }

        HideFlyingDragon();
        flyingDragonIndex = addressListIndex;

        //addressList is 0-based, hence the -1
        string assetAddress = addressList[addressListIndex - 1];

        if (asyncOperationHandles.ContainsKey(assetAddress))
        {
            if (asyncOperationHandles[assetAddress].Value.IsDone)
            {
                SpawnPrefabFromLoadedReference(asyncOperationHandles[assetAddress].Key);
            }
            return;
        }

        LoadAndSpawnAssetReference(assetAddress);
    }

    private void LoadAndSpawnAssetReference(string assetAddress)
    {
        AssetReference assetReference = new AssetReference(assetAddress);
        var op = Addressables.LoadAssetAsync<GameObject>(assetReference);
        asyncOperationHandles[assetAddress] =
            new KeyValuePair<AssetReference, AsyncOperationHandle<GameObject>>(assetReference, op);
        op.Completed += (obj) =>
        {
            SpawnPrefabFromLoadedReference(assetReference);
        };
    }

    private void SpawnPrefabFromLoadedReference(AssetReference assetReference)
    {
        assetReference.InstantiateAsync(transform.root).Completed += (asyncOpHandle) =>
        {
            flyingDragon = asyncOpHandle.Result;
            var notify = asyncOpHandle.Result.AddComponent<UIDestroyNotif>();
            notify.Destroyed += Remove;
            notify.AssetReference = assetReference;
        };
    }

    private void Remove(AssetReference assetReference, UIDestroyNotif notif)
    {
        Addressables.ReleaseInstance(notif.gameObject);

        AsyncOperationHandle<GameObject> asyncOpHandle = asyncOperationHandles.Where(a => a.Value.Key == assetReference).FirstOrDefault().Value.Value;
        string assetAddress = asyncOperationHandles.Where(a => a.Value.Key == assetReference).FirstOrDefault().Key;

        if (asyncOpHandle.IsValid())
        {
            Addressables.Release(asyncOpHandle);
        }

        asyncOperationHandles.Remove(assetAddress);
    }

    private void ClearReferences()
    {
        asyncOperationHandles.Clear();
    }
    */

    private void AssignFlyingDragonIndex()
    {
        InventorySave.Instance.AssignFlyingDragonIndex(flyingDragonIndex);
    }

    private void HideFlyingDragon()
    {
        flyingDragonIndex = 0;
        Destroy(flyingDragon);
        flyingDragon = null;
    }

    public void ToggleVisibility()
    {
        isChoosing = isChoosing ^ true;

        if (Vector3.Magnitude(rect.localScale) == 0)
        {
            rect.localScale = new Vector3(1f, 1f, 1f);
            gameObject.SetActive(true);
        }
        else
        {
            gameObject.SetActive(isChoosing);
        }

        player.IsChoosingTame = isChoosing;

        if (!isChoosing)
        {
            //prepare for possible transition to attack scene
            AssignFlyingDragonIndex();

            //clear memory of async operation handles
            //ClearReferences(); 
        }
    }

    public void UpdateTameMenuButtons()
    {
        OnTameMenuDisplay?.Invoke();
    }
}
