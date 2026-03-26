using UnityEngine;
using System.Collections;

    // ──────────────────────────────────────────────────────────────────────────
    // UI GLITCH — attach to Canvas or any RectTransform
    // Activates in late-game levels automatically
    // ──────────────────────────────────────────────────────────────────────────
    public class UIGlitch : MonoBehaviour
{
    [Header("Thresholds")]
    public int glitchStartLevel = 7;
    public float maxPixelShift = 2f;   // subtle: 1-2px
    public float glitchInterval = 4f;

    private RectTransform rt;
    private Vector2 originalPos;
    private float timer;

    void Start()
    {
        rt = GetComponent<RectTransform>();
        originalPos = rt.anchoredPosition;
    }

    void Update()
    {
        int level = ValidationSystem.Instance?.currentLevel ?? 1;
        if (level < glitchStartLevel) return;

        timer += Time.deltaTime;
        if (timer >= glitchInterval)
        {
            timer = 0f;
            StartCoroutine(OneFrameShift());
        }
    }

    IEnumerator OneFrameShift()
    {
        float shift = Random.Range(1f, maxPixelShift);
        rt.anchoredPosition = originalPos + new Vector2(shift, 0);
        yield return null; // exactly one frame
        rt.anchoredPosition = originalPos;
    }
}