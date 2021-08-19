using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIContainer : UIObject
{
    private UIInventory root;

    public override string Label
    {
        get { return GetType().Name; }
    }

    private void Awake()
    {
        if (transform.root.TryGetComponent(out root))
        {
            root.DeclareThis(Label, this);
        }
    }
}
