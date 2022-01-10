using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// THis is a simple script to handle start scene buttons
/// </summary>
public class StartManager : MonoBehaviour
{
    public void StartGame(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }


    public void ExitGame()
    {
        Application.Quit();
    }
}
