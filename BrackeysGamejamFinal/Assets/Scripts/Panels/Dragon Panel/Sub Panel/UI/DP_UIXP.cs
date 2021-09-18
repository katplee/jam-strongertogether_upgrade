using UnityEngine;
using UnityEngine.UI;

public class DP_UIXP : UIObject
{
    private DP_UIDragon parent;

    private Image image;

    public override string Label
    {
        get { return GetType().Name; }
    }

    private void Start()
    {
        image = transform.GetChild(1).GetComponent<Image>();

        if (transform.parent.TryGetComponent(out parent))
        {
            parent.DeclareThis(Label, this);
        }
    }

    public void ChangeFillAmount(float hp, float maxHP)
    {
        image.fillAmount = hp / maxHP;
    }

    public void ChangeFillAmount(float normalHP)
    {
        image.fillAmount = normalHP;
    }
}
