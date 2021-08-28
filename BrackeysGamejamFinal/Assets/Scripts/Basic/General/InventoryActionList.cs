using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryActionList : MonoBehaviour
{
    public void DeleteItem(Transform itemParentTransform)
    {
        //decrease the count in the ui
        ItemCollected _item = itemParentTransform.GetComponent<ItemCollected>();
        _item.DecreaseItemAmount(1);

        //delete one of the items in the inventory data
        InventorySave.Instance.TrashItem(_item.GetItemType(), 1);
        InventorySave.Instance.SaveInventoryData();

        //update the item ui
        UIInventory.Instance.UpdateSpriteParameters(itemParentTransform, _item);
    }

    public void Hello()
    {
        Debug.Log("Hello");
    }

    public void FireButtonResponse(ItemCollected item)
    {
        switch (item.GetItemType())
        {
            case ItemType.None:
                break;

            case ItemType.Square:
                StartCoroutine(MakeSpriteFlash(Color.green, Player.Instance.gameObject));
                Debug.Log("I used a square!");
                break;

            case ItemType.Circle:
                StartCoroutine(MakeSpriteFlash(Color.blue, Player.Instance.gameObject));
                Debug.Log("I used a circle!");
                break;

            case ItemType.Diamond:
                StartCoroutine(MakeSpriteFlash(Color.red, Player.Instance.gameObject));
                Debug.Log("I used a diamond!");
                break;

            default:
                break;
        }

        DeleteItem(item.transform);
    }

    IEnumerator MakeSpriteFlash(Color32 color, GameObject gameObject)
    {
        SpriteRenderer spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        Color32 current = spriteRenderer.color;

        spriteRenderer.color = color;
        yield return new WaitForSeconds(0.1f);
        spriteRenderer.color = current;
    }
}
