using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileGrid : MonoBehaviour
{
    public TileRow[] rows { get; private set; }//����
    public TileCell[] cells { get; private set; }//�ܹ�������

    public int size => cells.Length;//�ܹ�������
    public int height => rows.Length;//�߶�y
    public int width => size / height;//���x

    private void Awake()
    {
        //��TileGrid�й�������TileRow��TileCell
        rows = GetComponentsInChildren<TileRow>();
        cells = GetComponentsInChildren<TileCell>();
    }

    private void Start()
    {
        /* Ϊÿ��cell��������
         * (0,0) (0,1) (0,2) (0,3)
         * (1,0) ...
         * (2,0) ...
         * (3,0) ...
         */ 
        //��ѭ��Ϊ����ÿһ��
        for (int y = 0; y < rows.Length; y++)
        {
            //��ѭ��Ϊÿһ�е�ÿ��Ԫ�ظ�ֵ����
            for (int x = 0; x < rows[y].cells.Length; x++)
            {
                rows[y].cells[x].coordinates = new Vector2Int(x, y);
            }
        }
    }//Ϊÿ��cell��������

    public TileCell GetCell(Vector2Int coordinates)
    {
        return GetCell(coordinates.x, coordinates.y);
    }//�õ�x��y

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
    }//�õ�����x��y

    public TileCell GetAdjacentCell(TileCell cell, Vector2Int direction)
    {
        Vector2Int coordinates = cell.coordinates;
        coordinates.x += direction.x;
        coordinates.y -= direction.y;

        return GetCell(coordinates);
    }//���ݷ����ҵ����ڵ�cell

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

            // ����cells����ռ��
            if (index == startingIndex)
            {
                return null;
            }
        }

        return cells[index];
    }//�ҵ�һ���յ�cell
}
