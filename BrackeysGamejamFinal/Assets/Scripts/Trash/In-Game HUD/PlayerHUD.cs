using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHUD : MonoBehaviour
{
    private TPlayerData saved;

    //hp    
    public TMP_Text hpValueText;
    public Image hpBar;

    //armor
    public TMP_Text armorValueText;
    public Image armorBar;

    //dragons
    public TMP_Text dragonCountText;

    private void Update()
    {
        saved = TPlayerData.Instance;

        UpdateHP();
        UpdateArmor();
        UpdateDragons();
    }

    private void UpdateHP()
    {
        if (hpValueText.text == saved.playerHP.ToString()) { return; }

        hpValueText.text = saved.playerHP.ToString();
        hpBar.fillAmount = saved.playerHP / 100;
    }

    private void UpdateArmor()
    {
        if (armorValueText.text == saved.playerArmor.ToString()) { return; }

        armorValueText.text = saved.playerArmor.ToString();
        armorBar.fillAmount = saved.playerArmor / 100;
    }

    private void UpdateDragons()
    {
        if (dragonCountText.text == saved.dragonCount.ToString()) { return; }

        dragonCountText.text = saved.dragonCount.ToString();
    }
}
