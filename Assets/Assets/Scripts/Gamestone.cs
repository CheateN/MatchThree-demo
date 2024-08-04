using UnityEngine;
using Random = UnityEngine.Random;

public class Gamestone : MonoBehaviour
{
    public float xoffset = -2;
    public float yoffset = -3;
    public int rowIndex; // 行索引
    public int columnIndex; // 列索引

    public GameObject[] gamestonePrefab;
    public int gamestoneType;
    public GameController gameController;
    private GameObject _gamestone;

    private SpriteRenderer _spriteRenderer;

    public bool IsSelected
    {
        set => _spriteRenderer.color = value ? Color.red : Color.white;
    }

    void Awake()
    {
        gameController = GameObject.Find("GameController").GetComponent<GameController>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        if (_spriteRenderer == null)
        {
            _spriteRenderer = _gamestone.GetComponent<SpriteRenderer>();
        }
    }

    // 更新宝石的位置
    private void UpdatePositionInternal(int _rowIndex, int _columnIndex)
    {
        this.rowIndex = _rowIndex;
        columnIndex = _columnIndex;
        // 更新宝石的位置
        transform.position = new Vector3(_columnIndex * 1.2f + xoffset, _rowIndex * 1.2f + yoffset, 0);
    }


    public void UpdatePosition(int _rowIndex, int _columnIndex)
    {
        UpdatePositionInternal(_rowIndex, _columnIndex);
    }

    // 宝石移动到新位置
    public void TweenToPosition(int _rowIndex, int _columnIndex)
    {
        UpdatePositionInternal(_rowIndex, _columnIndex);
        iTween.MoveTo(gameObject,
            iTween.Hash("x", columnIndex * 1.2f + xoffset, "y", rowIndex * 1.2f + yoffset, "time", 0.5f));
    }

    // 随机创建宝石
    public void RandomCreateGamestone()
    {
        if (_gamestone) return;
        gamestoneType = Random.Range(0, gamestonePrefab.Length);
        _gamestone = Instantiate(gamestonePrefab[gamestoneType], transform, true);
    }

    // 鼠标点击宝石时的处理函数
    public void OnMouseDown()
    {
        gameController.Select(this);
    }

    // 处理宝石的销毁
    public void Dispose()
    {
        Destroy(gameObject);
        gameController = null;
    }
}