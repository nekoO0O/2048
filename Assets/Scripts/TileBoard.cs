using GameModule;
using InputModule;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileBoard : MonoBehaviour
{
    public GameManager gameManager;
    public Tile tilePrefab;
    public TileState[] tileStates;

    private TileGrid _grid;
    private List<Tile> _tiles;
    private bool _waiting; //等待动画结束，避免发生漂移等问题

    private void Awake()
    {
        _grid = GetComponentInChildren<TileGrid>();
        _tiles = new List<Tile>(16);
    }

    public void ClearBoard()
    {
        foreach (var cell in _grid.cells)
        {
            cell.tile = null;
        }

        foreach (var tile in _tiles)
        {
            Destroy(tile.gameObject);
        }

        _tiles.Clear();
    }

    /// <summary>
    /// 创建tile
    /// </summary>
    public void CreateTile(int num)
    {
        for (int i = 0; i < num; i++)
        {
            // 通过 tilePrefab 实例化一个 tile 在 grid.transform 的位置
            Tile tile = GameObject.Instantiate(tilePrefab, _grid.transform);

            // 创建资源
            tile.SetState(tileStates[0]);

            // 产生 tile 到随机空的的 cell 上
            tile.Spawn(_grid.GetRandomEmptyCell());
            _tiles.Add(tile);
        }
    }

    /// <summary>
    /// 接受用户输入
    /// </summary>
    private void Update()
    {
        if (!_waiting)
        {
            if (InputManager.Instance.inputControls.Game.Up.WasPressedThisFrame())
            {
                Move(Vector2Int.up, 0, 1, 1, 1);
            }
            else if (InputManager.Instance.inputControls.Game.Down.WasPressedThisFrame())
            {
                Move(Vector2Int.down, 0, 1, _grid.height - 2, -1);
            }
            else if (InputManager.Instance.inputControls.Game.Left.WasPressedThisFrame()) 
            {
                Move(Vector2Int.left, 1, 1, 0, 1);
            }
            else if (InputManager.Instance.inputControls.Game.Right.WasPressedThisFrame())
            {
                Move(Vector2Int.right, _grid.width - 2, -1, 0, 1);
            }
        }
    }

    /// <summary>
    /// 移动
    /// </summary>
    /// <param name="direction"></param>
    /// <param name="startX"></param>
    /// <param name="incrementX"></param>
    /// <param name="startY"></param>
    /// <param name="incrementY"></param>
    private void Move(Vector2Int direction, int startX, int incrementX, int startY, int incrementY)
    {
        bool changed = false;
        /*
         2 0 0 2
         0 2 0 2
         0 0 0 0
         2 0 2 0
         */
        for (int x = startX; x >= 0 && x < _grid.width; x += incrementX)
        {
            for (int y = startY; y >= 0 && y < _grid.height; y += incrementY)
            {
                TileCell cell = _grid.GetCell(x, y);

                if (cell.occupied)
                {
                    changed |= MoveTile(cell.tile, direction);
                }
            }
        }

        if (changed)
        {
            StartCoroutine(WaitForChanges()); // 等待动画完成
        }
    }

    /// <summary>
    /// 移动和合并tile，返回bool，表示是否移动
    /// </summary>
    /// <param name="tile"></param>
    /// <param name="direction"></param>
    /// <returns></returns>
    private bool MoveTile(Tile tile, Vector2Int direction)
    {
        TileCell newCell = null;
        TileCell adjacent = _grid.GetAdjacentCell(tile.cell, direction);

        while (adjacent != null)
        {
            if (adjacent.occupied)
            {
                // 合并
                if (CanMerge(tile, adjacent.tile))
                {
                    MergeTiles(tile, adjacent.tile);
                    return true;
                }

                break;
            }

            newCell = adjacent;
            adjacent = _grid.GetAdjacentCell(adjacent, direction);
        }

        if (newCell != null)
        {
            tile.MoveTo(newCell);
            return true;
        }

        return false;
    }

    private bool CanMerge(Tile a, Tile b)
    {
        return a.state == b.state && !b.locked;
    }

    private void MergeTiles(Tile a, Tile b)
    {
        _tiles.Remove(a);
        a.Merge(b.cell);

        int index = Mathf.Clamp(IndexOf(b.state) + 1, 0, tileStates.Length - 1);
        TileState newState = tileStates[index];

        b.SetState(newState);
        gameManager.IncreaseScore(newState.number);
    }

    /// <summary>
    /// 获取资源的索引，在更换资源时使用
    /// </summary>
    /// <param name="state"></param>
    /// <returns></returns>
    private int IndexOf(TileState state)
    {
        for (int i = 0; i < tileStates.Length; i++)
        {
            if (state == tileStates[i])
            {
                return i;
            }
        }

        return -1;
    }

    private IEnumerator WaitForChanges()
    {
        _waiting = true;

        yield return new WaitForSeconds(0.1f);

        _waiting = false;

        foreach (var tile in _tiles)
        {
            tile.locked = false;
        }

        if (_tiles.Count != _grid.size)
        {
            CreateTile(1);
        }

        if (CheckForGameOver())
        {
            gameManager.GameOver();
        }
    }

    public bool CheckForGameOver()
    {
        if (_tiles.Count != _grid.size)
        {
            return false;
        }

        foreach (var tile in _tiles)
        {
            TileCell up = _grid.GetAdjacentCell(tile.cell, Vector2Int.up);
            TileCell down = _grid.GetAdjacentCell(tile.cell, Vector2Int.down);
            TileCell left = _grid.GetAdjacentCell(tile.cell, Vector2Int.left);
            TileCell right = _grid.GetAdjacentCell(tile.cell, Vector2Int.right);

            if (up != null && CanMerge(tile, up.tile))
            {
                return false;
            }

            if (down != null && CanMerge(tile, down.tile))
            {
                return false;
            }

            if (left != null && CanMerge(tile, left.tile))
            {
                return false;
            }

            if (right != null && CanMerge(tile, right.tile))
            {
                return false;
            }
        }

        return true;
    }
}