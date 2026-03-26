using UnityEngine;

// ──────────────────────────────────────────
// SWITCH
// ──────────────────────────────────────────
public class Switch : MonoBehaviour
{
    [Header("Visuals")]
    public SpriteRenderer spriteRenderer;
    public Color offColor = Color.gray;
    public Color onColor = Color.yellow;

    private bool pressed;
    private Vector2Int gridPos;

    void Start()
    {
        gridPos = Vector2Int.RoundToInt(transform.position);
        GridManager.Instance.RegisterSwitch(this, gridPos);
        UpdateVisual();
    }

    public void SetPressed(bool state)
    {
        pressed = state;
        UpdateVisual();
    }

    void UpdateVisual()
    {
        if (spriteRenderer != null)
            spriteRenderer.color = pressed ? onColor : offColor;
    }

    public bool IsPressed => pressed;
}
