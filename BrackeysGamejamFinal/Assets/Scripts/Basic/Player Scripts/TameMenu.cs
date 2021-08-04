using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//210804_KAT: not referenced in any script

public class TameMenu : MonoBehaviour
{
    public Player player;
    public GameObject menu;
    private bool isChoosing = false;

    [Header("Buttons")]
    public GameObject fireDB;
    public GameObject fireDPrefab;

    public GameObject waterDB;
    public GameObject waterDPrefab;

    public GameObject earthDB;
    public GameObject earthDPrefab;

    public GameObject airDB;
    public GameObject airDPrefab;

    public GameObject basicDB;
    public GameObject basicDPrefab;

    public GameObject currDragon = null;

    void Start()
    {
        //menu.SetActive(false);
        //UpdateMenu();
    }

    public void UpdateMenu()
    {
        if (Inventory.Instance.hasFireD)
        {
            fireDB.SetActive(true);
        }
        else
        {
            fireDB.SetActive(false);
        }

        if (Inventory.Instance.hasWaterD)
        {
            waterDB.SetActive(true);
        }
        else
        {
            waterDB.SetActive(false);
        }

        if (Inventory.Instance.hasEarthD)
        {
            earthDB.SetActive(true);
        }
        else
        {
            earthDB.SetActive(false);
        }

        if (Inventory.Instance.hasAirD)
        {
            airDB.SetActive(true);
        }
        else
        {
            airDB.SetActive(false);
        }

        if (Inventory.Instance.hasBaseD)
        {
            basicDB.SetActive(true);
        }
        else
        {
            basicDB.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        /*
        if (Input.GetKeyDown(KeyCode.R))
        {
            if (isChoosing)
            {
                menu.SetActive(false);
                isChoosing = false;
                player.IsChoosingTame = false;
            }
            else
            {
                menu.SetActive(true);
                isChoosing = true;
                player.IsChoosingTame = true;
            }            
        }
        */
    }

    public void chooseDragon(int i)
    {
        if (currDragon != null)
        {
            Destroy(currDragon);
        }
        switch (i)
        {
            case 0:
                currDragon = Instantiate(basicDPrefab, transform);
                break;
            case 1:
                currDragon = Instantiate(fireDPrefab, transform);
                break;
            case 2:                
                currDragon = Instantiate(waterDPrefab, transform);
                break;
            case 3:
                currDragon = Instantiate(earthDPrefab, transform);
                break;
            case 4:
                currDragon = Instantiate(airDPrefab, transform);
                break;

        }
    }


}
