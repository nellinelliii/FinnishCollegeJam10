using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;

    [Header("Level Settings")]
    public int levelNumber = 1;
    public Door exitDoor;
    public float transitionDelay = 1.5f;

    [Header("Level 10 Special")]
    public bool isLevel10 = false;
    private float idleTime = 0f;
    private bool level10Solved = false;
    public bool isSolved = false;

    void Awake()
    {
        Instance = this;
        ValidationSystem.Instance.currentLevel = levelNumber;
    }

    void Update()
    {
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
                idleTime = 0f;
                DialogueUI.Instance?.Show("No.");
            }
        }
    }

    public void OnPuzzleSolved()
    {
        if (isLevel10) return;
        if (isSolved) return;
        isSolved = true;

        Debug.Log("Puzzle solved! Loading next level...");

        ValidationResult result = ValidationSystem.Instance.Evaluate(true);
        ObserverController.Instance?.ReactToValidation(result);

        exitDoor?.Open();
        StartCoroutine(LoadNextLevel());
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
        Debug.Log("Loading scene index: " + next);
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