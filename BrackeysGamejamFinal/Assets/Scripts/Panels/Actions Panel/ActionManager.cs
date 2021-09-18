using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionManager : MonoBehaviour
{
    public static event Action<string> OnButtonPress;

    private static ActionManager instance;
    public static ActionManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<ActionManager>();
            }
            return instance;
        }
    }

    public UIButton AAttack { get; private set; }
    public UIButton ALeave { get; private set; }
    public UIButton AFuse { get; private set; }

    private void SetAAttack(UIButton button)
    {
        this.AAttack = button;
    }

    private void SetALeave(UIButton button)
    {
        this.ALeave = button;
    }

    private void SetAFuse(UIButton button)
    {
        this.AFuse = button;
    }

    public void SetAllInteractability(bool value)
    {
        AAttack.SetInteractability(value);
        ALeave.SetInteractability(value);
        AFuse.SetInteractability(value);
    }

    public void DeclareThis(string name, UIButton button)
    {
        switch (name)
        {
            case "Attack":
                SetAAttack(button);
                break;

            case "Leave":
                SetALeave(button);
                break;

            case "Fuse":
                SetAFuse(button);
                break;
        }
    }

    public void SendButtonResponse(string buttonName)
    {
        OnButtonPress?.Invoke(buttonName);
    }
}
