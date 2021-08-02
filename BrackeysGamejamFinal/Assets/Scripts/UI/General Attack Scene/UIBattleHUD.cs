using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIBattleHUD : UIObject
{
    public UIName elemName; //convert to private
    public UILevel elemLevel; //convert to private
    public UIHP elemHP; //convert to private
    public UIArmor elemArmor; //convert to private

    public override string Label
    {
        get { return transform.tag; }
    }

    private void Start()
    {
        Debug.Log($"{gameObject.name} setting...");

        HUDManager.Instance.DeclareThis(Label, this);
    }

    public void UpdateHUD<T>(T element)
        where T : Element
    {
        elemName.ChangeText(element.Type.ToString());
        //elemLevel.text = "Lvl " + element.armor.ToString();
        elemArmor.ChangeFillAmount(element.NormalArmor());
        elemHP.ChangeFillAmount(element.NormalHP());
    }

    public void UpdateHPArmor<T>(float hp, float armor, float maxHP, float maxArmor)
        where T : Element
    {
        //hpStatsBar.fillAmount = hp / maxHP;
        //armorStatsBar.fillAmount = armor / maxArmor;
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

    //to be called from the UI object being declared
    public void DeclareThis<T>(string element, T UIObject)
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
        }
    }
}
