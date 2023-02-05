using System;
using UnityEngine;
using UnityEngine.UI;

public class ScoreController : MonoBehaviour
{
    public GameObject TextObject;
    private Text _text;
    public GameObject ReslultTextObject;
    private Text _resultText;

    private float elapsedTime;
    public float timePerScorePoint = 1f;

    // Start is called before the first frame update
    void Start()
    {
        _text = TextObject.GetComponent<Text>();
        _resultText = ReslultTextObject.GetComponent<Text>();
    }

    private void Update()
    {
        _text.text = GlobalStore.Score.ToString();
        _resultText.text = $"Your score: {GlobalStore.Score}";
        
        if(GlobalStore.ShouldScrollScreen() || GlobalStore.Score == 0) 
        {
            TextObject.SetActive(true);
            ReslultTextObject.SetActive(false);
            if(GlobalStore.ShouldScrollScreen()) {
                ManageScoreIncrement();
            }
        }
        else {
            TextObject.SetActive(false);
            ReslultTextObject.SetActive(true);
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
