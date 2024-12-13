using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelGoal : MonoBehaviour
{
    public string nextLevelName;
    public GameObject levelCompletePanel;

    private void Awake(){
        levelCompletePanel.SetActive(false);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("Level Completed!");
            ShowLevelComplete();
            LoadNextLevel();
        }
    }

    private void LoadNextLevel()
    {
        SceneManager.LoadScene(nextLevelName);
    }

        public void ShowLevelComplete()
    {
        levelCompletePanel.SetActive(true);
    }
}
