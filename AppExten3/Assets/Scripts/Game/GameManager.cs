using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;

public class GameManager : MonoBehaviour
{
    public Player player;
    private Transform playerLocation;
    public bool isPaused = false;
    public GameObject[] panelsToSwitch;
    public int activePanel = -1; //-1 means no panel is active (i would do 0 but thats needed for indexes and whatnot) 
    [SerializeField]
    private int gameState = 0; //game states will use an id system, switch the number and you switch the current state
    private int activeScene;

    //Inventory variables
    public ItemDatabase itemDatabase;
    [SerializeField]
    private Inventory inv;
    private Hotbar hotbar; //our hotbar inventory, holds 3 items
    public Image[] inventorySlots;
    public Image[] equipSlots;
    public Image[] hotbarSlots;
    public RectTransform activeHotbarSlot;
    public float[] aHSPositions;
    public int activeItem = 0; //just a value to keep track of the active hotbar slot
    public Sprite emptySlotImage; //keep an image of an empty slot to replace once we remove an item

    //Dialogue Stuff
    private DialogueManager yapper;
    
    //Camera Stuff
    public CameraFollow camsys;
    public Transform tempCameraLocation;
    private bool toggleCamLocation;

    //Save Game Shenanagains
    bool savedFileExists;
    Vector3 lastPosition;
    int lastScene;
    private static int progression; //we'll update this int whenever big game events happen, like a checkpoint system. Certain things will only happen when this number is a certain value.

    public Sprite[] healthIcons;
    public Image healthDisplay;


    private void Awake()
    {
        DontDestroyOnLoad(gameObject); //this is our game manager, it would be annoying to manually add it in everywhere so just make sure it never leaves us
        //preload a save file if it exists to define savedFileExits
    }
    private void Start()
    {
        playerLocation = player.transform;
        yapper = GetComponent<DialogueManager>();
    }

    private void loadGameData()
    {
        //put the logic for loading here, thats gamestate 0
        if (!savedFileExists)
        {
            inv = new Inventory(itemDatabase);
            hotbar = new Hotbar();
            setGameState(1);
            return;
        }
        setGameState(1);
    }

    private void saveGameData(){
        lastPosition = playerLocation.position;
        lastScene = activeScene;

        //how write to JSON google dot com
    }

