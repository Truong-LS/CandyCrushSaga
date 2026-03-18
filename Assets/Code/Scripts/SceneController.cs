using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadScene("MainMenu");
    }
    public void ChangeSceneMap(string scenceName)
    {
        SceneManager.LoadScene(scenceName);
    }
    public void PlayOnCurrentLevel()
    {
        int currentLevel = PlayerPrefs.GetInt("SavedLevel", 1);

        string sceneToLoad = "InGame_" + currentLevel;

        SceneManager.LoadScene(sceneToLoad);
    }
}