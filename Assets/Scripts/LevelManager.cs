using UnityEngine;
using TMPro;

public class LevelManager : MonoBehaviour
{
    [Header("Level Settings")]
    [SerializeField] float StartingScale = 1f;
    [SerializeField] float[] SizeThresholds = new float[6];
    [SerializeField][Range(0, 1)] float DestroyedPercentRequired = 0.95f;

    [Header("Variable References")]
    [SerializeField] IntVariable ScoreVariable;
    [SerializeField] IntVariable TimerVariable;

    [Header("Object References")]
    [SerializeField] GameObject TornadoPrefab;
    [SerializeField] GameObject MenuCam;
    [SerializeField] GameObject EndPanel;
    [SerializeField] GameObject Timer;
    [SerializeField] GameObject Score;
    [SerializeField] TextMeshProUGUI FinalStats;

    bool gameOver;
    GameObject tornado;

    private void Awake()
    {
        Time.timeScale = 0;
    }

    private void FixedUpdate()
    {
        if(!gameOver)
            CheckEndGameConditions();
    }

    public void StartGame()
    {
        ScoreVariable.Value = 0;

        Time.timeScale = 1;
        tornado = Instantiate(TornadoPrefab, transform.position, Quaternion.identity, transform);
        //Sets the base scale of the tornado using the starting scale
        tornado.GetComponentInChildren<TornadoScaling>().SetBaseSize(StartingScale);

        MenuCam.SetActive(false);
    }

    void CheckEndGameConditions()
    {
        float totalDestroyed = PhysicsObject.TotalDestroyedObjects / PhysicsObject.TotalObjects;

        if (TimerVariable.Value >= 0 && totalDestroyed < DestroyedPercentRequired)
            return;

        EndGame(totalDestroyed);
    }

    void EndGame(float totalDestroyed)
    {
        Score.SetActive(false);
        Timer.SetActive(false);
        gameOver = true;
        MenuCam.SetActive(true);
        Destroy(tornado);
        Debug.Log("Game Over!");
        Time.timeScale = 0;
        EndPanel.SetActive(true);
        FinalStats.SetText($"Final Score\n{ScoreVariable.Value}\n\nTown Destroyed\n{Mathf.Round(totalDestroyed * 100)}%");
    }
}
