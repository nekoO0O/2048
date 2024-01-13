using UnityEngine;

public class TileRow : MonoBehaviour
{
    public TileCell[] cells { get; private set; }

    private void Awake()
    {
        // 在 TileRow 中管理所有 TileCell
        cells = GetComponentsInChildren<TileCell>();
    }
}
