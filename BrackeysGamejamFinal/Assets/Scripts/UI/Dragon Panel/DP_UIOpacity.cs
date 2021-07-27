using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DP_UIOpacity : UIObject
{
    private DP_UIDragon parent;

    private Image image;
    private const byte inactiveOpacity = 100;
    private const byte activeOpacity = 0;

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

    public void ChangeOpacity(bool active)
    {        
        Color32 color = image.color;
        color.a = active? activeOpacity : inactiveOpacity;
        image.color = color;
    }
}
