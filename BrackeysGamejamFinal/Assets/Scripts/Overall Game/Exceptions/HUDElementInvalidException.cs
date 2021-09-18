using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

[Serializable]
public class HUDElementInvalidException : Exception
{
    public override string Message
    {
        get { return "The HUD element you are trying to access was not declared properly."; }
    }
}
