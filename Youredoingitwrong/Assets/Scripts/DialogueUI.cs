using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

[System.Serializable]
public class DialogueLine
{
    [TextArea] public string text;
    public float delayBefore = 0.7f;
    public float displayTime = 2.5f;
    public bool cutEarly = false;
    [Range(0f, 1f)]
    public float cutAtPercent = 0.6f;
}

[CreateAssetMenu(menuName = "YDIY/DialogueDatabase")]
public class DialogueDatabase : ScriptableObject
{
    [System.Serializable]
    public class LevelDialogue
    {
        public int levelNumber;
        public DialogueLine onSolve;
        public DialogueLine onWrongAttempt;
        public DialogueLine onIdle;
    }

    public LevelDialogue[] levels;

    public LevelDialogue Get(int level)
    {
        foreach (var l in levels)
            if (l.levelNumber == level) return l;
        return null;
    }
}

public class DialogueUI : MonoBehaviour
{
    public static DialogueUI Instance;

    [Header("Refs")]
    public TextMeshProUGUI label;
    public CanvasGroup group;

    [Header("Typewriter")]
    public float charDelay = 0.03f;

    private Coroutine active;

    void Awake() => Instance = this;

    public void Show(DialogueLine line)
    {
        if (active != null) StopCoroutine(active);
        active = StartCoroutine(Run(line));
    }

    public void Show(string text, float delayBefore = 0.7f, float displayTime = 2.5f)
    {
        Show(new DialogueLine
        {
            text = text,
            delayBefore = delayBefore,
            displayTime = displayTime
        });
    }

    IEnumerator Run(DialogueLine line)
    {
        group.alpha = 0f;
        label.text = "";

        yield return new WaitForSeconds(line.delayBefore);

        string full = line.text;
        int charLimit = line.cutEarly
            ? Mathf.FloorToInt(full.Length * line.cutAtPercent)
            : full.Length;

        group.alpha = 1f;

        for (int i = 0; i <= charLimit; i++)
        {
            label.text = full.Substring(0, i);
            yield return new WaitForSeconds(charDelay);
        }

        if (line.cutEarly)
            label.text += "—";

        yield return new WaitForSeconds(line.displayTime);

        float t = 0f;
        while (t < 0.4f)
        {
            t += Time.deltaTime;
            group.alpha = Mathf.Lerp(1f, 0f, t / 0.4f);
            yield return null;
        }
        group.alpha = 0f;
    }
}