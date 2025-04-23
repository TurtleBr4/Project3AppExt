using UnityEngine;

public class Hotbar
{
    public HotbarNode firstNode;
    private Inventory mainInventory;
    private const int MAX_SLOTS = 3;

    public Hotbar(Inventory inventory)
    {
        mainInventory = inventory;
        firstNode = null;
    }

    public int count()
    {
        int count = 0;
        HotbarNode temp = firstNode;
        while (temp != null)
        {
            count++;
            temp = temp.next;
        }
        return count;
    }

    public void assignToHotbar(ItemNode reference)
    {
        if (reference == null || count() >= MAX_SLOTS)
        {
            Debug.LogWarning("Hotbar is full or reference is null.");
            return;
        }

        if (firstNode == null)
        {
            firstNode = new HotbarNode(reference);
        }
        else
        {
            HotbarNode temp = firstNode;
            while (temp.next != null)
            {
                temp = temp.next;
            }
            temp.next = new HotbarNode(reference);
        }
    }

    public void useItem(int index, Inventory inventory)
    {
        HotbarNode temp = firstNode;
        HotbarNode prev = null;

        for (int i = 0; i < index && temp != null; i++)
        {
            prev = temp;
            temp = temp.next;
        }

        if (temp == null || temp.reference == null)
            return;

        temp.reference.setQuantity(temp.reference.getQuantity() - 1);

        if (temp.reference.getQuantity() <= 0)
        {
            // Find the index of the reference in the inventory and remove it
            int invIndex = inventory.getIndexOf(temp.reference);
            if (invIndex != -1)
            {
                inventory.removeItem(invIndex);
            }

            // Remove the hotbar node
            if (prev == null)
                firstNode = temp.next;
            else
                prev.next = temp.next;
        }
    }

    public void removeFromHotbar(int index)
    {
        HotbarNode temp = firstNode;
        HotbarNode prev = null;

        for (int i = 0; i < index && temp != null; i++)
        {
            prev = temp;
            temp = temp.next;
        }

        if (temp == null) return;

        if (prev == null)
        {
            firstNode = temp.next;
        }
        else
        {
            prev.next = temp.next;
        }
    }

    public bool ContainsReference(ItemNode reference)
    {
        HotbarNode temp = firstNode;
        while (temp != null)
        {
            if (temp.reference == reference)
                return true;
            temp = temp.next;
        }
        return false;
    }
}
