using UnityEngine;

public class Pushable : MonoBehaviour
{
    private Vector2Int gridPos;
    public float moveSpeed = 8f;
    private Vector2 targetPos;
    private bool isMoving;

    void Start()
    {
        gridPos = Vector2Int.RoundToInt(transform.position);
        targetPos = transform.position;
        GridManager.Instance.RegisterPushable(this, gridPos);
    }

    void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);
        if ((Vector2)transform.position == targetPos) isMoving = false;
    }

    public bool TryPush(Vector2Int newPos)
    {
        if (GridManager.Instance.IsWall(newPos)) return false;
        if (GridManager.Instance.GetPushable(newPos) != null) return false;

        Vector2Int oldPos = gridPos;
        gridPos = newPos;
        targetPos = new Vector2(newPos.x, newPos.y);
        isMoving = true;

        GridManager.Instance.MovePushable(oldPos, newPos, this);
        return true;
    }

    public Vector2Int GridPos => gridPos;
}