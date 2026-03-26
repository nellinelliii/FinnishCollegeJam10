using UnityEngine;

/// <summary>
/// The heart of the game.
/// isCorrectSolution  = real game logic (never lies)
/// gameSaysCorrect    = what the player is TOLD  (lies increasingly)
/// </summary>
public class ValidationSystem : MonoBehaviour
{
    public static ValidationSystem Instance;

    [Range(0, 10)]
    public int currentLevel = 1;

    void Awake() => Instance = this;

    // ── Core call ──────────────────────────────────────────────────────────
    public ValidationResult Evaluate(bool isCorrectSolution)
    {
        bool gameSaysCorrect = ComputeFakeFeedback(isCorrectSolution);

        return new ValidationResult
        {
            isCorrectSolution = isCorrectSolution,
            gameSaysCorrect = gameSaysCorrect
        };
    }

    // ── Fake feedback curve ────────────────────────────────────────────────
    bool ComputeFakeFeedback(bool truth)
    {
        // Levels 1-3:  always honest
        if (currentLevel <= 3) return truth;

        // Levels 4-6:  mostly honest, small doubt introduced
        if (currentLevel <= 6)
        {
            float lieChance = (currentLevel - 3) * 0.08f; // 8%, 16%, 24%
            if (Random.value < lieChance) return !truth;
            return truth;
        }

        // Levels 7-8:  INVERTED on correct solutions; wrong is sometimes praised
        if (currentLevel <= 8)
        {
            if (truth) return false;           // correct → told "wrong"
            return Random.value < 0.5f;         // wrong   → 50/50
        }

        // Level 9:  almost fully inverted
        if (currentLevel == 9)
        {
            return !truth;
        }

        // Level 10:  no solution exists; game always says wrong (until player does nothing)
        return false;
    }
}

// ── Result struct ──────────────────────────────────────────────────────────
public struct ValidationResult
{
    public bool isCorrectSolution;
    public bool gameSaysCorrect;
}