using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.EventSystems;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UI;

//ERRORS:
//Assertion failed on expression: 'ShouldRunBehaviour()'
//Comes up whenever the addressables class comes up.
//Alternative: 

public class UIDragonSubPanel : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerUpHandler, IPointerDownHandler
{
    private static UIDragonSubPanel instance;
    public static UIDragonSubPanel Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<UIDragonSubPanel>();
            }
            return instance;
        }
    }

    public DragonType Type { get; private set; }

    //Interactibility-related variables
    private RectTransform rect;
    private Image image;
    public bool IsSelected { get; set; } //has a pet dragon selected

    //Position-related variables
    private Vector3 mainPanelUpperLeftPt;
    private Vector3 mousePosition;

    //Dragon-listing-related variables
    private Transform interim; //refers to the vertical container in-between this and the dragon list
    private List<DragonData> dragonList = new List<DragonData>();
    //private int dragonPrefabsGenerated;
    private List<GameObject> dragonPrefabsList = new List<GameObject>();
    public GameObject dragonPrefab; //this needs to be fixed!

    //Addressables-related variables
    private const string dragonPrefabAddress = "Prefabs/DRAGON.prefab";
    private Dictionary<AssetReference, List<GameObject>> spawnedDragonMenus =
        new Dictionary<AssetReference, List<GameObject>>();
    private Dictionary<AssetReference, Queue<DragonData>> queuedSpawnRequests =
        new Dictionary<AssetReference, Queue<DragonData>>();
    private Dictionary<string, KeyValuePair<AssetReference, AsyncOperationHandle<GameObject>>> asyncOperationHandles =
        new Dictionary<string, KeyValuePair<AssetReference, AsyncOperationHandle<GameObject>>>();

    void Start()
    {
        //set isSelected bool to false
        IsSelected = false;

        //set panel to zero scale, but active
        rect = GetComponent<RectTransform>();
        rect.localScale = new Vector3(0f, 0f, 0f);

        //retrieve interim vertical container
        interim = transform.GetChild(0);

        /*
        //set the dragon prefab
        Addressables.LoadAssetAsync<GameObject>(dragonPrefabAddress).Completed += (obj) =>
        {
            if (obj.Result == null)
            {
                Debug.LogError("Dragon prefab not uploaded properly.");
                return;
            }

            dragonPrefab = obj.Result;
        };
        */
    }
    
    public void PrepareForDragonListGeneration()
    {
        if (dragonList == null) { return; }

        if (dragonList.Count == 0) { return; }
        
        //instantiate enough dragon prefabs for each dragon on list
        for (int i = 0; i < dragonList.Count; i++)
        {
            if (dragonPrefabsList.Count == dragonList.Count) { return; }

            GameObject go = Instantiate(dragonPrefab, interim);
            dragonPrefabsList.Add(go);
        }

        StartCoroutine(UpdateDragonList());
    }

    IEnumerator UpdateDragonList()
    {
        //distribute info to each instantiated dragon prefab
        for (int i = 0; i < dragonPrefabsList.Count; i++)
        {
            DP_UIDragon dragon = dragonPrefabsList[i].GetComponent<DP_UIDragon>();
            if (dragon.uploadState != 0) { yield return null; }
            dragon.UpdateDragon(dragonList[i]);
        }
    }

    public void ClearSubPanel()
    {
        foreach (Transform child in interim)
        {
            Destroy(child.gameObject);
        }

        dragonPrefabsList.Clear();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
    }

    public void OnPointerExit(PointerEventData eventData)
    {
    }

    public void OnPointerUp(PointerEventData eventData)
    {
    }

    public void OnPointerDown(PointerEventData eventData)
    {
    }

    private Vector3 ParentUpperLeftPoint()
    {
        //obtain the necessary points for position computation
        RectTransform rect = transform.parent.GetComponent<RectTransform>();
        BoxCollider2D box = transform.parent.GetComponent<BoxCollider2D>();
        mainPanelUpperLeftPt = new Vector3(
            box.bounds.center.x - box.bounds.extents.x,
            box.bounds.center.y + box.bounds.extents.y,
            0);

        return mainPanelUpperLeftPt;
    }

    private Vector3 MousePosition(Vector3 mousePosition)
    {
        this.mousePosition = mousePosition;

        return this.mousePosition;
    }


    public void PassDragonList(List<DragonData> list)
    {
        dragonList = list;
    }

    public void SetDragonType(DragonType dragonType)
    {

        Type = dragonType;
    }

    public bool ToggleInteractability(bool buttonChanged)
    {
        if (Vector3.Magnitude(rect.localScale) == 0)
        {
            rect.localScale = new Vector3(1f, 1f, 1f);

            gameObject.SetActive(true);
        }
        else if (buttonChanged)
        {
            gameObject.SetActive(true);
        }
        else
        {
            gameObject.SetActive(gameObject.activeSelf ^ true);
        }

        return gameObject.activeSelf;
    }

    public void GenerateSubPanel(Vector3 mousePosition)
    {
        Vector3 subPanelUpperLeftPoint = MousePosition(mousePosition) - ParentUpperLeftPoint();
        RectTransform subPanel = GetComponent<RectTransform>();

        if (mousePosition.x > 0)
        {
            subPanel.pivot = new Vector2(1f, 1f);
        }
        else
        {
            subPanel.pivot = new Vector2(0f, 1f);
        }

        subPanel.anchoredPosition = subPanelUpperLeftPoint;
    }

    #region Addressables approach
    //Causes the Assertion failed on expression: 'ShouldRunBehaviour()' error
    /*
    public void PrepareForDragonListGeneration()
    {
        if (dragonList == null) { return; }

        if (dragonList.Count == 0) { return; }
        
        if (dragonPrefabsGenerated == dragonList.Count) { return; }
        
        //instantiate enough dragon prefabs for each dragon on list
        for (int i = 0; i < dragonList.Count; i++)
        {
            Spawn(dragonPrefabAddress, dragonList[i]);
        }
    }
    private void Spawn(string assetAddress, DragonData dragonData)
    {
        if (asyncOperationHandles.ContainsKey(assetAddress))
        {
            if (asyncOperationHandles[assetAddress].Value.IsDone)
            {
                SpawnDragonMenuFromLoadedReference(asyncOperationHandles[assetAddress].Key, dragonData);
            }
            else
            {
                EnqueueSpawnForAfterLoading(asyncOperationHandles[assetAddress].Key, dragonData);
            }
            return;
        }

        LoadAndSpawnAssetReference(assetAddress, dragonData);
    }

    private void EnqueueSpawnForAfterLoading(AssetReference assetReference, DragonData dragonData)
    {
        if(queuedSpawnRequests.ContainsKey(assetReference) == false)
        {
            queuedSpawnRequests[assetReference] = new Queue<DragonData>();
        }
        queuedSpawnRequests[assetReference].Enqueue(dragonData);
    }

    private void LoadAndSpawnAssetReference(string assetAddress, DragonData dragonData)
    {
        AssetReference assetReference = new AssetReference(assetAddress);
        var op = Addressables.LoadAssetAsync<GameObject>(assetReference);
        asyncOperationHandles[assetAddress] = 
            new KeyValuePair<AssetReference, AsyncOperationHandle<GameObject>>(assetReference, op);
        op.Completed += (obj) =>
        {
            SpawnDragonMenuFromLoadedReference(assetReference, dragonData);
            
            if (queuedSpawnRequests.ContainsKey(assetReference))
            {
                while(queuedSpawnRequests[assetReference]?.Any() == true)
                {
                    DragonData queuedDragonData = queuedSpawnRequests[assetReference].Dequeue();
                    SpawnDragonMenuFromLoadedReference(assetReference, queuedDragonData);
                }
            }            
        };
    }

    private void SpawnDragonMenuFromLoadedReference(AssetReference assetReference, DragonData dragonData)
    {
        assetReference.InstantiateAsync(interim).Completed += (asyncOpHandle) =>
        {
            if (spawnedDragonMenus.ContainsKey(assetReference) == false)
            {
                spawnedDragonMenus[assetReference] = new List<GameObject>();
            }

            spawnedDragonMenus[assetReference].Add(asyncOpHandle.Result);
            DP_UIDragon dragon = asyncOpHandle.Result.GetComponent<DP_UIDragon>();
            StartCoroutine(UpdateDragonList(dragonData, dragon));
            dragonPrefabsGenerated++;
            var notify = asyncOpHandle.Result.AddComponent<UIDestroyNotif>();
            notify.Destroyed += Remove;
            notify.AssetReference = assetReference;
        };
    }

    private void Remove(AssetReference assetReference, UIDestroyNotif notif)
    {
        Addressables.ReleaseInstance(notif.gameObject);

        spawnedDragonMenus[assetReference].Remove(notif.gameObject);

        if(spawnedDragonMenus[assetReference].Count() == 0)
        {
            AsyncOperationHandle<GameObject> asyncOpHandle = asyncOperationHandles.Where(a => a.Value.Key == assetReference).FirstOrDefault().Value.Value;
            string assetAddress = asyncOperationHandles.Where(a => a.Value.Key == assetReference).FirstOrDefault().Key;

            if (asyncOpHandle.IsValid())
            {
                Addressables.Release(asyncOpHandle);
            }

            asyncOperationHandles.Remove(assetAddress);
        }
    }

    IEnumerator UpdateDragonList(DragonData dragonData, DP_UIDragon dragon)
    {
        if (dragon.uploadState != 0) { yield return null; }
        dragon.UpdateDragon(dragonData);
    }

    public void ClearSubPanel()
    {
        foreach (Transform child in interim)
        {
            Destroy(child.gameObject);
        }

        dragonPrefabsGenerated = 0;
    }
    */
    #endregion
}
