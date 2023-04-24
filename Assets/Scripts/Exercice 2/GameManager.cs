using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameManager : Singleton<GameManager>
{
    #region Attributes

    private List<Ball> _balls;
    private PlayerBall _playerBall;
    private int _score = 4;
    private List<Vector2> _initPositions;
    private bool _running = false;

    [SerializeField] private Vector2 ballInitPosition;
    [SerializeField] private GameObject ballPrefab;
    [SerializeField] private GameObject playerBallPrefab;

    #endregion

    #region Properties

    public int Score => _score;

    #endregion
    
    #region Methods

    #region MonoBehaviours Callbacks

    private void Start()
    {
        //Behaviours setups
        OnGameWon += () => _running = false;
        OnGameLost += () => _running = false;
        
        //Objects setups
        _balls = new List<Ball>();

        //Initialize the game
        Init();
    }

    #endregion

    #region Public Methods

    /// <summary>
    /// Change score depending on ball color and reset its position if needed
    /// </summary>
    /// <param name="ball">The ball that went into the hole</param>
    public void BallInHole(Ball ball)
    {
        if (!_running) return;
        
        switch (ball.Color)
        {
            case Ball.BallColor.Red:
                ScoreUp();
                ball.gameObject.SetActive(false);
                break;
            case Ball.BallColor.Yellow:
                ScoreDown();
                ball.ResetPosition();
                break;
            case Ball.BallColor.White:
                ScoreDown();
                ball.ResetPosition();
                break;
        }
    }

    /// <summary>
    /// Add 1pt to the score
    /// </summary>
    private void ScoreUp()
    {
        _score++;
        OnScoreUpdate?.Invoke();
        if (_score >= 10)
        {
            OnGameWon?.Invoke();
        }
    }

    /// <summary>
    /// Remove 1pt from the score
    /// </summary>
    private void ScoreDown()
    {
        _score--;
        OnScoreUpdate?.Invoke();
        if (_score <= 0)
        {
            OnGameLost?.Invoke();
        }
    }

    #endregion

    #region Public Methods    

    /// <summary>
    /// Initiate the game
    /// </summary>
    public void Init()
    {
        //Initial setups
        _running = true;
        OnGameStart?.Invoke();
        
        //Reset the balls if this is a new game
        ResetBalls();
        _score = 4;
        
        //Calculate positions
        CalculateInitPosition();

        //Spawn Balls
        SpawnBalls();
        
        //Init Player Ball
        Vector2 pos = new Vector2(ballInitPosition.x + 5*(ballPrefab.transform.localScale.x), ballInitPosition.y);
        GameObject pBall = GameObject.Instantiate(playerBallPrefab, pos, Quaternion.identity);
        _playerBall = pBall.GetComponent<PlayerBall>();
        _playerBall.InitPos = pos;
        _balls.Add(_playerBall);
    }
    
    #endregion
    
    #region Private Methods

    /// <summary>
    /// Calculate the initial positions of the balls depending on the first ball position
    /// </summary>
    private void CalculateInitPosition()
    {
        _initPositions = new List<Vector2>();
        float distScale = ballPrefab.transform.localScale.x;

        int i = 1;
        int tot = 0;

        float memX = ballInitPosition.x;
        float memY = ballInitPosition.y;
        
        while (i < 7 && tot < 18)
        {
            for (int j = 0; j < i; j++)
            {
                _initPositions.Add(new Vector2(memX, memY - j*distScale));
                tot++;
                if (tot >= 18) break;
            }
            
            memX -= distScale;
            memY += 0.5f*distScale;
            i++;
        }
    }

    /// <summary>
    /// Create a ball of given color at given position
    /// </summary>
    /// <param name="color"></param>
    /// <param name="position"></param>
    private void SpawnBall(Ball.BallColor color, Vector2 position)
    {
        GameObject ball = GameObject.Instantiate(ballPrefab, position, Quaternion.identity);
        Ball tempBall = ball.GetComponent<Ball>();
        tempBall.Color = color;
        tempBall.InitPos = position;
        _balls.Add(tempBall);
    }

    /// <summary>
    /// Create all the non-player balls of the game. Their positions are random.
    /// </summary>
    private void SpawnBalls()
    {
        int countRed = 8;
        int countYellow = 8;
        int ballCount = 0;

        while (countRed > 0 || countYellow > 0 && ballCount < 18)
        {
            if (countRed <= 0)
            {
                SpawnBall(Ball.BallColor.Yellow, _initPositions[ballCount]);
                ballCount++;
                countYellow--;
            } else if (countYellow <= 0)
            {
                SpawnBall(Ball.BallColor.Red, _initPositions[ballCount]);
                ballCount++;
                countRed--;
            }
            else
            {
                int rand = Random.Range(0, 2);
                SpawnBall(rand == 0 ? Ball.BallColor.Red : Ball.BallColor.Yellow, _initPositions[ballCount]);
                ballCount++;
                if (rand == 0) countRed--;
                else countYellow--;
            }
        }
    }

    /// <summary>
    /// Destroy all the balls currently used
    /// </summary>
    private void ResetBalls()
    {
        foreach (var ball in _balls)
        {
            if (ball != null)
            {
                Destroy(ball.gameObject);
            }
        }
    }

    #endregion
    
    #endregion

    #region Events

    public event Action OnScoreUpdate;
    public event Action OnGameStart;

    public delegate void OnGameEnd();

    public event OnGameEnd OnGameWon;
    public event OnGameEnd OnGameLost;

    #endregion
}
