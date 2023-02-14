using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScoreController : MonoBehaviour
{
    public GameObject TextObject;
    private TextMeshPro _text;

    private float elapsedTime;
    public float timePerScorePoint = 1f;
    bool isRunning = GlobalStore.State.Value == GameState.Running;

    // Start is called before the first frame update
    void Start()
    {
        _text = gameObject.GetComponent<TextMeshPro>();
        _text.text = $"{GlobalStore.Score.Value} | HI: {GlobalStore.HighestScore}";
        GlobalStore.State.Onchange += (s, v) => isRunning = v == GameState.Running;
        GlobalStore.Score.Onchange += onScoreChange;
    }

    private void Update()
    {
    }

    void onScoreChange(object sender, int score)
    {
        if (GlobalStore.HighestScore < score)
        {
            GlobalStore.HighestScore = score;
        }
        _text.text = $"{GlobalStore.Score.Value} | HI: {GlobalStore.HighestScore}";
    }
}
