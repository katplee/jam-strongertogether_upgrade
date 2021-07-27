using UnityEngine;
using UnityEngine.EventSystems;


public class DP_UIDragon : UIObject, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    public int uploadState { get; set; } = 3;

    private DP_UIAvatar dragonAvatar;
    private DP_UIName dragonName;
    private DP_UIHP dragonHP;
    private DP_UIXP dragonXP;
    private DP_UIOpacity dragonOpacity;

    private DragonData dragonData = new DragonData();

    //check this
    public override string Label
    {
        get { return transform.tag; }
    }

    private void Awake()
    {
        SubscribeEvents();
    }

    private void OnDestroy()
    {
        UnsubscribeEvents();
    }

    private void SetAvatar(DP_UIAvatar avatar)
    {
        dragonAvatar = avatar;
        uploadState--;
    }

    private void SetName(DP_UIName name)
    {
        dragonName = name;
        uploadState--;
    }

    private void SetHP(DP_UIHP hp)
    {
        dragonHP = hp;
        uploadState--;
    }

    private void SetXP(DP_UIXP xp)
    {
        dragonXP = xp;
    }

    private void SetOpacity(DP_UIOpacity opacity)
    {
        dragonOpacity = opacity;
    }

    public void DeclareThis<T>(string element, T DP_UIObject)
        where T : MonoBehaviour
    {
        switch (element)
        {
            case "DP_UIAvatar":
                SetAvatar(DP_UIObject as DP_UIAvatar);
                break;

            case "DP_UIName":
                SetName(DP_UIObject as DP_UIName);
                break;

            case "DP_UIHP":
                SetHP(DP_UIObject as DP_UIHP);
                break;

            case "DP_UIXP":
                SetXP(DP_UIObject as DP_UIXP);
                break;

            case "DP_UIOpacity":
                SetOpacity(DP_UIObject as DP_UIOpacity);
                break;
        }
    }

    public void UpdateDragon(DragonData dragon)
    {
        dragonData = dragon;
        dragonAvatar.ChangeAvatar();
        dragonName.ChangeText(dragon.name);
        dragonHP.ChangeFillAmount(NormalHP(dragon.hp, dragon.maxHP));
        //dragonXP.ChangeFillAmount();
    }

    public float NormalHP(float hp, float maxHP)
    {
        if (maxHP == 0) { return 0; }

        float normalHP = hp / maxHP;
        return normalHP;
    }

    private void UpdateOpacity(bool active)
    {
        dragonOpacity.ChangeOpacity(active);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        UpdateOpacity(true);

        //change avatar and stats of player (like a preview)
        //call OnFusePreview from Fight Manager and pass dragonData!
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        //if there is a dragon selected, selected item will not change opacity
        if (UIDragonSubPanel.Instance.IsSelected) { return; }

        UpdateOpacity(false);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            UIDragonSubPanel.Instance.IsSelected = UIDragonSubPanel.Instance.IsSelected ^ true;
            UpdateOpacity(UIDragonSubPanel.Instance.IsSelected);

            if (UIDragonSubPanel.Instance.IsSelected) { PlayerSpriteLoader.Instance.SetSelection(); }
            else { PlayerSpriteLoader.Instance.ClearSelection(); }
        }
    }

    private void SendDragonData()
    {
        FightManager.Instance.PassDragonData(dragonData);
    }

    private void SubscribeEvents()
    {
        FightManager.OnDragonFuse += SendDragonData;
    }

    private void UnsubscribeEvents()
    {
        FightManager.OnDragonFuse -= SendDragonData;
    }
}
