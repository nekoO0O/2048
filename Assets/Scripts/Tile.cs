using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Tile : MonoBehaviour
{
    public TileState state { get; private set; } // 获取资源
    public TileCell cell { get; private set; }
    public bool locked { get; set; }

    private Image _background;
    private TextMeshProUGUI _text;

    private void Awake()
    {
        _background = GetComponent<Image>();
        _text = GetComponentInChildren<TextMeshProUGUI>();

        DOTween.Init(false, true, LogBehaviour.ErrorsOnly);
    }

    /// <summary>
    /// 将资源设置到Tile上
    /// </summary>
    /// <param name="state"></param>
    public void SetState(TileState state)
    {
        this.state = state;

        _background.color = state.backgroundColor;
        _text.color = state.textColor;
        _text.text = state.number.ToString();
    }

    /// <summary>
    /// 产生tile到指定的cell上
    /// </summary>
    /// <param name="cell"></param>
    public void Spawn(TileCell cell)
    {
        if (this.cell != null)
        {
            this.cell.tile = null;
        }

        this.cell = cell;
        this.cell.tile = this;

        transform.position = cell.transform.position;

        // 使用DOTween产生动画
        transform.localScale = Vector3.zero;
        transform.DOScale(Vector3.one, 0.2f);
    }

    /// <summary>
    /// 移动
    /// </summary>
    /// <param name="cell"></param>
    public void MoveTo(TileCell cell)
    {
        if (this.cell != null)
        {
            this.cell.tile = null;
        }

        this.cell = cell;
        this.cell.tile = this;

        transform.DOMove(cell.transform.position, 0.1f);
    }

    /// <summary>
    /// 合并
    /// </summary>
    /// <param name="cell"></param>
    public void Merge(TileCell cell)
    {
        if (this.cell != null)
        {
            this.cell.tile = null;
        }

        this.cell = null;
        cell.tile.locked = true;

        transform.DOMove(cell.transform.position, 0.1f)
            .OnComplete(() =>
            {
                Destroy(gameObject); // 销毁当前物体
            });
    }
}