using TMPro;
using UnityEngine;
using System;
using System.Collections;

public class UIValue : UIObject
{
    private UIBattleHUD parent;

    private TMP_Text text;

    public override string Label
    {
        get { return GetType().Name; }
    }

    private void Start()
    {
        text = GetComponent<TMP_Text>();

        if (transform.parent.TryGetComponent(out parent))
        {
            parent.DeclareThis(Label, this, name);
        }

        UpdateHUD();
    }
    
    private void UpdateHUD()
    {
        if(!FindObjectOfType<UIUpdater>()) { return; }

        UIUpdater.Instance.SetAnimParameter(0, name.ToLower().Replace(" ", "_"), 1);
    }

    public void ChangeText(Element element)
    {
        string paramName = SetParameterName();
        
        switch (paramName)
        {
            case "hp":
                element.NormalHP(out string hpString);
                text.text = hpString;
                break;

            case "armor":
                element.NormalArmor(out string armorString);
                text.text = armorString;
                break;

            default:
                break;
        }
    }

    private string SetParameterName()
    {
        int found = name.IndexOf(" Value");
        string paramName = name.Substring(0, found).ToLower();
        return paramName;
    }
}
