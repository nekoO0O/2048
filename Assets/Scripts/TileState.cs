using UnityEngine;

// 创建自定义资源菜单的属性标签
[CreateAssetMenu(menuName = "Tile State")]
public class TileState : ScriptableObject
{
    public Color backgroundColor;
    public Color textColor;
    public int number;
}
