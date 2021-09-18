using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UIArmor : UIObject
{
    private UIBattleHUD parent;

    private Image image;

    public override string Label
    {
        get { return GetType().Name; }
    }

    private void Start()
    {
        image = GetComponent<Image>();

        if (transform.parent.TryGetComponent(out parent))
        {
            parent.DeclareThis(Label, this);
        }

        UpdateHUD();
    }

    private void UpdateHUD()
    {
        if(!FindObjectOfType<UIUpdater>()) { return; }

        UIUpdater.Instance.SetAnimParameter(0, name.ToLower().Replace(" ", "_"), 1);
    }

    public void ChangeFillAmount(float armor, float maxArmor)
    {
        image.fillAmount = armor / maxArmor;
    }

    public void ChangeFillAmount(float normalArmor)
    {
        image.fillAmount = normalArmor;
    }
}
