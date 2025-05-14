using Cysharp.Threading.Tasks;
using DG.Tweening;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace StickHero.Scripts.Game
{
    public class Game : MonoBehaviour
    {
        [SerializeField] private HeroAnimations heroAnimations;
        [SerializeField] private Stick stick;
        [SerializeField] private ScoreLabel scoreLabel;
        [SerializeField] private GameOverScreen gameOverScreen;
        [SerializeField] private CinemachineCamera cinemachineCamera;
        
        [SerializeField] private float heroOffsetByEdge = 1.0f;
        [SerializeField] private float heroMovingDuration = 1.0f;
        
        [SerializeField] private Level[] levels;
        
        private bool _isGameOver;
        
        private int _indexCurrentPlatform;
        private Platform[] _platforms;

        private void Start()
        {
            gameOverScreen.OnRestart = Restart;
            StartGame();
        }

        private void Restart()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        private async void StartGame()
        {
            heroAnimations.SetIdleAnimation();
            gameOverScreen.Hide();
            scoreLabel.ResetScore();
            
            _indexCurrentPlatform = 0;
            stick.OnCompleteDropStick = OnCompleteDropStick;
            stick.OnStartStickResize = () =>
            {
                heroAnimations.SetUpStickAnimation();
            };
            stick.OnStickComplete = () =>
            {
                heroAnimations.SetIdleAnimation();
            };

            LoadLevelObjects();
            
            //for some reason the platforms don't have time to be created without delay
            await UniTask.DelayFrame(3);
            SetHeroToEndEdgeOnPlatform(_platforms[_indexCurrentPlatform]);
        }

        private void LoadLevelObjects()
        {
            foreach (var level in levels)
            {
                level.gameObject.SetActive(false);
            }

            var selectLevel = levels[GlobalSettings.LevelIndex];
            selectLevel.gameObject.SetActive(true);
            selectLevel.InitLevel();
            
            var createdPlatforms = selectLevel.PlatformsGenerator.CreatePlatforms();
            _platforms = createdPlatforms.ToArray();
        }

        private void SetHeroToEndEdgeOnPlatform(Platform platform)
        {
            var edgePoint = platform.GetTopRightPosition();
            heroAnimations.transform.position = new Vector3(edgePoint.x - heroOffsetByEdge, 
                heroAnimations.transform.position.y, heroAnimations.transform.position.z);
            stick.transform.position = new Vector3(edgePoint.x, 
                stick.transform.position.y, stick.transform.position.z);
            
            stick.ResetStickStage();
        }

        private void OnCompleteDropStick(float stickLength)
        {
            var startPointMoving = _platforms[_indexCurrentPlatform].GetTopRightPosition();
            var endOfStickX = startPointMoving.x + stickLength;

            var targetMovePlatform = FindNearestPlatform(endOfStickX);
            if (targetMovePlatform)
            {
                scoreLabel.UpScore((int)stickLength);
            }
            HeroMoveOnStickAsync(endOfStickX , targetMovePlatform);
        }

        private Platform FindNearestPlatform(float endOfStickX)
        {
            Platform findNextPlatform = null;
            var findPlatformIndex = 0;

            var maxCheckPlatforms = 5;
            for (int i = 0; i < maxCheckPlatforms; i++)
            {
                if(_indexCurrentPlatform + i >= _platforms.Length)
                    break;
                
                var platform = _platforms[_indexCurrentPlatform + i];
                var topLeftPosition = platform.GetTopLeftPosition();
                var topRightPosition = platform.GetTopRightPosition();

                if (endOfStickX > topLeftPosition.x && 
                    endOfStickX < topRightPosition.x)
                {
                    findNextPlatform = platform;
                    findPlatformIndex = i;
                    break;
                }
            }
            
            _indexCurrentPlatform += findPlatformIndex;
            return findNextPlatform;
        }

        private async void HeroMoveOnStickAsync(float endPointMoving , Platform platform)
        {
            if (platform != null)
            {
                heroAnimations.SetMovingAnimation();
                
                var positionMove = platform.GetTopRightPosition();
                await heroAnimations.transform.DOMoveX(positionMove.x - heroOffsetByEdge, heroMovingDuration);
                
                SetHeroToEndEdgeOnPlatform(platform);
            }
            //Game over
            else
            {
                heroAnimations.SetMovingAnimation();
                
                cinemachineCamera.enabled = false;
                await heroAnimations.transform.DOMoveX(endPointMoving, heroMovingDuration);
                
                heroAnimations.SetFallAnimation();
                await heroAnimations.transform.DOMoveY( heroAnimations.transform.position.y - 10.0f, heroMovingDuration);
                
                gameOverScreen.Show();
            }
            
            heroAnimations.SetIdleAnimation();
        }
    }
}