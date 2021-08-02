using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIObject : MonoBehaviour
{
    public virtual string Label
    {
        get { return transform.parent.tag; }
    }
}
