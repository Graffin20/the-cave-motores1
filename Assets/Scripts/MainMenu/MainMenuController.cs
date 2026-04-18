using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    [SerializeField] private string sceneName;

    public void ExitGame()
    {
        Application.Quit();
    }
    public void LoadMainGame()
    {
        SceneManager.LoadScene(sceneName);
    }
}
