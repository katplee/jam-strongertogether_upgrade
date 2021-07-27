using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIButton : UIObject, IPointerDownHandler
{
    private Button button;

    public override string Label
    {
        get { return name; }
    }

    private void Start()
    {
        Debug.Log($"{name} setting...");

        button = GetComponent<Button>();
        button.interactable = false;

        ActionManager.Instance.DeclareThis(Label, this);
    }

    public void SetInteractability(bool value)
    {
        button.interactable = value;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        ActionManager.Instance.SendButtonResponse(Label);
    }
}
