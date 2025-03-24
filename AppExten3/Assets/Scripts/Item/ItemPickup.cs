using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public ItemData itemData;
    public int quantity = 1;
    GameManager inventory;

    private void Start()
    {
        inventory = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Attempting picking up an item");
        if (inventory != null && other.tag == "Player")
        {
            inventory.addItemToInventory(itemData.itemID, quantity);
            Destroy(gameObject);
            Debug.Log("Picking up an item");
        }
    }
}
