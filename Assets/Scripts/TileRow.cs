using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileRow : MonoBehaviour
{
    public TileCell[] cells { get; private set; }

    private void Awake()
    {
        //在TileRow中管理所有TileCell
        cells = GetComponentsInChildren<TileCell>();
    }
}
