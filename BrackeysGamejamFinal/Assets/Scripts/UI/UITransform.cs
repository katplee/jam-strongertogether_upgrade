using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UITransform : UIObject
{
    private void Start()
    {
        Debug.Log($"{gameObject.name} setting...");

        TransformManager.Instance.DeclareThis(Label, transform);
    }
}
