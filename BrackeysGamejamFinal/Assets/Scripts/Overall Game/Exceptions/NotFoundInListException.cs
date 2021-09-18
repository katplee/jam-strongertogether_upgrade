using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

[Serializable]
public class NotFoundInListException : Exception
{
    public override string Message
    {
        get { return "The object is not in this list."; }
    }

    public NotFoundInListException()
    {
    }
}
