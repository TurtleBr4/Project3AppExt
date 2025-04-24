using UnityEngine;
using UnityEngine.SceneManagement;
public class LevelTransition : MonoBehaviour
{
    public SaveManager save;
    public GameManager game;

    private void Start()
    {
        save = GameObject.Find("SAVESYSTEM").GetComponent<SaveManager>();
        game = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            save.SaveGameToFile(game.player, game.getInventory(), game.getProgression());
            SceneManager.LoadScene(save.GetSavedSceneIndex() + 1);
        }
    }
}
