using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIBattleHUD : UIObject
{
    private static bool UIBattleHUDReady = false;

    private UIName elemName;
    private UILevel elemLevel;
    private UIHP elemHP;
    private UIArmor elemArmor;
    private Dictionary<string, UIValue> elemValue = new Dictionary<string, UIValue>();

    public override string Label
    {
        get { return transform.tag; }
    }

    private void Start()
    {
        Debug.Log($"{gameObject.name} setting...");

        //this method will only be called in the attack scene
        if (GameManager.currentSceneName != GameManager.attackScene)
        {
            UIBattleHUDReady = true;
            //update the HUD seen in the basic map scene
            StartCoroutine(UpdateBasicHUD());
        }

        //this method will only be called in the attack scene
        if (GameManager.currentSceneName == GameManager.attackScene)
            HUDManager.Instance.DeclareThis(Label, this);
    }

    public void UpdateHUD<T>(T element)
        where T : Element
    {
        if (elemName != null) { elemName.ChangeText(element.Type.ToString()); }
        //elemLevel.text = "Lvl " + element.armor.ToString();
        if (elemArmor != null) { elemArmor.ChangeFillAmount(element.NormalArmor(out _)); }
        if (elemHP != null) { elemHP.ChangeFillAmount(element.NormalHP(out _)); }
        if (elemValue.Count != 0) 
        {
            foreach(KeyValuePair<string, UIValue> pair in elemValue)
            {
                pair.Value.ChangeText(element);
            }
        }
    }

    public void UpdateHPArmor<T>(float hp, float armor, float maxHP, float maxArmor)
        where T : Element
    {
        //hpStatsBar.fillAmount = hp / maxHP;
        //armorStatsBar.fillAmount = armor / maxArmor;
    }

    IEnumerator UpdateBasicHUD()
    {
        if (!UIBattleHUDReady) { yield return null; }
        if (!Player.PlayerParamsReady) { yield return null; }
        UpdateHUD(Player.Instance);
    }

    private void SetName(UIName name)
    {
        if (name == null) { throw new HUDElementInvalidException(); }

        elemName = name;
    }

    private void SetLevel(UILevel level)
    {
        if (level == null) { throw new HUDElementInvalidException(); }

        elemLevel = level;
    }

    private void SetHP(UIHP hp)
    {
        if (hp == null) { throw new HUDElementInvalidException(); }

        elemHP = hp;
    }

    private void SetArmor(UIArmor armor)
    {
        if (armor == null) { throw new HUDElementInvalidException(); }

        elemArmor = armor;
    }

    private void SetValue(UIValue value, string name)
    {
        if (value == null) { throw new HUDElementInvalidException(); }

        if (elemValue.ContainsKey(name)) { throw new HUDElementInvalidException(); }

        elemValue.Add(name, value);
    }

    //to be called from the UI object being declared
    public void DeclareThis<T>(string element, T UIObject, string name = "")
        where T : MonoBehaviour
    {
        switch (element)
        {
            case "UILevel":
                SetLevel(UIObject as UILevel);
                break;

            case "UIName":
                SetName(UIObject as UIName);
                break;

            case "UIHP":
                SetHP(UIObject as UIHP);
                break;

            case "UIArmor":
                SetArmor(UIObject as UIArmor);
                break;

            case "UIValue":
                SetValue(UIObject as UIValue, name);
                break;
        }
    }
}
