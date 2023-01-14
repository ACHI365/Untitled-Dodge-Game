using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static int score;
    public TextMeshProUGUI scoreText, highsSoreText, afterDeath;
    public int highScore;
    private int offset;

    void Start()
    {
        highScore = PlayerPrefs.GetInt("Score");
        // highsSoreText.text = "HighScore: " + highScore.ToString();
    }

    void Update()
    {
        scoreText.text = score.ToString();
    }

    public void End()
    {
        if (score > highScore)
        {
            highScore = score;
            highsSoreText.text = "HighScore: " + highScore.ToString();
            PlayerPrefs.SetInt("Score", highScore);
            PlayerPrefs.Save();
        }
        highsSoreText.text = "HighScore: " + highScore.ToString();
        afterDeath.text = "Score: " + score.ToString();
    }
    
}
