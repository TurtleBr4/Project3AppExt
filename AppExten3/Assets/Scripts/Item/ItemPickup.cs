using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public ItemData itemData;
    public int quantity = 1;
    GameManager inventory;

    private AudioSource source;
    public AudioClip pickupFX;

    private void Start()
    {
        inventory = GameObject.Find("GameManager").GetComponent<GameManager>();
        source = GetComponent<AudioSource>();
        source.clip = pickupFX;
        source.playOnAwake = false;
        source.loop = false;
        
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Attempting picking up an item");
        if (inventory != null && other.tag == "Player")
        {
            source.Play();
            inventory.addItemToInventory(itemData.itemID, quantity);
            Destroy(gameObject);
            Debug.Log("Picking up an item");
        }
    }
}
