using UnityEngine;
using UnityEngine.SceneManagement;

public class DifficultySelector : MonoBehaviour
{
    public void SelectNormalDifficulty()
    {
        PlayerPrefs.SetString("GameDifficulty", "Normal");
        LoadGameScene();
    }

    public void SelectHardDifficulty()
    {
        PlayerPrefs.SetString("GameDifficulty", "Hard");
        LoadGameScene();
    }

    private void LoadGameScene()
    {
        SceneManager.LoadScene("GameScene");
    }
}
