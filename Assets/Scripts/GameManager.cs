using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] IntVariable ScoreVariable;
    [SerializeField] IntVariable TimerVariable;
    [SerializeField] [Range(0, 1)] float DestroyedPercentRequired = 0.95f;

    bool gameOver;

    private void FixedUpdate()
    {
        if(!gameOver)
            CheckEndGameConditions();
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
        Debug.Log("Game Over!");
    }
}
