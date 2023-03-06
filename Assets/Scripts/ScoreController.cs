using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TMPro;
using UnityEngine;

public class ScoreController : MonoBehaviour
{
    public GameObject TextObject;
    private TextMeshPro _text;
    private Canvas _canvas;

    // Start is called before the first frame update
    void Start()
    {
        _text = gameObject.GetComponent<TextMeshPro>();
        _canvas = GetComponent<Canvas>();
        _text.text = $"{GlobalStore.Score.Value} | HI: {GlobalStore.HighestScore}";
        GlobalStore.Score.Onchange += onScoreChange;
    }

    private void Update()
    {

        _canvas.overrideSorting = true;
        _canvas.sortingOrder = 1;
    }

    void onScoreChange(object sender, int score)
    {
        if (GlobalStore.HighestScore < score)
        {
            GlobalStore.HighestScore = score;
            ScoreManager.instance.LastScore = score;
        }
        _text.text = $"{GlobalStore.Score.Value} | HI: {GlobalStore.HighestScore}";
    }
}

public class ScoreManager
{
    private ScoreBoard _scoreBoard;
    private string dirPath;
    private string filePath;
    private static ScoreManager _instance;
    public int? LastScore {
        get { return _scoreBoard.LastPlayerScore; }
        set { _scoreBoard.LastPlayerScore = value; }
    }

    private ScoreManager()
    {
        dirPath = Path.Combine(Application.dataPath, "Score");
        filePath = Path.Combine(dirPath, $"{DateTime.Now.ToShortDateString().Replace('.', '-')}.json");
        _scoreBoard = LoadScore();
    }
    public static ScoreManager instance
    {
        get
        {
            if (_instance == null)
                _instance = new ScoreManager();
            return _instance;
        }
    }

    public void SetLastScore(int score)
    {
        _scoreBoard.LastPlayerScore = score;
        StoreScores(_scoreBoard);
    }

    public void AddPlayerScore(string playerName, int score)
    {
        _scoreBoard.LastPlayerScore = null;
        _scoreBoard.Add(playerName, score);
        StoreScores(_scoreBoard);
    }

    private ScoreBoard LoadScore()
    {
        if (!Directory.Exists(dirPath))
        {
            Directory.CreateDirectory(dirPath);
        }
        if (File.Exists(filePath))
        {
            var contentst = File.ReadAllText(filePath);
            try
            {
                var scoreBoard = JsonUtility.FromJson<ScoreBoard>(contentst);

                if (scoreBoard.OverallScore.Any())
                {
                    GlobalStore.HighestScore = scoreBoard.OverallScore.First().PlayerScore;
                }

                StoreScores(scoreBoard);
                return scoreBoard;
            }
            catch (Exception ex)
            {
                Debug.LogError(ex);
                return new ScoreBoard();
            }
        }
        else
        {
            var scoreBoard = new ScoreBoard();
            StoreScores(scoreBoard);
            return scoreBoard;
        }
    }
    private void StoreScores(ScoreBoard scoreBoard)
    {
        var contents = JsonUtility.ToJson(scoreBoard, true);
        File.WriteAllText(filePath, contents);
    }
}

[Serializable]
public class ScoreBoard
{
    public int? LastPlayerScore = null;
    public Score[] Score = new Score[0];
    public IList<Score> OverallScore => Score.OrderByDescending(s => s.PlayerScore).Take(3).ToList();

    public void Add(string playerName, int score)
    {
        var list = Score.ToList();
        list.Add(new Score
        {
            PlayerName = playerName,
            PlayerScore = score,
            Time = DateTime.Now
        });
        Score = list.OrderByDescending(s => s.PlayerScore).ToArray();
    }
}

[Serializable]
public class Score
{
    public string PlayerName;
    public int PlayerScore;
    public string TimeStr;
    public DateTime Time
    {
        set { TimeStr = value.ToShortTimeString(); }
        get { return DateTime.Parse(TimeStr); }
    }
}
