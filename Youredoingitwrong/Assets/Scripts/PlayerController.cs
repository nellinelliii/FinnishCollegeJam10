using UnityEditor.EditorTools;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 8f;
    private bool isMoving;
    private Vector2 targetPos;
    private Vector3Int gridPos;

    void Start()
    {
        targetPos = transform.position;
        gridPos = Vector3Int.RoundToInt(transform.position);
    }

    void Update()
    {
        if (!isMoving)
            HandleInput();

        transform.position = Vector2.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);

        if ((Vector2)transform.position == targetPos)
            isMoving = false;
    }

    void HandleInput()
    {
        Vector2Int dir = Vector2Int.zero;

        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A)) dir = Vector2Int.left;
        else if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D)) dir = Vector2Int.right;
        else if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W)) dir = Vector2Int.up;
        else if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S)) dir = Vector2Int.down;

        if (dir == Vector2Int.zero) return;

        Vector2Int nextPos = new Vector2Int(gridPos.x + dir.x, gridPos.y + dir.y);

        // Check for wall
        if (GridManager.Instance.IsWall(nextPos)) return;

        // Check for door
        if (DoorManager.Instance != null && DoorManager.Instance.IsDoorAt(nextPos)) return;

        // Check for box
        Pushable box = GridManager.Instance.GetPushable(nextPos);
        if (box != null)
        {
            Vector2Int boxTarget = new Vector2Int(nextPos.x + dir.x, nextPos.y + dir.y);
            if (!box.TryPush(boxTarget)) return;
        }

        // Move player
        gridPos = new Vector3Int(nextPos.x, nextPos.y, 0);
        targetPos = new Vector2(gridPos.x, gridPos.y);
        isMoving = true;

        GridManager.Instance.OnPlayerMoved(nextPos);
    }
}