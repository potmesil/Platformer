using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    private void Awake()
    {
        transform.Find("NewGameButton").GetComponent<Button>().onClick.AddListener(() =>
        {
            GameData.Delete();
            SceneManager.LoadScene(1);
        });

        transform.Find("QuitButton").GetComponent<Button>().onClick.AddListener(() =>
        {
            #if UNITY_EDITOR
				UnityEditor.EditorApplication.isPlaying = false;
            #else
				Application.Quit();
			#endif
        });

        var gameData = GameData.Load();
        if (gameData != null)
        {
            var loadGameBtn = transform.Find("LoadGameButton").GetComponent<Button>();
            loadGameBtn.interactable = true;
            loadGameBtn.onClick.AddListener(() => SceneManager.LoadScene(gameData.SceneIndex));
        }
    }
}