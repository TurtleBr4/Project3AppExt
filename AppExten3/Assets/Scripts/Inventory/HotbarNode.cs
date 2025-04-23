using UnityEngine;

public class HotbarNode 
{
    public ItemNode reference; //reference to main inventory node
    public HotbarNode next;

    public HotbarNode(ItemNode refNode)
    {
        reference = refNode;
        next = null;
    }
}
