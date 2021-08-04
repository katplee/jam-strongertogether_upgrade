using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIPanelButton : UIObject, IPointerDownHandler, IPointerEnterHandler
{
    private Button button;
    private Image dragonImage;
    private RectTransform rect;
    private static Vector3 currentSubPanelPosition = Vector3.zero;
    private int buttonIndex;

    InventoryData inventory = new InventoryData();
    private DragonType type;

    private const byte activeOpacity = 255;

    public override string Label
    {
        get { return name; }
    }

    private void Start()
    {
        button = GetComponent<Button>();
        dragonImage = GetComponent<Image>();
        rect = GetComponent<RectTransform>();

        //set the type of dragon in the panel tile
        SetDragonType();

        //set the interactability of the button based on the inventory
        InventorySave inventorySave = InventorySave.Instance.LoadInventoryData();
        inventory = inventorySave.inventory;

        if (inventory.CountTamedDragons(type) > 0)
        {
            SetInteractability(true);
        }

        //set the buttonIndex
        buttonIndex = transform.GetSiblingIndex() + 1;
    }

    public void SetInteractability(bool value)
    {
        //this means the dragon has been unlocked/tamed

        //set the interactability of the button
        button.interactable = value;

        //change the opacity of the image if interactable
        if (!value) { return; }

        Color32 color = dragonImage.color;
        color.a = activeOpacity;
        dragonImage.color = color;
    }

    private void SetDragonType()
    {
        int found = tag.IndexOf("Dragon");
        string element = tag.Substring(0, found).ToUpper();
        type = (DragonType)Enum.Parse(typeof(DragonType), element);
    }

    private void GenerateDragonList()
    {
        List<DragonData> chosenDragonList = inventory.SendTamedDragonList(type);
        UIDragonSubPanel.Instance.SetDragonType(type);
        UIDragonSubPanel.Instance.PassDragonList(chosenDragonList);
        UIDragonSubPanel.Instance.PrepareForDragonListGeneration();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
    /*
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            ActionManager.Instance.SendButtonResponse(Label);
        }
    */
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (UIDragonSubPanel.Instance.IsSelected) { return; }
        
        //show the subpanel with updated tamed dragon stats
        ShowSubPanel();

        if(inventory.CountTamedDragons(type) > 0)
        {
            //if the dragon is tamed, proceed to doing the preview animation for the player avatar
            FightManager.Instance.OnFusePreview(buttonIndex);
        }
    }

    private void ShowSubPanel()
    {
        if (currentSubPanelPosition == Vector3.zero)
        {
            currentSubPanelPosition = rect.position;
        }

        UIDragonSubPanel.Instance.ClearSubPanel();

        //change the static variable to the new button
        currentSubPanelPosition = rect.position;

        //send dragon type and list data to the subpanel
        GenerateDragonList();

        //send the world point of the center of the panel button to generate the subpanel
        Vector3 subPanelWithAllowance = rect.position + new Vector3(0f, -0.9f, 0f);
        UIDragonSubPanel.Instance.GenerateSubPanel(subPanelWithAllowance);

        //make subpanel active/inactive
        //bool newButton = !(rect.position == currentSubPanelPosition);
        UIDragonSubPanel.Instance.ToggleInteractability(true);

        //compute the world point of the mouse position - discontinued
        //Vector3 mousePosition = Camera.main.ScreenToWorldPoint(eventData.position);
    }
}
