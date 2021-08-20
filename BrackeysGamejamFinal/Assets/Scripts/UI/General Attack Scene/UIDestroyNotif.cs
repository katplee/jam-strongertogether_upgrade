using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class UIDestroyNotif : MonoBehaviour
{
    public static event Action Destroyed;

    

    #region For addressables system
    /*
    //to be attached to prefabs instatiated using the addressable system

    public event Action<AssetReference, UIDestroyNotif> Destroyed;
    public AssetReference AssetReference { get; set; }

    private void OnDestroy()
    {
        Destroyed?.Invoke(AssetReference, this);    
    }
    */
    #endregion

}
