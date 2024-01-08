using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Tile : MonoBehaviour
{
    public TileState state { get; private set; }//��ȡ��Դ
    public TileCell cell { get; private set; }
    public bool locked { get; set; }

    private Image background;
    private TextMeshProUGUI text;

    private void Awake()
    {
        background = GetComponent<Image>();
        text = GetComponentInChildren<TextMeshProUGUI>();

        DOTween.Init(false, true, LogBehaviour.ErrorsOnly);
    }

    /// <summary>
    /// ����Դ���õ�Tile��
    /// </summary>
    /// <param name="state"></param>
    public void SetState(TileState state)
    {
        this.state = state;

        background.color = state.backgroundColor;
        text.color = state.textColor;
        text.text = state.number.ToString();
    }

    /// <summary>
    /// ����tile��ָ����cell��
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

        // ʹ��DOTween��������
        transform.localScale = Vector3.zero;
        transform.DOScale(Vector3.one, 0.2f);
    }

    /// <summary>
    /// �ƶ�
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
    /// �ϲ�
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
                Destroy(gameObject);// ���ٵ�ǰ����
            });
    }
}
