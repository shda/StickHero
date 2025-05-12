using TMPro;
using UnityEngine;

namespace StickHero.Scripts.Game
{
    public class ScoreLabel : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI scoreText;

        private int _score;
        public int Score
        {
            get => _score;
            set
            {
                _score = value;
                scoreText.text = _score.ToString();
            }
        }
        
        public void ResetScore()
        {
            Score = 0;
        }
        
        public void SetScore(int score)
        {
            scoreText.text = score.ToString();
        }

        public void UpScore(int scoreAdd)
        {
            Score += scoreAdd;
        }
    }
}