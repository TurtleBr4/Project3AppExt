using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public void goToScene(int sceneID){
        SceneManager.LoadScene(sceneID);
    }
}
