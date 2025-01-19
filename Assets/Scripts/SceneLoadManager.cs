using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoadManager : PersistentSingleton<SceneLoadManager>
{
    public string startSceneName = "StartScene";
    public int playerNum = 2;
    
    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
        if (AudioManager.Instance != null) AudioManager.Instance.StopMusic();
    }

    public void GoToStart()
    {
        LoadScene(startSceneName);
    }

    public void Reload()
    {
        LoadScene(SceneManager.GetActiveScene().name);
    }
    
    public void OnExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
