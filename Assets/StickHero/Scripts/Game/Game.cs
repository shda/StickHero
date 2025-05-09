using Cysharp.Threading.Tasks;
using DG.Tweening;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace StickHero.Scripts.Game
{
    public class Game : MonoBehaviour
    {
        [SerializeField] private Hero hero;
        [SerializeField] private Stick stick;
        [SerializeField] private ScoreLabel scoreLabel;
        [SerializeField] private GameOver gameOver;
        [SerializeField] private PlatformsGenerator platformsGenerator;
        [SerializeField] private CinemachineCamera cinemachineCamera;
       // [SerializeField] Transform cameraTransform;

        [SerializeField] private float heroOffsetByEdge = 1.0f;
        [SerializeField] private float movingDuration = 1.0f;
        [SerializeField] private float movingCameraDuration = 1.0f;
        
        private bool _isGameOver;
        
        private int _currentPlatform;
        private Platform[] _platforms;

        private void Start()
        {
            hero.SavePosition();
            
            gameOver.OnRestart = Restart;
            StartGame();
        }

        private void Restart()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        private void StartGame()
        {
            gameOver.Hide();
            scoreLabel.ResetScore();
            
            _currentPlatform = 0;
            stick.OnCompleteDropStick = OnCompleteDropStick;
            var newPlatforms = platformsGenerator.CreatePlatforms();
            _platforms = newPlatforms.ToArray();
            SetHeroStart(_platforms[_currentPlatform]);
        }

        private async void SetHeroStart(Platform platform)
        {
            await UniTask.DelayFrame(3);
            
            var edgePoint = platform.GetTopRightPosition();
            hero.transform.position = new Vector3(edgePoint.x - heroOffsetByEdge, 
                hero.transform.position.y, hero.transform.position.z);
            stick.transform.position = new Vector3(edgePoint.x, 
                stick.transform.position.y, stick.transform.position.z);
            
            stick.ResetStick();
        }

        private void OnCompleteDropStick(float distance)
        {
            var startPointMoving = _platforms[_currentPlatform].GetTopRightPosition();
            var endPointMoving = startPointMoving.x + distance;

            var targetMovePlatform = FindStickPlatform(endPointMoving);
            if (targetMovePlatform != null)
            {
                scoreLabel.AddScore((int)distance);
            }
            HeroMovingToPlatform(endPointMoving , targetMovePlatform);
        }

        private Platform FindStickPlatform(float endPointMoving)
        {
            Platform nextPlatform = null;
            var offsetNextPlatform = 0;
            for (int i = 0; i < 5; i++)
            {
                if(_currentPlatform + i >= _platforms.Length)
                    break;
                
                var platform = _platforms[_currentPlatform + i];
                var leftPoint = platform.GetTopLeftPosition();
                var rightPoint = platform.GetTopRightPosition();

                if (endPointMoving > leftPoint.x && 
                    endPointMoving < rightPoint.x)
                {
                    Debug.Log("Ok");
                    nextPlatform = platform;
                    offsetNextPlatform = i;
                    break;
                }
            }
            
            _currentPlatform += offsetNextPlatform;
            return nextPlatform;
        }

        private async void HeroMovingToPlatform(float endPointMoving , Platform platform)
        {
            if (platform != null)
            {
                var nextPoint = platform.GetTopRightPosition();
                await hero.transform.DOMoveX(nextPoint.x - heroOffsetByEdge, movingDuration);
                await MovingCameraToPlatform(platform);
                
                SetHeroStart(platform);
            }
            //Game over
            else
            {
                cinemachineCamera.enabled = false;
                await hero.transform.DOMoveX(endPointMoving, movingDuration);
                await hero.transform.DOMoveY( hero.transform.position.y - 10.0f, movingDuration);
                
                gameOver.Show();
            }
        }

        private async UniTask MovingCameraToPlatform(Platform platform)
        {
            /*
            var pointMoving = platform.GetTopLeftPosition();
            await cameraTransform.transform.DOMoveX(pointMoving.x, movingDuration);
            */
        }
    }
}