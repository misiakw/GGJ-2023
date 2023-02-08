using System;
using UnityEngine;
using UnityEngine.UI;

public class ScoreController : MonoBehaviour
{
    public GameObject TextObject;
    private Text _text;
    public GameObject ReslultTextObject;

    private float elapsedTime;
    public float timePerScorePoint = 1f;
    bool isRunning = GlobalStore.State.Value == GameState.Running;

    // Start is called before the first frame update
    void Start()
    {
        _text = TextObject.GetComponent<Text>();
        GlobalStore.State.Onchange += (s, v) => isRunning = v == GameState.Running;
    }

    private void Update()
    {
        _text.text = $"{GlobalStore.Score} | HI: {GlobalStore.HighestScore}";

        if (isRunning)
        {
            ManageScoreIncrement();
        }
    }

    private void ManageScoreIncrement()
    {
        elapsedTime += Time.deltaTime;
        if (elapsedTime >= timePerScorePoint)
        {
            elapsedTime = 0;
            GlobalStore.Score++;
        }
    }
}
