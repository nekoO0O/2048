using DG.Tweening;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public TileBoard board;
    public CanvasGroup gameOver;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI hisScoreText;

    private int _score;

    private void Awake()
    {
        InputManager inputManager = new InputManager();
        inputManager.Init();
    }

    private void OnEnable()
    {
        InputManager.Instance.Enable();
    }

    private void Start()
    {
        NewGame();
    }

    private void OnDisable()
    {
        InputManager.Instance.Disable();
    }

    public void NewGame()
    {
        // reset score
        SetScore(0);
        hisScoreText.text = LoadHisScore().ToString();

        // hide game over screen
        gameOver.alpha = 0f;
        gameOver.interactable = false;

        // update board state
        board.ClearBoard();
        board.CreateTile();
        board.CreateTile();
        board.enabled = true;
    }

    public void GameOver()
    {
        board.enabled = false;
        gameOver.interactable = true;

        gameOver.DOFade(1f, 1f);
    }

    public void IncreaseScore(int points)
    {
        SetScore(_score + points);
    }

    private void SetScore(int score)
    {
        this._score = score;
        scoreText.text = score.ToString();

        SaveHisScore();
    }

    private void SaveHisScore()
    {
        int hisScore = LoadHisScore();

        if (_score > hisScore)
        {
            PlayerPrefs.SetInt("hisScore", _score);
        }
    }

    private int LoadHisScore()
    {
        return PlayerPrefs.GetInt("hisScore", 0);
    }
}