using DG.Tweening;
using InputModule;
using TMPro;
using UnityEngine;

namespace GameModule
{
    public class GameManager : MonoBehaviour
    {
        private const string HighScoreKey = "hisScore";

        public TileBoard board;
        public CanvasGroup gameOver;
        public TextMeshProUGUI scoreText;
        public TextMeshProUGUI highScoreText;

        private int _score;

        private void Awake()
        {
            InputManager.Instance.Init();
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
            ResetGameState();
            InitializeBoardState();
        }

        private void ResetGameState()
        {
            SetScore(0);
            highScoreText.text = LoadHighScore().ToString();

            gameOver.alpha = 0f;
            gameOver.interactable = false;
        }

        private void InitializeBoardState()
        {
            board.ClearBoard();
            board.CreateTile(2);
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
            _score = score;
            scoreText.text = score.ToString();
            SaveHighScore();
        }

        private void SaveHighScore()
        {
            int highScore = LoadHighScore();
            PlayerPrefs.SetInt(HighScoreKey, Mathf.Max(_score, highScore));
        }

        private int LoadHighScore()
        {
            return PlayerPrefs.GetInt(HighScoreKey, 0);
        }
    }
}