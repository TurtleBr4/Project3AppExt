using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public bool isPaused = false;
    public GameObject[] panelsToSwitch;
    public int activePanel = -1; //-1 means no panel is active (i would do 0 but thats needed for indexes and whatnot) 

    [SerializeField]
    private TestPlayer player;
    [SerializeField]
    private int gameState = 0; //game states will use an id system, switch the number and you switch the current state

    //Inventory variables
    public ItemDatabase itemDatabase;
    [SerializeField]
    private Inventory inv;
    public Image[] inventorySlots;
    public Image[] equipSlots;
    public Image[] hotbarSlots;

    //Save Game Shenanagains
    bool savedFileExists;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject); //this is our game manager, it would be annoying to manually add it in everywhere so just make sure it never leaves us
    }
    private void Start()
    {
        //put the logic for loading here, thats gamestate 0

        if (!savedFileExists)
        {
            inv = new Inventory(itemDatabase);
        }

        gameState = 1; //unpaused, game is playing
    }

    private void Update()
    {
        switch (gameState)
        {
            case 0: //theoretically theres no way for this to ever be reached but JUST IN CASE (get it) we'll handle it anyway
                setGameState(1);
                break;
            case 1:
                //call any regular game state functions here
                if(Time.timeScale == 0)
                {
                    Time.timeScale = 1;
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
            default:
                //if by some genuinely impressive run of bad luck none of these state are active, logic to fix whatever mess caused that should go here
                break;
        }
    }

    void playState()
    {
        if (Input.GetKeyDown(KeyCode.E))
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

        if (Input.GetKeyDown(KeyCode.Escape))
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

    public void printInventoryToConsole()
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
}
