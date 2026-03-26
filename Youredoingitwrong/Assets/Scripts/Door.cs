using UnityEngine;
// ──────────────────────────────────────────
// DOOR
// ──────────────────────────────────────────
public class Door : MonoBehaviour
{
    public bool isOpen = false;
    private SpriteRenderer sr;

    void Awake() => sr = GetComponent<SpriteRenderer>();

    public void Open()
    {
        isOpen = true;
        Debug.Log("Door opened!");
        if (sr) sr.color = new Color(1, 1, 1, 0.2f);
        GetComponent<Collider2D>().enabled = false;
    }

    public void Close()
    {
        isOpen = false;
        if (sr) sr.color = Color.white;
        GetComponent<Collider2D>().enabled = true;
    }
}
