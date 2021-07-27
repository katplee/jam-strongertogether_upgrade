using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TamingReqs : MonoBehaviour
{
    [Header("Taming System")]
    public int pStones = 2;
    
    ///refers to the panel containing the reqts and all
    ///this is causing all the errors.
    ///it suddenly disappears and reappears on the prefab
    ///should we make this private?
    /// vvvvvvvvvvvvvvvvvvvvvvvvv
    public GameObject pStonesReq;
    
    public Dragon dragon; //refers to the game object's Dragon.cs script

    [Header("UI")]    
    public Text reqText;
    public Sprite reqStone;
    public Image imgStone;

    void Start()
    {
        //hides the reqts
        if (pStonesReq != null)
        {
            pStonesReq.SetActive(false);
            reqText.text = pStones.ToString();
            imgStone.sprite = reqStone;
        }               
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        string tag = collision.gameObject.tag;

        InventorySave inventorySave = InventorySave.Instance.LoadInventoryData();
        InventoryData inventory = inventorySave.inventory;

        int type = (int)dragon.DType;
        StoneType element = (StoneType)type;

        if (tag == "Player")
        {
            if (inventory.CountCollectedStones(element) >= pStones)
            {
                //use the required pStones and save inventory data
                InventorySave.Instance.UseStone(element, pStones);
                InventorySave.Instance.SaveInventoryData();

                //increase dragon count
                InventorySave.Instance.AddDragon();

                //change dragon data property isTame to true
                dragon.TameDragon();
                dragon.ResaveThisDragon();
                
                //destroy the dragon's game object
                Destroy(gameObject);
            }
            else
            {
                pStonesReq.SetActive(true);
            }
        }
        
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        string tag = collision.gameObject.tag;

        if (tag == "Player")
        {
            pStonesReq.SetActive(false);
        }
    }

    /*
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            switch (dragon.DType)
            {
                case DragonType.BASE:
                    if (Inventory.Instance.baseStones >= pStones)
                    {
                        Inventory.Instance.baseStones -= pStones; //kat added this!
                        OnDragonCapture();
                        return;
                    }
                    break;

                case DragonType.FIRE:
                    if (Inventory.Instance.fireStones >= pStones)
                    {
                        Inventory.Instance.fireStones -= pStones; //kat added this!
                        OnDragonCapture();
                        return;
                    }
                    break;

                case DragonType.WATER:
                    if (Inventory.Instance.waterStones >= pStones)
                    {
                        Inventory.Instance.waterStones -= pStones; //kat added this!
                        OnDragonCapture();
                        return;
                    }
                    break;

                case DragonType.AIR:
                    if (Inventory.Instance.airStones >= pStones)
                    {
                        Inventory.Instance.airStones -= pStones; //kat added this!
                        OnDragonCapture();
                        return;
                    }
                    break;

                case DragonType.EARTH:
                    if (Inventory.Instance.earthStones >= pStones)
                    {
                        Inventory.Instance.earthStones -= pStones;
                        OnDragonCapture();
                        return;
                    }
                    break;
            }

            //if the stones are not enough, just display the reqts panel
            pStonesReq.SetActive(true);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            pStonesReq.SetActive(false);
        }
    }

    private void OnDragonCapture()
    {
        //add the dragon to the inventoru
        Inventory.Instance.AddDragon(dragon);

        //destroy the dragon in the scene
        Destroy(gameObject);
    }
    */
}
