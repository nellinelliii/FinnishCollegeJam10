using UnityEngine;

public class DoorManager : MonoBehaviour
{
    public static DoorManager Instance;
    public Door door;
    private Vector2Int doorGridPos;

    void Awake()
    {
        Instance = this;
        doorGridPos = Vector2Int.RoundToInt(door.transform.position);
    }

    public bool IsDoorAt(Vector2Int pos)
    {
        if (door.isOpen) return false;
        return pos == doorGridPos;
    }
}