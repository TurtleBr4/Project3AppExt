using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "ItemDatabase", menuName = "Scriptable Objects/ItemDatabase")]
public class ItemDatabase : ScriptableObject
{
    public List<ItemData> items = new List<ItemData>(); //list of every item in the game

    private Dictionary<int, ItemData> itemLookup;

    void OnEnable() //go through every item on our list and put in in our dictionary for effecient lookups
    {
        itemLookup = new Dictionary<int, ItemData>();
        foreach (var item in items)
        {
            itemLookup[item.itemID] = item;
        }
    }

    public ItemData GetItemById(int id) //pass in an id, return item data
    {
        return itemLookup.TryGetValue(id, out var item) ? item : null;
    }
}
