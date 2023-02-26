using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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
        }
        _text.text = $"{GlobalStore.Score.Value} | HI: {GlobalStore.HighestScore}";
    }
}
