using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUDManager : MonoBehaviour
{
    private static HUDManager instance;
    public static HUDManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<HUDManager>();
            }
            return instance;
        }
    }

    public UIBattleHUD HPlayer { get; private set; }
    public UIBattleHUD HEnemy { get; private set; }

    private void SetHPlayer(UIBattleHUD HPlayer)
    {
        this.HPlayer = HPlayer;
    }

    private void SetHEnemy(UIBattleHUD HEnemy)
    {
        this.HEnemy = HEnemy;
    }

    public void DeclareThis(string tag, UIBattleHUD HUD)
    {
        switch (tag)
        {
            case "Player":
                SetHPlayer(HUD);
                break;

            case "Enemy":
                SetHEnemy(HUD);
                break;
        }
    }
}
