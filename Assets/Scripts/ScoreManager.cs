using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    #region Simple Singleton

    public static ScoreManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    #endregion

    int totalScore;

    public void AddScore(int scoreToAdd)
    {
        totalScore += scoreToAdd;
    }

    public int GetScore()
    {
        return totalScore;
    }
}
