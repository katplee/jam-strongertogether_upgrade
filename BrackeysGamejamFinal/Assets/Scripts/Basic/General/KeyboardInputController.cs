using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class KeyboardInputController : MonoBehaviour
{
    private KeyboardInputs keyboardInputs;

    private void Awake()
    {
        keyboardInputs = new KeyboardInputs();
    }

    private void OnEnable()
    {
        keyboardInputs.Enable();

        keyboardInputs.TameMenu.PressR.performed += ShowTameMenu;
        keyboardInputs.TameMenu.PressR.Enable();
    }

    

    private void ShowTameMenu(InputAction.CallbackContext obj)
    {
        Debug.Log("helloooo");
    }

    private void OnDisable()
    {
        keyboardInputs.Disable();
    }

}

