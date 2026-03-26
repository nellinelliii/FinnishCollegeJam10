using UnityEngine;
using System.Collections.Generic;

public class GridManager : MonoBehaviour
{
    public static GridManager Instance;

    [Header("Level Elements")]
    public List<Vector2Int> wallPositions = new List<Vector2Int>();

    private Dictionary<Vector2Int, Pushable> pushables = new Dictionary<Vector2Int, Pushable>();
    private Dictionary<Vector2Int, Switch> switches = new Dictionary<Vector2Int, Switch>();

    void Awake()
    {
        Instance = this;
    }

    // Called by Pushable objects on Start to register themselves
    public void RegisterPushable(Pushable p, Vector2Int pos)
    {
        pushables[pos] = p;
    }

    public void MovePushable(Vector2Int from, Vector2Int to, Pushable p)
    {
        pushables.Remove(from);
        pushables[to] = p;
        CheckSwitches();
    }

    public void RegisterSwitch(Switch s, Vector2Int pos)
    {
        switches[pos] = s;
    }

    public bool IsWall(Vector2Int pos)
    {
        return wallPositions.Contains(pos);
    }

    public Pushable GetPushable(Vector2Int pos)
    {
        pushables.TryGetValue(pos, out Pushable p);
        return p;
    }

    public void OnPlayerMoved(Vector2Int pos)
    {
        // Could trigger observer reaction here
        //ObserverController.Instance?.OnPlayerMoved(pos);
    }

    void CheckSwitches()
    {
        bool allPressed = true;
        foreach (var kvp in switches)
        {
            bool pressed = pushables.ContainsKey(kvp.Key);
            kvp.Value.SetPressed(pressed);
            if (!pressed) allPressed = false;
        }

        if (allPressed && switches.Count > 0)
        {
            LevelManager.Instance.OnPuzzleSolved();
        }
    }
}