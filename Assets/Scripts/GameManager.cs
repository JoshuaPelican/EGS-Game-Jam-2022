using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] IntVariable ScoreVariable;
    [SerializeField] IntVariable TimerVariable;
    [SerializeField] [Range(0, 1)] float DestroyedPercentRequired = 0.95f;

    [SerializeField] GameObject TornadoPrefab;
    [SerializeField] GameObject MenuCam;
    [SerializeField] GameObject EndPanel;

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
        MenuCam.SetActive(false);
    }

    void CheckEndGameConditions()
    {
        float totalDestroyed = PhysicsObject.TotalDestroyedObjects / PhysicsObject.TotalObjects;

        if (TimerVariable.Value >= 0 && totalDestroyed < DestroyedPercentRequired)
            return;

        EndGame();
    }

    void EndGame()
    {
        gameOver = true;
        MenuCam.SetActive(true);
        Destroy(tornado);
        Debug.Log("Game Over!");
        Time.timeScale = 0;
        EndPanel.SetActive(true);
    }
}
