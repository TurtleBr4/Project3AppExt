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
    public Image[] inventorySlots;
    public Image[] equipSlots;
    public Image[] hotbarSlots;

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
        if (Input.GetKeyDown(KeyCode.Alpha1)){ //debugging feature
            if(toggleCamLocation){
                camsys.switchFocus(tempCameraLocation, 7, 5);
                toggleCamLocation = !toggleCamLocation;
            }
            else{
                camsys.switchFocus(camsys.playerTarget, 3, 5);
                toggleCamLocation = !toggleCamLocation;
            }
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
            inv.addItem(id, quant);
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
    }

    public void printInventoryToConsole() //for debugging
    {
        ItemNode temp = inv.firstNode;
        while (temp != null)
        {
            Debug.Log("Item: " + itemDatabase.GetItemById(temp.getID()).Name + ", Quantity: " + temp.getQuantity());
            temp = temp.next;
        }
    }

    void updateInventorySlots(int choice)
    {
        int i = 0;
        ItemNode temp = inv.firstNode;
        switch (choice)
        {
            case 0: //inventory menu slots
                while (temp != null && i < inventorySlots.Length)
                {
                    inventorySlots[i].sprite = itemDatabase.GetItemById(temp.getID()).Icon;
                    Debug.Log("Updating slot " + i);
                    i++;
                    temp = temp.next;
                }
                break;
            case 1: //hotbar slots
                while (temp != null && i < hotbarSlots.Length)
                {
                    hotbarSlots[i].sprite = itemDatabase.GetItemById(temp.getID()).Icon;
                    Debug.Log("Updating hotbar slot " + i);
                    i++;
                    temp = temp.next;
                }
                break;
            case 2: //equipable slots
                while (temp != null && i < equipSlots.Length)
                {
                    equipSlots[i].sprite = itemDatabase.GetItemById(temp.getID()).Icon;
                    Debug.Log("Updating slot " + i);
                    i++;
                    temp = temp.next;
                }
                break;
        }
       
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

}
