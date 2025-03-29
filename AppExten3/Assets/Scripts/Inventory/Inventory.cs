
public class Inventory
{
    public ItemNode firstNode;
    public ItemDatabase itemDatabase;

    //This script is JUST the linked list inventory itself, other behavior is handled in InventoryManager
    public Inventory(ItemDatabase database)
    {
        itemDatabase = database;
        firstNode = null; //assign the first node on start
    }

    public void addItem(int ID, int Amount) //adds item nodes to our list
    {
        if(firstNode == null)
        {
            firstNode = new ItemNode(ID, Amount);
        }
        else
        {
            ItemNode temp = firstNode;
            while(temp.next != null)
            {
                temp = temp.next;
            }
            temp.next = new ItemNode(ID, Amount);
        }
    }

    public void removeItem(int slotID)
    {
        if(firstNode == null)
        {
            return; //this should never be reached, but just in case, if we try and remove anything while we have nothing it will fail :D
        }
        if (slotID < 0 || slotID >= inventoryCount())
        {
            return; //first if block does null detection for the empty list and this one checks for invalid indexes
        }
        if (slotID == 0)
        {
            removeFirstItem();
            return; //call removing the first node if our index is 0
        }
        ItemNode temp = firstNode;
        for (int i = 0; i < slotID - 1; i++)
        {
            temp = temp.next;
        }
        temp.next = temp.next.next; //traverse to the index before, link the current (right before the index) to the one after
    }

    public int inventoryCount()
    {
        int listSize = 0;
        ItemNode temp = firstNode;
        while (temp != null)
        {
            listSize++;
            temp = temp.next;
        }
        return listSize; //counts up each time temp.next isnt null, returns the count
    }

    public void swapItems(int id1, int id2)
    {
        if (id1 == id2) { return; } //if the items are (somehow) the same then dont bother

        ItemNode prevX = null, currX = firstNode;
        while (currX != null && currX.getID() != id1)
        {
            prevX = currX;
            currX = currX.next;
        }

        ItemNode prevY = null, currY = firstNode;
        while (currY != null && currY.getID() != id2)
        {
            prevY = currY;
            currY = currY.next;
        }

        if (currX == null || currY == null) { return; }

        if (prevX != null)
        {
            prevX.next = currY;
        }
        else { firstNode = currY; }

        if (prevY != null)
        {
            prevY.next = currX;
        }
        else { firstNode = currX; }

        ItemNode temp = currX.next;
        currX.next = currY.next;
        currY.next = temp;
    }
    public void removeFirstItem()
    {
        if (firstNode == null)
        {
            return; //again, dont bother if somehow we reach this with nothing in the inventory
        }
        firstNode = firstNode.next; //moves the 2nd item
    }
}
