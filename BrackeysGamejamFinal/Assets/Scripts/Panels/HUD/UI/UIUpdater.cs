using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIUpdater : MonoBehaviour
{
    public static event Action OnHUDUpdate;

    private static UIUpdater instance;
    public static UIUpdater Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<UIUpdater>();
            }
            return instance;
        }
    }

    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void SetAnimParameter(int paramType, string paramName, int assignValue)
    {
        //0 is equivalent to bool
        //1 is equivalent to trigger
        //2 is equivalent to int
        //3 is equivalent to float

        switch (paramType)
        {
            case 0:
                if (assignValue != 0 && assignValue != 1) { break; }
                bool assignBool = (assignValue == 0) ? false : true;
                animator.SetBool(paramName, assignBool);
                break;
            case 1:
                animator.SetTrigger(paramName);
                break;
            case 2:
                animator.SetInteger(paramName, assignValue);
                break;
            case 3:
                animator.SetFloat(paramName, assignValue);
                break;
            default:
                break;
        }
    }
}
