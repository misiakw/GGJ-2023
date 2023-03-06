using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreBoardScript : MonoBehaviour
{

    private GameObject playerWindow;
    private TMP_Text playerScoreLabel;
    private TMP_InputField playerNameInput;
    private ScoreManager scoreManager;

    void Start()
    {
        playerWindow = transform.Find("CurrentPlayerScore").gameObject;
        playerScoreLabel = transform.Find("CurrentPlayerScore/Canvas/Score")
                                .gameObject.GetComponent<TMP_Text>();
        playerNameInput = transform.Find("CurrentPlayerScore/Canvas/NameInput")
                                .gameObject.GetComponent<TMP_InputField>();
        scoreManager = ScoreManager.instance;
        if (scoreManager.LastScore.HasValue)
        {
            playerScoreLabel.text = scoreManager.LastScore.Value.ToString();
            playerWindow.SetActive(true);
        }
    }
    public void OnTextEnter()
    {
        var playerName = playerNameInput.text.Replace('<', '(').Replace('>', ')');
        if (playerName != playerNameInput.text)
            playerNameInput.text = playerName;
    }

    public void SavePlayerScore()
    {
        var playerName = playerNameInput.text;
        var score = scoreManager.LastScore ?? 0;
        Debug.Log($"Save name {playerName} score {score}");
        playerWindow.SetActive(false);

        if(scoreManager.LastScore.HasValue)
        {
            scoreManager.AddPlayerScore(playerName, score);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
