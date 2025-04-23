using UnityEngine;
public class Hotbar
{
    public ItemNode firstNode;
    private const int MAX_SLOTS = 3;

    public Hotbar()
    {
        firstNode = null;
    }

    public int count()
    {
        int count = 0;
        ItemNode temp = firstNode;
        while (temp != null)
        {
            count++;
            temp = temp.next;
        }
        return count;
    }

    public void assignToHotbar(ItemNode item)
    {
        if (item == null || count() >= MAX_SLOTS)
        {
            Debug.LogWarning("Hotbar is full or the item is null.");
            return;
        }

        if (firstNode == null)
        {
            firstNode = item;
        }
        else
        {
            ItemNode temp = firstNode;
            while (temp.next != null)
            {
                temp = temp.next;
            }
            temp.next = item;
        }
    }

    public void removeFromHotbar(int index)
    {
        if (index < 0 || index >= count()) return;

        // Traverse to the target index
        ItemNode temp = firstNode;
        for (int i = 0; i < index; i++)
        {
            temp = temp.next;
        }

        // Simply set the item at the given index to null
        if (temp != null)
        {
            temp = null;
        }
    }

    public void swapHotbarItems(int index1, int index2)
    {
        if (index1 == index2 || index1 < 0 || index2 < 0) return;

        ItemNode node1 = getNodeAt(index1);
        ItemNode node2 = getNodeAt(index2);

        if (node1 != null && node2 != null)
        {
            int tempID = node1.getID();
            int tempQuantity = node1.getQuantity();

            node1.setQuantity(node2.getQuantity());
            node1.setID(node2.getID());

            node2.setQuantity(tempQuantity);
            node2.setID(tempID);
        }
    }

    public void swapInventoryAndHotbarItems(int inventoryIndex, int hotbarIndex, Inventory inventory)
    {
        if (inventoryIndex < 0 || hotbarIndex < 0) return;

        ItemNode inventoryItem = inventory.getNodeById(inventoryIndex);
        ItemNode hotbarItem = getNodeAt(hotbarIndex);

        if (inventoryItem != null && hotbarItem != null)
        {
            // Swap items between hotbar and inventory
            inventory.swapItems(inventoryIndex, hotbarIndex);
        }
    }

    private ItemNode getNodeAt(int index)
    {
        int currentIndex = 0;
        ItemNode temp = firstNode;
        while (temp != null && currentIndex < index)
        {
            temp = temp.next;
            currentIndex++;
        }
        return temp;
    }

    public ItemNode getItemAt(int index)
    {
        ItemNode node = getNodeAt(index);
        return node != null ? node : null;
    }
}
