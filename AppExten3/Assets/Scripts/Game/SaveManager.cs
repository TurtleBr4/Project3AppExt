using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance { get; private set; }
    public ItemDatabase itemDatabase;
    private string saveFile => Application.persistentDataPath + "/save.json";
    private int savedSceneIndex = -1; // Store the saved scene index

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Persist across scenes
        }
    }

    // Retrieves the saved scene index from the save file
    public void RetrieveSavedSceneIndex()
    {
        savedSceneIndex = GetSavedSceneIndex(); // Get the saved scene index from file

        if (savedSceneIndex != -1)
        {
            Debug.Log("Saved scene index retrieved: " + savedSceneIndex);
        }
        else
        {
            Debug.Log("No saved scene found.");
        }
    }

    // Returns the saved scene index from the save file
    public int GetSavedSceneIndex()
    {
        if (!File.Exists(saveFile))
        {
            Debug.Log("No save file found.");
            return -1; // Return -1 to indicate no save file exists
        }

        string json = File.ReadAllText(saveFile);
        SaveData data = JsonUtility.FromJson<SaveData>(json);

        return data.sceneIndex; // Return the saved scene index
    }

    // Load the saved scene (call this when you're ready to load the scene)
    public void LoadSavedScene()
    {
        if (savedSceneIndex != -1)
        {
            Debug.Log("Loading saved scene: " + savedSceneIndex);
            SceneManager.LoadScene(savedSceneIndex); // Load the saved scene
        }
        else
        {
            Debug.Log("No saved scene index available to load.");
        }
    }

    // Save the game data to a file
    public void SaveGameToFile(Player player, Inventory inv, int progression)
    {
        SaveData data = new SaveData();

        // Player data
        data.player = new PlayerSaveData
        {
            health = player.Health,
            position = new float[] {
                player.transform.position.x,
                player.transform.position.y,
                player.transform.position.z
            }
        };

        // Inventory data
        data.inventory = new InventorySaveData();
        ItemNode current = inv.firstNode;
        while (current != null)
        {
            data.inventory.items.Add(new InventoryItem
            {
                id = current.getID(),
                quantity = current.getQuantity()
            });
            current = current.next;
        }

        // Scene and progression
        data.sceneIndex = SceneManager.GetActiveScene().buildIndex;
        data.progression = progression;

        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(saveFile, json);
        Debug.Log("Game saved.");
    }

    // Load game data from a file
    public bool LoadGameFromFile(ref Player player, ref Inventory inv, ref int progression)
    {
        if (!File.Exists(saveFile))
        {
            Debug.Log("No save file found.");
            return false;
        }

        string json = File.ReadAllText(saveFile);
        SaveData data = JsonUtility.FromJson<SaveData>(json);

        // Load player
        Vector3 pos = new Vector3(data.player.position[0], data.player.position[1], data.player.position[2]);
        player.transform.position = pos;
        player.Health = data.player.health;

        // Load inventory
        inv = new Inventory(itemDatabase); // Recreate the inventory
        foreach (var item in data.inventory.items)
        {
            inv.addItem(item.id, item.quantity);
        }

        // Load scene and progression
        progression = data.progression;

        Debug.Log("Game loaded.");
        return true;
    }
}