    private void Update()
    {
        switch (gameState)
        {
            case 0: //Load state, also the main menu state (this should only ever be called once!)
                loadGameData();
                break;
            case 1:
                //call any regular game state functions here
                if(Time.timeScale != 1)
                {
                    Time.timeScale = 1;
                }
                if (yapper.isYapping)
                {
                    setGameState(4);
                    return;
                }
                updateInventorySlots(1); //update the hotbar contiounously (look into listeners for more effeciency)
                playState();
                break;
            case 2:
                //logic for being in a menu
                Time.timeScale = 0;
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    switchActivePanel(-1); //close the pause menu
                    setGameState(1);
                }
                if (Input.GetKeyDown(KeyCode.E))
                {
                    switchActivePanel(-1); //close the pause menu
                    setGameState(1);
                }
                break;
            case 3:
                //scene transition logic
                
                break;
            case 4:
                //dialog state
                if (!yapper.isYapping) { setGameState(1); }
                if (Input.GetKeyDown(KeyCode.Mouse0)) 
                {
                    yapper.onNextLine();
                    Debug.Log("Onto the next line!");
                } //consider rewrite for more flexible controls
                break;
            default:
                //if by some genuinely impressive run of bad luck none of these state are active, logic to fix whatever mess caused that should go here
                break;
        }
    }

    void playState()
    {
        if (Input.GetKeyDown(KeyCode.E)) //inventory
        {
            Debug.Log("e pressed");
            if (activePanel == -1)
            {
                updateInventorySlots(0);
                switchActivePanel(2); //pull up the inventory
            }
            else if (activePanel == 2)
            {
                switchActivePanel(-1); //close the pause menu
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape)) //menu
        {
            Debug.Log("esc pressed");
            if (activePanel == -1)
            {
                switchActivePanel(0); //pull up the pause menu 
            }
            else
            {
                switchActivePanel(-1); //close the pause menu
            }
        }

        if (Input.GetKeyDown(KeyCode.Alpha1)){ //select the hotbar slot
            Vector2 newPosition = activeHotbarSlot.anchoredPosition;
            newPosition.x = aHSPositions[0];
            activeHotbarSlot.anchoredPosition = newPosition;
            activeItem = 0;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2)) {
            Vector2 newPosition = activeHotbarSlot.anchoredPosition;
            newPosition.x = aHSPositions[1];
            activeHotbarSlot.anchoredPosition = newPosition;
            activeItem = 1;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3)) {
            Vector2 newPosition = activeHotbarSlot.anchoredPosition;
            newPosition.x = aHSPositions[2];
            activeHotbarSlot.anchoredPosition = newPosition;
            activeItem = 2;
        }


        if (Input.GetKeyDown(KeyCode.Alpha8)){ //debugging feature
            if(toggleCamLocation){
                camsys.switchFocus(tempCameraLocation, 7, 5);
                toggleCamLocation = !toggleCamLocation;
                Debug.Log(Application.persistentDataPath);
            }
            else{
                camsys.switchFocus(camsys.playerTarget, 3, 5);
                toggleCamLocation = !toggleCamLocation;
                SaveGameToFile();
            }
        }

        if (Input.GetKeyDown(KeyCode.Tab)) { debugLoadFile(); }
        if (Input.GetKeyDown(KeyCode.Mouse1)) { useItem(activeItem); } //use the active item in the hotbar

        updateHealthDisplay();
    }

    void useItem(int index)
    {
        inv.removeItem(index);
        updateInventorySlots(1);
    }

    void updateHealthDisplay()
    {
        switch (player.Health)
        {
            case > 84:
                healthDisplay.sprite = healthIcons[0];
                break;
            case > 68:
                healthDisplay.sprite = healthIcons[1];
                break;
            case > 52:
                healthDisplay.sprite = healthIcons[2];
                break;
            case > 36:
                healthDisplay.sprite = healthIcons[3];
                break;
            case > 20:
                healthDisplay.sprite = healthIcons[4];
                break;
            case > 0:
                healthDisplay.sprite = healthIcons[5];
                break;
            default:
                healthDisplay.sprite = healthIcons[6];
                break;
        }
    }

    public void switchActivePanel(int panIndex)
    {
        if(getGameState() == 1 && panIndex != -1)
        {
            setGameState(2); //change the game state
            panelsToSwitch[panIndex].SetActive(true);
            activePanel = panIndex;
            Debug.Log("pausing");
        }
        else if (panIndex == -1)
        {
            panelsToSwitch[activePanel].SetActive(false);
            activePanel = panIndex;
            setGameState(1); //back to play mode
            Debug.Log("back to game");
        }
        else
        {
            panelsToSwitch[activePanel].SetActive(false);
            activePanel = panIndex;
            panelsToSwitch[panIndex].SetActive(true);
        }
        
    }

    int sceneSwitch(int sceneID)
    {
        setGameState(3); //scene switcher game state
        return sceneID; //the idea here is this method is called externally, and we use this to store the specific scene to go to (its 5 in the morning)
    }

    public void quitGame()
    {
        Debug.Log("Quitting...");
        Application.Quit(); //temporary, we have to save our game data at some point
    }

    public void setGameState(int i) { gameState = i; }
    public int getGameState() { return gameState; }
    public int getProgression() { return progression; }

    public void addItemToInventory(int id, int quant)
    {
        if (inv != null && itemDatabase != null)
        {
            ItemNode existing = inv.getNodeById(id);

            if (existing == null)
            {
                // Item is new to the inventory
                inv.addItem(id, quant);
                ItemNode newNode = inv.getNodeById(id); // Get the node that was just added
                hotbar.assignToHotbar(newNode); // Assign to hotbar
            }
            else
            {
                existing.setQuantity(existing.getQuantity() + quant);
            }

            ItemData item = itemDatabase.GetItemById(id);
            if (item != null)
            {
                Debug.Log($"Picked up {quant}x {item.Name}");
            }
        }
        else
        {
            Debug.LogError("Inventory or ItemDatabase is not assigned!");
        }

        printInventoryToConsole();
        updateInventorySlots(1); // Refresh hotbar
    }

    // Update UI for all slots
    public void UpdateUI()
    {
        updateInventorySlots(0);  // Inventory
        updateInventorySlots(1);  // Hotbar
        updateInventorySlots(2);  // Equipable Slots (if used)
    }

    // Print inventory to the console for debugging purposes
    public void printInventoryToConsole()
    {
        ItemNode temp = inv.firstNode;
        while (temp != null)
        {
            Debug.Log("Item: " + itemDatabase.GetItemById(temp.getID()).Name + ", Quantity: " + temp.getQuantity());
            temp = temp.next;
        }
    }

    // Update UI slots for a specific category (Inventory, Hotbar, Equipable)
    void updateInventorySlots(int choice)
    {
        int i = 0;
        ItemNode temp = inv.firstNode;

        switch (choice)
        {
            case 0: // Inventory slots
                while (temp != null && i < inventorySlots.Length)
                {
                    inventorySlots[i].sprite = itemDatabase.GetItemById(temp.getID()).Icon;
                    Debug.Log("Updating inventory slot " + i);
                    i++;
                    temp = temp.next;
                }
                break;

            case 1: // Hotbar slots
                ItemNode hotbarTemp = hotbar.firstNode;
                while (hotbarTemp != null && i < hotbarSlots.Length)
                {
                    hotbarSlots[i].sprite = itemDatabase.GetItemById(hotbarTemp.getID()).Icon;
                    Debug.Log("Updating hotbar slot " + i);
                    i++;
                    hotbarTemp = hotbarTemp.next;
                }
                break;

            case 2: // Equipable slots (if any)
                while (temp != null && i < equipSlots.Length)
                {
                    equipSlots[i].sprite = itemDatabase.GetItemById(temp.getID()).Icon;
                    Debug.Log("Updating equipable slot " + i);
                    i++;
                    temp = temp.next;
                }
                break;
        }
    }

    // Swap items between inventory slots
    public void swapInventoryItems(int invIndex1, int invIndex2)
    {
        inv.swapItems(invIndex1, invIndex2);
        UpdateUI();  // Update the UI to reflect the changes
    }

    // Swap items within the hotbar
    public void swapHotbarItems(int hotbarIndex1, int hotbarIndex2)
    {
        hotbar.swapHotbarItems(hotbarIndex1, hotbarIndex2);
        UpdateUI();  // Update the UI to reflect the changes
    }

    // Swap items between inventory and hotbar
    public void swapInventoryAndHotbarItems(int inventoryIndex, int hotbarIndex)
    {
        ItemNode inventoryItem = inv.getNodeById(inventoryIndex);
        ItemNode hotbarItem = hotbar.getItemAt(hotbarIndex);

        if (inventoryItem != null && hotbarItem != null)
        {
            // Swap the items between the inventory and hotbar
            inv.swapItems(inventoryIndex, hotbarIndex);
        }

        UpdateUI();  // Update the UI to reflect the changes
    }

    // Example of swapping hotbar and inventory
    public void swapHotbarToInventory(int hotbarIndex, int invIndex)
    {
        ItemNode hotbarItem = hotbar.getItemAt(hotbarIndex);
        ItemNode inventoryItem = inv.getNodeById(invIndex);

        if (hotbarItem != null && inventoryItem != null)
        {
            hotbar.swapHotbarItems(hotbarIndex, invIndex);
        }

        UpdateUI();
    }


    private string saveFile => Application.persistentDataPath + "/save.json";

    private void SaveGameToFile() {
        SaveData data = new SaveData();

        // Player data
        data.player = new PlayerSaveData {
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
        while (current != null) {
            data.inventory.items.Add(new InventoryItem {
                id = current.getID(),
                quantity = current.getQuantity()
            });
            current = current.next;
        }

        data.sceneIndex = SceneManager.GetActiveScene().buildIndex;
        data.progression = progression;

        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(saveFile, json);
        Debug.Log("Game saved.");
    }

    private bool LoadGameFromFile() {
        if (!File.Exists(saveFile)) {
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
        inv = new Inventory(itemDatabase); // clear and recreate
        foreach (var item in data.inventory.items) {
            inv.addItem(item.id, item.quantity);
        }

        // Optional: load scene and progression
        lastScene = data.sceneIndex;
        progression = data.progression;

        Debug.Log("Game loaded.");
        return true;
    }

    private void debugLoadFile()
    {
        LoadGameFromFile();
    }

}
