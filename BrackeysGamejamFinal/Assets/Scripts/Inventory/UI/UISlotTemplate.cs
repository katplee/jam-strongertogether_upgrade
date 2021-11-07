using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISlotTemplate : UIObject
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
            if (!transform.parent.GetComponent<UIContainer>())
            {
                root.DeclareThis(Label, this);
            }
        }
        
        transform.localScale = Vector3.zero;
    }
}
