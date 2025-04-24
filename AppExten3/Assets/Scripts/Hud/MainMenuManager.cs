using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public SaveManager save;
    private void Start()
    {
        save = GameObject.Find("SAVESYSTEM").GetComponent<SaveManager>();    
    }
    public void goToScene(int sceneID){
        SceneManager.LoadScene(sceneID);
    }

    public void loadGame()
    {
       if( save.GetSavedSceneIndex() != -1)
        {
            goToScene(save.GetSavedSceneIndex());
        }
    }

    public void quitGame()
    {
        Application.Quit();
    }

    public void newGame()
    {
        goToScene(2);
        save.newGame = true;
    }
}
