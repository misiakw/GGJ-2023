using System;
using UnityEngine;
using UnityEngine.UI;

public class ScoreController : MonoBehaviour
{
    public GameObject TextObject;
    private Text _text;
    public GameObject ResultTextObject;

    private float elapsedTime;
    public float timePerScorePoint = 1f;
    bool isRunning = GlobalStore.State.Value == GameState.Running;

    // Start is called before the first frame update
    void Start()
    {
        _text = TextObject.GetComponent<Text>();
        GlobalStore.State.Onchange += (s, v) => isRunning = v == GameState.Running;
        GlobalStore.Score.Onchange += onScoreChange;
    }

    private void Update()
    {
        if (isRunning)
        {
            ManageScoreIncrement();
        }
    }

    void onScoreChange(object sender, int score)
    {
        if (GlobalStore.HighestScore < score)
        {
            GlobalStore.HighestScore = score;
        }
        _text.text = $"{GlobalStore.Score.Value} | HI: {GlobalStore.HighestScore}";
    }

    private void ManageScoreIncrement()
    {
        elapsedTime += Time.deltaTime;
        if (elapsedTime >= timePerScorePoint)
        {
            elapsedTime = 0;
            GlobalStore.Score.Value++;
        }
    }
}
