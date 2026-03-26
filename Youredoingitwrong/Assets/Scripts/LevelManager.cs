using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;

    [Header("Level Settings")]
    public int levelNumber = 1;          // Set this per scene
    public Door exitDoor;
    public float transitionDelay = 1.5f;

    [Header("Level 10 Special")]
    public bool isLevel10 = false;
    private float idleTime = 0f;
    private bool level10Solved = false;

    void Awake()
    {
        Instance = this;
        ValidationSystem.Instance.currentLevel = levelNumber;
    }

    void Update()
    {
        // Level 10: doing NOTHING for 5 seconds = winning
        if (isLevel10 && !level10Solved)
        {
            bool playerMoving = Input.anyKey;
            if (!playerMoving)
            {
                idleTime += Time.deltaTime;
                if (idleTime >= 5f) SolveLevel10();
            }
            else
            {
                idleTime = 0f; // reset if they try to do something
                DialogueUI.Instance?.Show("No.");
            }
        }
    }

    // Called by GridManager when all switches are pressed
    public void OnPuzzleSolved()
    {
        if (isLevel10) return; // Level 10 handled separately

        bool isCorrect = true; // Real logic: switches pressed = correct
        ValidationResult result = ValidationSystem.Instance.Evaluate(isCorrect);

        ObserverController.Instance?.ReactToValidation(result);

        if (result.isCorrectSolution) // Real logic always opens the door
        {
            exitDoor?.Open();
            StartCoroutine(LoadNextLevel());
        }
    }

    void SolveLevel10()
    {
        level10Solved = true;
        ObserverController.Instance?.SetMood(ObserverMood.Blank);
        StartCoroutine(LoadCredits());
    }

    IEnumerator LoadNextLevel()
    {
        yield return new WaitForSeconds(transitionDelay);
        int next = SceneManager.GetActiveScene().buildIndex + 1;
        if (next < SceneManager.sceneCountInBuildSettings)
            SceneManager.LoadScene(next);
        else
            SceneManager.LoadScene("Credits");
    }

    IEnumerator LoadCredits()
    {
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene("Credits");
    }
}