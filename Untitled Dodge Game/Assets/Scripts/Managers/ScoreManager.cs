using TMPro;
using UnityEngine;

namespace Managers
{
    public class ScoreManager : MonoBehaviour
    {
        public static int Score;
        public TextMeshProUGUI scoreText, highsSoreText, afterDeath;
        public int highScore;

        void Start()
        {
            highScore = PlayerPrefs.GetInt("Score"); // set highScore
        }

        void Update()
        {
            scoreText.text = Score.ToString();
        }

        public void End() // if game ended display highScore
        {
            if (Score > highScore)
            {
                highScore = Score;
                highsSoreText.text = highScore.ToString();
                PlayerPrefs.SetInt("Score", highScore);
                PlayerPrefs.Save();
            }
            highsSoreText.text = highScore.ToString();
            afterDeath.text = Score.ToString();
        }
    
    }
}