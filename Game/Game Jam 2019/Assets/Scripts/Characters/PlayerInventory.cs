using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    private List<GameObject> itemsInInventory;
    public List<GameObject> ItemsInInventory
    {
        get
        {
            if(itemsInInventory == null)
            {
                itemsInInventory = new List<GameObject>();
            }
            return itemsInInventory;
        }
    }

    public void AddItemToInventory(GameObject item)
    {
        ItemsInInventory.Add(item);
    }
    public void RemoveItemFromInventory(GameObject item)
    {
        ItemsInInventory.Remove(item);
    }

    public bool HasItemInInventory(GameObject desiredItem)
    {
        Debug.Log(ItemsInInventory[0]);
        if(ItemsInInventory.Count > 0)
        {
            return ItemsInInventory.Where(obj => obj.Equals(desiredItem)).First() != null;
        }
        return false;
    }

    public GameObject GetItemInInventoryByTag(string tag)
    {
        return ItemsInInventory.Where(obj => obj.tag == tag).First();
    }

    public GameObject GetItemInInventoryByName(string name)
    {
        return ItemsInInventory.Where(obj => obj.name == name).First();
    }

    public List<GameObject> GetItemsInInventoryByTag(string tag)
    {
        return ItemsInInventory.Where(obj => obj.tag == tag).ToList();
    }

    public List<GameObject> GetItemsInInventoryByName(string name)
    {
        return ItemsInInventory.Where(obj => obj.name == name).ToList();
    }
}
