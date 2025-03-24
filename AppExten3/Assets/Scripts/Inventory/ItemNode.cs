public class ItemNode
{
    private int ID = 0;
    private int Quantity;
    public ItemNode next;
    public ItemNode(int id, int amt)
    {
        ID = id;
        Quantity = amt;
        next = null;
    }

    public int getQuantity() { return Quantity; }
    public void setQuantity(int val) { Quantity = val; }
    public int getID() { return ID; }
}
