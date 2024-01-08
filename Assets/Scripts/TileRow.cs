using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileRow : MonoBehaviour
{
    public TileCell[] cells { get; private set; }

    private void Awake()
    {
        //��TileRow�й�������TileCell
        cells = GetComponentsInChildren<TileCell>();
    }
}
