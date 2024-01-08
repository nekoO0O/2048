using UnityEngine;

public class TileCell : MonoBehaviour
{
    public Vector2Int coordinates { get; set; }//坐标
    public Tile tile { get; set; }
    public bool empty => tile == null;//空
    public bool occupied => tile != null;//被占用
}
