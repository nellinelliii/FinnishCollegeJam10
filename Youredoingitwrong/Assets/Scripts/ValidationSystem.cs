using UnityEngine;

public class ValidationSystem : MonoBehaviour
{
    public static ValidationSystem Instance;

    [Range(0, 10)]
    public int currentLevel = 1;

    void Awake() => Instance = this;

    public ValidationResult Evaluate(bool isCorrectSolution)
    {
        bool gameSaysCorrect = ComputeFakeFeedback(isCorrectSolution);
        return new ValidationResult
        {
            isCorrectSolution = isCorrectSolution,
            gameSaysCorrect = gameSaysCorrect
        };
    }

    bool ComputeFakeFeedback(bool truth)
    {
        // Levels 1-3: always honest
        if (currentLevel <= 3) return truth;

        // Levels 4-6: mostly honest, small doubt
        if (currentLevel <= 6)
        {
            float lieChance = (currentLevel - 3) * 0.08f;
            if (Random.value < lieChance) return !truth;
            return truth;
        }

        // Levels 7-9: correct → told wrong, wrong → told wrong too
        // onSolve always fires on solve, onWrongAttempt on wrong push
        // gameSaysCorrect only affects Observer mood/face
        if (currentLevel <= 9)
        {
            return false; // always says wrong — mood stays Judgment
        }

        // Level 10: always false
        return false;
    }
}

public struct ValidationResult
{
    public bool isCorrectSolution;
    public bool gameSaysCorrect;
}