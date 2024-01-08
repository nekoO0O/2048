using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileGrid : MonoBehaviour
{
    public TileRow[] rows { get; private set; }//行数
    public TileCell[] cells { get; private set; }//总共格子数

    public int size => cells.Length;//总共格子数
    public int height => rows.Length;//高度y
    public int width => size / height;//宽度x

    private void Awake()
    {
        //在TileGrid中管理所有TileRow和TileCell
        rows = GetComponentsInChildren<TileRow>();
        cells = GetComponentsInChildren<TileCell>();
    }

    private void Start()
    {
        /* 为每个cell赋予坐标
         * (0,0) (0,1) (0,2) (0,3)
         * (1,0) ...
         * (2,0) ...
         * (3,0) ...
         */ 
        //外循环为遍历每一列
        for (int y = 0; y < rows.Length; y++)
        {
            //内循环为每一行的每个元素赋值坐标
            for (int x = 0; x < rows[y].cells.Length; x++)
            {
                rows[y].cells[x].coordinates = new Vector2Int(x, y);
            }
        }
    }//为每个cell赋予坐标

    public TileCell GetCell(Vector2Int coordinates)
    {
        return GetCell(coordinates.x, coordinates.y);
    }//得到x和y

    public TileCell GetCell(int x, int y)
    {
        if (x >= 0 && x < width && y >= 0 && y < height)
        {
            return rows[y].cells[x];
        }
        else
        {
            return null;
        }
    }//得到坐标x和y

    public TileCell GetAdjacentCell(TileCell cell, Vector2Int direction)
    {
        Vector2Int coordinates = cell.coordinates;
        coordinates.x += direction.x;
        coordinates.y -= direction.y;

        return GetCell(coordinates);
    }//根据方向找到相邻的cell

    public TileCell GetRandomEmptyCell()
    {
        int index = Random.Range(0, cells.Length);
        int startingIndex = index;

        while (cells[index].occupied)
        {
            index++;

            if (index >= cells.Length)
            {
                index = 0;
            }

            // 所有cells都被占用
            if (index == startingIndex)
            {
                return null;
            }
        }

        return cells[index];
    }//找到一个空的cell
}
