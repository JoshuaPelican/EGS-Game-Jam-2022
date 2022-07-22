using UnityEngine;
using TMPro;

public enum LevelState
{
    Menu,
    Start,
    Play,
    Pause,
    End
}

public class LevelManager : MonoBehaviour
{
    [SerializeField] LevelSettings LevelSettings;
    [Space]
    [SerializeField] Transform StartPoint;

    [Header("Variable References")]
    [SerializeField] IntVariable ScoreVariable;
    [SerializeField] IntVariable TimerVariable;

    [Header("Menu References")]
    [SerializeField] GameObject MenuCam;
    [SerializeField] GameObject EndPanel;
    [SerializeField] GameObject PausePanel;

    [Header("Variable Displays")]
    [SerializeField] GameObject Timer;
    [SerializeField] GameObject Score;
    [SerializeField] TextMeshProUGUI FinalStats;

    [Header("Tornado")]
    [SerializeField] GameObject TornadoPrefab;

    GameObject tornado;
    LevelState currentLevelState;


    private void Awake()
    {
        ChangeLevelState(LevelState.Menu);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.P))
        {
            ChangeLevelState(currentLevelState == LevelState.Pause ? LevelState.Play : LevelState.Pause);
        }
    }

    public void ChangeLevelState(LevelState levelState)
    {
        if(currentLevelState == LevelState.Pause)
        {
            //Unpause
            OnPause(false);
        }

        currentLevelState = levelState;

        switch (currentLevelState)
        {
            case LevelState.Menu:
                OnMenu(); break;
            case LevelState.Start:
                OnStart(); break;
            case LevelState.Play:
                OnPlay(); break;
            case LevelState.Pause:
                OnPause(true); break;
            case LevelState.End:
                OnEnd(); break;
        }
    }

    public void ChangeLevelState(int levelState)
    {
        ChangeLevelState((LevelState)levelState);
    }

    void OnMenu()
    {
        Time.timeScale = 0;
    }

    void OnStart()
    {
        ScoreVariable.Value = 0;

        Time.timeScale = 1;
        tornado = Instantiate(TornadoPrefab, StartPoint.position, StartPoint.rotation, transform);

        InitializeTornado();

        MenuCam.SetActive(false);
    }

    void OnPlay()
    {
        //
    }

    void InitializeTornado()
    {
        TornadoScaling scaling = tornado.GetComponentInChildren<TornadoScaling>();

        //Sets the base scale of the tornado using the starting scale
        scaling.SetBaseSize(LevelSettings.StartingScale);
        scaling.SetGrowthFactor(LevelSettings.SizeGrowthFactor);
    }

    void OnPause(bool pause)
    {
        Time.timeScale = pause ? 0 : 1;
        PausePanel.SetActive(pause);
    }

    void OnEnd()
    {
        float totalDestroyed = PhysicsObject.TotalDestroyedObjects / PhysicsObject.TotalObjects;
        Score.SetActive(false);
        Timer.SetActive(false);
        MenuCam.SetActive(true);

        Destroy(tornado);

        Time.timeScale = 0;
        EndPanel.SetActive(true);
        FinalStats.SetText($"Final Score\n{ScoreVariable.Value}\n\nTown Destroyed\n{Mathf.Round(totalDestroyed * 100)}%");
    }

    /*
    void CheckEndGameConditions()
    {
        float totalDestroyed = PhysicsObject.TotalDestroyedObjects / PhysicsObject.TotalObjects;

        if (TimerVariable.Value >= 0 && totalDestroyed < LevelSettings.DestroyedPercentCompletion)
            return;

        EndGame(totalDestroyed);
    }
    */
}
