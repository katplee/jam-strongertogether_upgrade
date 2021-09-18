using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class KeyboardInputController : MonoBehaviour
{
    private KeyboardInputs keyboardInputs;

    //Game objects with input controls

    private void Awake()
    {
        keyboardInputs = new KeyboardInputs();
    }

    private void Start()
    {
    }

    private void OnEnable()
    {
        keyboardInputs.Enable();

        //press R to toggle the tame menu on and off
        keyboardInputs.TameMenu.PressR.performed += ShowTameMenu;
    }

    private void ShowTameMenu(InputAction.CallbackContext obj)
    {
        UITameMenu.Instance.UpdateTameMenuButtons();
        UITameMenu.Instance.ToggleVisibility();
    }

    private void OnDisable()
    {
        keyboardInputs.Disable();
    }

}

