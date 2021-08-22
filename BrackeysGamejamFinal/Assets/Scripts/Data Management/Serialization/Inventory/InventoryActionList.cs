using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryActionList : MonoBehaviour
{
    public void DeleteItem(Transform transform)
    {
        //decrease the count in the ui
        ItemCollected _item = transform.parent.GetComponent<ItemCollected>();
        _item.DecreaseItemAmount(1);

        //delete one of the items in the inventory data
        InventorySave.Instance.TrashItem(_item.GetItemType(), 1);
        InventorySave.Instance.SaveInventoryData();

        //update the item ui
        UIInventory.Instance.UpdateSpriteParameters(transform.parent, _item);
    }

    public void Hello()
    {

    }

    public void FireButtonResponse(ItemData itemData)
    {


        switch (itemData)
        {
            

            default:
                break;
        }
    }
}
