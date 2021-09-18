using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

[Serializable]
public class WrongPathException : Exception
{
    public override string Message
    {
        get { return "The path called was wrong."; }
    }

    public WrongPathException()
    {
    }
}
