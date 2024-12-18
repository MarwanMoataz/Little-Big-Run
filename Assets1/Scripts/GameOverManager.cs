using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    public TMPro.TextMeshProUGUI collectiblesText; // UI for collectibles
    public TMPro.TextMeshProUGUI leaderboardText;  // UI for leaderboard


    private int totalCollectibles;                // Total collectibles
    private int finalScore;                       // Final score
    private string levelName = "Run";             // Level name

    private void Start()
    {
      
        totalCollectibles = PlayerPrefs.GetInt("TotalCollectibles", 0);
        int sessionCollectibles = GameManager.instance.GetSessionCollectibles();

        // Display session and total collectibles
        collectiblesText.text = $"Session Collectibles: {sessionCollectibles}\n" +
                                $"Total Collectibles: {totalCollectibles}";

        // Retrieve and display leaderboard
        int finalScore = GameManager.instance.GetSessionScore();
        UpdateLeaderboard(levelName, finalScore);
        DisplayLeaderboard(levelName);

       
    }

    private void UpdateLeaderboard(string levelName, int newScore)
    {
        // Get current leaderboard scores
        int score1 = PlayerPrefs.GetInt(levelName + "_Score1", 0);
        int score2 = PlayerPrefs.GetInt(levelName + "_Score2", 0);
        int score3 = PlayerPrefs.GetInt(levelName + "_Score3", 0);

        // Update leaderboard
        if (newScore > score1)
        {
            PlayerPrefs.SetInt(levelName + "_Score3", score2);
            PlayerPrefs.SetInt(levelName + "_Score2", score1);
            PlayerPrefs.SetInt(levelName + "_Score1", newScore);
        }
        else if (newScore > score2)
        {
            PlayerPrefs.SetInt(levelName + "_Score3", score2);
            PlayerPrefs.SetInt(levelName + "_Score2", newScore);
        }
        else if (newScore > score3)
        {
            PlayerPrefs.SetInt(levelName + "_Score3", newScore);
        }

        PlayerPrefs.Save();
    }

    private void DisplayLeaderboard(string levelName)
    {
        int score1 = PlayerPrefs.GetInt(levelName + "_Score1", 0);
        int score2 = PlayerPrefs.GetInt(levelName + "_Score2", 0);
        int score3 = PlayerPrefs.GetInt(levelName + "_Score3", 0);

        leaderboardText.text = $"Leaderboard\n1st: {score1}\n2nd: {score2}\n3rd: {score3}";
    }

    public void ReturnToMainMenu(string sceneName)
    {
        Debug.Log("Returning to Main Menu...");
        SceneManager.LoadScene(sceneName); // Replace 0 with your main menu scene index
    }

    public void RetryLevel(string sceneName)
    {
        Debug.Log("Retrying Level...");
        SceneManager.LoadScene(sceneName); // Replace 1 with your game level scene index
    }
}
