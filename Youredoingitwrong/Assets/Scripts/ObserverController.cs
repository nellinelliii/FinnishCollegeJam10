using UnityEngine;
using System.Collections;

public class ObserverController : MonoBehaviour
{
    public static ObserverController Instance;

    [Header("Refs")]
    public Transform playerTransform;
    public SpriteRenderer faceSprite;
    public DialogueDatabase db;

    [Header("Positions (set in Inspector per level)")]
    public Transform positionEarly;
    public Transform positionLate;

    private ObserverMood mood = ObserverMood.Friendly;
    private float idleTimer;
    private bool idleFired;
    private float wrongCooldown = 0f;

    void Awake() => Instance = this;

    void Start()
    {
        int level = ValidationSystem.Instance.currentLevel;
        SetMood(level >= 9 ? ObserverMood.Blank : ObserverMood.Friendly);
    }

    void Update()
    {
        FacePlayer();
        TrackIdle();
        if (wrongCooldown > 0) wrongCooldown -= Time.deltaTime;
    }

    void FacePlayer()
    {
        if (playerTransform == null) return;
        if (faceSprite == null) return;
        float dx = playerTransform.position.x - transform.position.x;
        faceSprite.flipX = dx < 0;
    }

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

    public void ReactToValidation(ValidationResult result)
    {
        int level = ValidationSystem.Instance.currentLevel;

        ObserverMood bodyMood;
        if (level >= 9) bodyMood = ObserverMood.Blank;
        else if (result.gameSaysCorrect) bodyMood = level <= 3
                                              ? ObserverMood.Friendly
                                              : ObserverMood.Doubt;
        else bodyMood = level <= 6
                                              ? ObserverMood.Doubt
                                              : ObserverMood.Judgment;

        SetMood(bodyMood);

        var entry = db?.Get(level);
        if (entry == null) return;

        DialogueLine line = result.gameSaysCorrect ? entry.onSolve : entry.onWrongAttempt;
        if (line != null && !string.IsNullOrWhiteSpace(line.text))
            DialogueUI.Instance?.Show(line);
    }

    public void SetMood(ObserverMood m)
    {
        mood = m;
        if (faceSprite == null) return;
        // sprite switching — assign sprites in Inspector later
    }

    public void OnPlayerWrongAttempt()
    {
        if (wrongCooldown > 0) return;
        wrongCooldown = 4f;

        int level = ValidationSystem.Instance.currentLevel;
        var entry = db?.Get(level);
        if (entry?.onWrongAttempt != null)
            DialogueUI.Instance?.Show(entry.onWrongAttempt);

        if (level >= 8) StartCoroutine(FlickerPosition());
    }

    public IEnumerator FlickerPosition()
    {
        Vector3 origin = transform.position;
        transform.position += new Vector3(Random.Range(-0.06f, 0.06f), 0, 0);
        yield return null;
        transform.position = origin;
    }
}