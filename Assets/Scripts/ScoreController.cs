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

    // Start is called before the first frame update
    void Start()
    {
        _text = TextObject.GetComponent<Text>();
    }

    private void Update()
    {
        _text.text = $"{GlobalStore.Score} | HI: {GlobalStore.HighestScore}";
        
        if(GlobalStore.ShouldScrollScreen()) 
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
