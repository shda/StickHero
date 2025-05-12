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
        [SerializeField] private GameOverMenu gameOverMenu;
        [SerializeField] private CinemachineCamera cinemachineCamera;
        
        [SerializeField] private float heroOffsetByEdge = 1.0f;
        [SerializeField] private float heroMovingDuration = 1.0f;
        
        [SerializeField] private Level[] levels;
        
        private bool _isGameOver;
        
        private int _indexCurrentPlatform;
        private Platform[] _platforms;

        private void Start()
        {
            gameOverMenu.OnRestart = Restart;
            StartGame();
        }

        private void Restart()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        private async void StartGame()
        {
            hero.SetIdleAnimation();
            gameOverMenu.Hide();
            scoreLabel.ResetScore();
            
            _indexCurrentPlatform = 0;
            stick.OnCompleteDropStick = OnCompleteDropStick;
            stick.OnStartStickResize = () =>
            {
                hero.SetUpStickAnimation();
            };
            stick.OnStickComplete = () =>
            {
                hero.SetIdleAnimation();
            };

            LoadLevelObjects();
            
            //for some reason the platforms don't have time to be created without delay
            await UniTask.DelayFrame(3);
            SetHeroToEndPlatform(_platforms[_indexCurrentPlatform]);
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

        private void SetHeroToEndPlatform(Platform platform)
        {
            var edgePoint = platform.GetTopRightPosition();
            hero.transform.position = new Vector3(edgePoint.x - heroOffsetByEdge, 
                hero.transform.position.y, hero.transform.position.z);
            stick.transform.position = new Vector3(edgePoint.x, 
                stick.transform.position.y, stick.transform.position.z);
            
            stick.ResetStick();
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
                hero.SetMovingAnimation();
                
                var positionMove = platform.GetTopRightPosition();
                await hero.transform.DOMoveX(positionMove.x - heroOffsetByEdge, heroMovingDuration);
                
                SetHeroToEndPlatform(platform);
            }
            //Game over
            else
            {
                hero.SetMovingAnimation();
                
                cinemachineCamera.enabled = false;
                await hero.transform.DOMoveX(endPointMoving, heroMovingDuration);
                
                hero.SetFallAnimation();
                await hero.transform.DOMoveY( hero.transform.position.y - 10.0f, heroMovingDuration);
                
                gameOverMenu.Show();
            }
            
            hero.SetIdleAnimation();
        }
    }
}