using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileBoard : MonoBehaviour
{
    public GameManager gameManager;
    public Tile tilePrefab;
    public TileState[] tileStates;

    private TileGrid grid;
    private List<Tile> tiles;
    private bool waiting;//�ȴ��������������ⷢ��Ư�Ƶ�����

    private void Awake()
    {
        grid = GetComponentInChildren<TileGrid>();
        tiles = new List<Tile>(16);
    }

    public void ClearBoard()
    {
        foreach (var cell in grid.cells)
        {
            cell.tile = null;
        }

        foreach (var tile in tiles)
        {
            Destroy(tile.gameObject);
        }

        tiles.Clear();
    }

    /// <summary>
    /// ����tile
    /// </summary>
    public void CreateTile()
    {
        // ͨ��tilePrefabʵ����һ��tile��grid.transform��λ��
        Tile tile = Instantiate(tilePrefab, grid.transform);
        // ������Դ
        tile.SetState(tileStates[0]);
        // ����tile������յĵ�cell��
        tile.Spawn(grid.GetRandomEmptyCell());
        tiles.Add(tile);
    }

    /// <summary>
    /// �����û�����
    /// </summary>
    private void Update()
    {
        if (!waiting)
        {
            if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
            {
                Move(Vector2Int.up, 0, 1, 1, 1);
            }
            else if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
            {
                Move(Vector2Int.left, 1, 1, 0, 1);
            }
            else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
            {
                Move(Vector2Int.down, 0, 1, grid.height - 2, -1);
            }
            else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
            {
                Move(Vector2Int.right, grid.width - 2, -1, 0, 1);
            }
        }
    }

    /// <summary>
    /// �ƶ�
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
        for (int x = startX; x >= 0 && x < grid.width; x += incrementX)
        {
            for (int y = startY; y >= 0 && y < grid.height; y += incrementY)
            {
                TileCell cell = grid.GetCell(x, y);

                if (cell.occupied)
                {
                    changed |= MoveTile(cell.tile, direction);
                }
            }
        }

        if (changed)
        {
            StartCoroutine(WaitForChanges());//�ȴ��������
        }
    }

    /// <summary>
    /// �ƶ��ͺϲ�tile������bool����ʾ�Ƿ��ƶ�
    /// </summary>
    /// <param name="tile"></param>
    /// <param name="direction"></param>
    /// <returns></returns>
    private bool MoveTile(Tile tile, Vector2Int direction)
    {
        TileCell newCell = null;
        TileCell adjacent = grid.GetAdjacentCell(tile.cell, direction);

        while (adjacent != null)
        {
            if (adjacent.occupied)
            {
                //�ϲ�
                if (CanMerge(tile, adjacent.tile))
                {
                    MergeTiles(tile, adjacent.tile);
                    return true;
                }

                break;
            }

            newCell = adjacent;
            adjacent = grid.GetAdjacentCell(adjacent, direction);
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
        tiles.Remove(a);
        a.Merge(b.cell);

        int index = Mathf.Clamp(IndexOf(b.state) + 1, 0, tileStates.Length - 1);
        TileState newState = tileStates[index];

        b.SetState(newState);
        gameManager.IncreaseScore(newState.number);
    }

    /// <summary>
    /// ��ȡ��Դ���������ڸ�����Դʱʹ��
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
        waiting = true;

        yield return new WaitForSeconds(0.1f);

        waiting = false;

        foreach (var tile in tiles)
        {
            tile.locked = false;
        }

        if (tiles.Count != grid.size)
        {
            CreateTile();
        }

        if (CheckForGameOver())
        {
            gameManager.GameOver();
        }
    }

    public bool CheckForGameOver()
    {
        if (tiles.Count != grid.size)
        {
            return false;
        }

        foreach (var tile in tiles)
        {
            TileCell up = grid.GetAdjacentCell(tile.cell, Vector2Int.up);
            TileCell down = grid.GetAdjacentCell(tile.cell, Vector2Int.down);
            TileCell left = grid.GetAdjacentCell(tile.cell, Vector2Int.left);
            TileCell right = grid.GetAdjacentCell(tile.cell, Vector2Int.right);

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
