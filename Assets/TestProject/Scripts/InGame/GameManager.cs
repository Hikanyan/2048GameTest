using System;
using TMPro;
using UnityEngine;

namespace HikanyanLaboratory.InGame
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private GridManager _gridManager;
        [SerializeField] private InputManager _inputManager;

        [SerializeField] private CanvasGroup _gameOverScreen;
        [SerializeField] private bool _gameOver = false;
        [SerializeField] private bool _gameClear = false;
        [SerializeField] private TextMeshProUGUI _scoreText;
        [SerializeField] private TextMeshProUGUI _highScoreText;

        public GridManager GridManager => _gridManager;
        public int Score { get; private set; } = 0;

        private void Start()
        {
            _gridManager.InitializeGrid();
            NewGame();
        }

        public void NewGame()
        {
            // スコアをリセット
            SetScore(0);
            _highScoreText.text = LoadHighScore().ToString();

            // ゲームオーバー画面を非表示
            _gameOverScreen.alpha = 0f;
            _gameOverScreen.interactable = false;

            // グリッドの初期化
            _gridManager.InitializeGrid();
            _gridManager.SpawnTile(0, 0, 2);
            _gridManager.SpawnTile(1, 1, 2);
        }

        public bool CheckGameStatus()
        {
            if (_gridManager.IsFull() && !AnyMovesLeft())
            {
                _gameOver = true;
                ShowGameOverScreen();
                return false;
            }
            return true;
        }

        private void ShowGameOverScreen()
        {
            _gameOverScreen.alpha = 1f;
            _gameOverScreen.interactable = true;
        }

        private bool AnyMovesLeft()
        {
            // グリッド内の全てのタイルを確認し、隣接して同じ値のタイルがあるか、もしくは空きタイルがあるかをチェック
            for (int x = 0; x < _gridManager.GridSize; x++)
            {
                for (int y = 0; y < _gridManager.GridSize; y++)
                {
                    Tile currentTile = _gridManager.GetTileAt(x, y);

                    // 空のマスがあれば、まだ移動可能
                    if (currentTile == null)
                    {
                        return true;
                    }

                    // 隣接するタイルと同じ値があれば移動可能
                    if ((x < _gridManager.GridSize - 1 && currentTile.GetValue() == _gridManager.GetTileAt(x + 1, y)?.GetValue()) ||
                        (y < _gridManager.GridSize - 1 && currentTile.GetValue() == _gridManager.GetTileAt(x, y + 1)?.GetValue()))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public void IncreaseScore(int points)
        {
            SetScore(Score + points);
        }

        private void SetScore(int score)
        {
            this.Score = score;
            _scoreText.text = score.ToString();

            SaveHighScore();
        }

        private void SaveHighScore()
        {
            int highScore = LoadHighScore();
            if (Score > highScore)
            {
                PlayerPrefs.SetInt("HighScore", Score);
            }
        }

        private int LoadHighScore()
        {
            return PlayerPrefs.GetInt("HighScore", 0);
        }
    }
}
