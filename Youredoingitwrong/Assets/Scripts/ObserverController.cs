using UnityEngine;
using System.Collections;

public class ObserverController : MonoBehaviour
{
    public static ObserverController Instance;

    [Header("Refs")]
    public Transform playerTransform;
    public SpriteRenderer faceSprite;
    public DialogueDatabase db;

    [Header("Face Sprites")]
    public Sprite spriteFriendly;
    public Sprite spriteDoubt;
    public Sprite spriteJudgment;
    public Sprite spriteBlank;

    [Header("Positions (set in Inspector per level)")]
    public Transform positionEarly;   // near switch/solution
    public Transform positionLate;    // near wrong spot

    // ── Internal ───────────────────────────────────────────────────────────
    private ObserverMood mood = ObserverMood.Friendly;
    private float idleTimer;
    private bool idleFired;

    void Awake() => Instance = this;

    void Start()
    {
        int level = ValidationSystem.Instance.currentLevel;

        // Position: early game near correct spot, late game near wrong spot
        if (level >= 7 && positionLate != null) transform.position = positionLate.position;
        else if (positionEarly != null) transform.position = positionEarly.position;

        SetMood(level >= 9 ? ObserverMood.Blank : ObserverMood.Friendly);
    }

    void Update()
    {
        FacePlayer();
        TrackIdle();
    }

    // ── Always face player ─────────────────────────────────────────────────
    void FacePlayer()
    {
        if (playerTransform == null) return;
        float dx = playerTransform.position.x - transform.position.x;
        faceSprite.flipX = dx < 0;
    }

    // ── Idle detection → Observer reacts if player stands still ───────────
    void TrackIdle()
    {
        if (Input.anyKey) { idleTimer = 0f; idleFired = false; return; }

        idleTimer += Time.deltaTime;
        if (idleTimer >= 6f && !idleFired)
        {
            idleFired = true;
            FireIdleLine();
        }
    }

    void FireIdleLine()
    {
        var entry = db?.Get(ValidationSystem.Instance.currentLevel);
        if (entry?.onIdle != null)
            DialogueUI.Instance?.Show(entry.onIdle);
    }

    // ── Called by LevelManager after puzzle evaluated ──────────────────────
    public void ReactToValidation(ValidationResult result)
    {
        int level = ValidationSystem.Instance.currentLevel;

        // BODY LANGUAGE: mood driven by gameSaysCorrect, NOT truth
        ObserverMood bodyMood;
        if (level >= 9) bodyMood = ObserverMood.Blank;
        else if (result.gameSaysCorrect) bodyMood = level <= 3
                                              ? ObserverMood.Friendly
                                              : ObserverMood.Doubt;
        else bodyMood = level <= 6
                                              ? ObserverMood.Doubt
                                              : ObserverMood.Judgment;

        SetMood(bodyMood);

        // DIALOGUE: driven by gameSaysCorrect
        var entry = db?.Get(level);
        if (entry == null) return;

        DialogueLine line = result.gameSaysCorrect ? entry.onSolve : entry.onWrongAttempt;

        if (line != null && !string.IsNullOrWhiteSpace(line.text))
            DialogueUI.Instance?.Show(line);
    }

    // ── Mood → sprite ──────────────────────────────────────────────────────
    public void SetMood(ObserverMood m)
    {
        mood = m;
        faceSprite.sprite = m switch
        {
            ObserverMood.Friendly => spriteFriendly,
            ObserverMood.Doubt => spriteDoubt,
            ObserverMood.Judgment => spriteJudgment,
            ObserverMood.Blank => spriteBlank,
            _ => spriteFriendly
        };
    }

    // ── Late-game 1-frame position flicker ────────────────────────────────
    public IEnumerator FlickerPosition()
    {
        Vector3 origin = transform.position;
        transform.position += new Vector3(Random.Range(-0.06f, 0.06f), 0, 0);
        yield return null;
        transform.position = origin;
    }

    public void OnPlayerWrongAttempt()
    {
        int level = ValidationSystem.Instance.currentLevel;
        var entry = db?.Get(level);
        if (entry?.onWrongAttempt != null)
            DialogueUI.Instance?.Show(entry.onWrongAttempt);

        if (level >= 8) StartCoroutine(FlickerPosition());
    }
}