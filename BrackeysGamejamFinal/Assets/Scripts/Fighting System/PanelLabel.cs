using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelLabel : MonoBehaviour
{
    public int index;

    private void Start()
    {
        DeterminePanelIndex();
    }

    private void DeterminePanelIndex()
    {
        switch (gameObject.name.ToLower())
        {
            case "first":
                index = 1;
                break;            

            case "second":
                index = 2;
                break;

            case "third":
                index = 3;
                break;

            case "fourth":
                index = 4;
                break;

            case "fifth":
                index = 5;
                break;
        }
    }
}
