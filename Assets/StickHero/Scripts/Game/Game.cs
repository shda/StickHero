using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace StickHero.Scripts.Game
{
    public class Game : MonoBehaviour
    {
        [SerializeField] private Hero hero;
        [SerializeField] private Stick stick;
        [SerializeField] private PlatformsGenerator platformsGenerator;
        //[SerializeField] private Transform rootPlatforms;
        
        [SerializeField] Transform cameraTransform;

        [SerializeField] private float heroOffsetByEdge = 1.0f;
        [SerializeField] private float movingDuration = 1.0f;
        [SerializeField] private float movingCameraDuration = 1.0f;
        
        private int _currentPlatform;
        private Platform[] _platforms;

        private void Start()
        {
           // _platforms = rootPlatforms.GetComponentsInChildren<Platform>();
            StartGame();
        }

        private void StartGame()
        {
            _currentPlatform = 0;
            stick.OnCompleteDropStick = OnCompleteDropStick;
            var newPlatforms = platformsGenerator.CreatePlatforms();
            _platforms = newPlatforms.ToArray();
            
            SetHeroStart(_platforms[_currentPlatform]);
        }

        private async void SetHeroStart(Platform platform)
        {
            await UniTask.Yield();
            
            var edgePoint = platform.GetTopRightPosition();
            hero.transform.position = new Vector3(edgePoint.x - heroOffsetByEdge, 
                hero.transform.position.y, hero.transform.position.z);
            stick.transform.position = new Vector3(edgePoint.x, 
                stick.transform.position.y, stick.transform.position.z);
            
            stick.ResetStick();
        }

        private void OnCompleteDropStick(float distance)
        {
            var startPoint = _platforms[_currentPlatform].GetTopRightPosition();
            var endPointMoving = startPoint.x + distance;
            
            var nextPlatform = _platforms[_currentPlatform + 1];
            var leftPoint = nextPlatform.GetTopLeftPosition();
            var rightPoint = nextPlatform.GetTopRightPosition();

            if (endPointMoving > leftPoint.x && 
                endPointMoving < rightPoint.x)
            {
                Debug.Log("Ok");
                HeroMoving(endPointMoving , nextPlatform);
                _currentPlatform++;
            }
            else
            {
                Debug.Log("EndLevel");
                HeroMoving(endPointMoving , null);
            }
        }

        private async void HeroMoving(float endPointMoving , Platform platform)
        {
            if (platform != null)
            {
                var nextPoint = platform.GetTopRightPosition();
                await hero.transform.DOMoveX(nextPoint.x - heroOffsetByEdge, movingDuration);
                await MovingCameraToPlatform(platform);
                
                SetHeroStart(platform);
            }
            else
            {
                await hero.transform.DOMoveX(endPointMoving, movingDuration);
                await hero.transform.DOMoveY( hero.transform.position.y - 10.0f, movingDuration);
            }
        }

        private async UniTask MovingCameraToPlatform(Platform platform)
        {
            var pointMoving = platform.GetTopLeftPosition();
            await cameraTransform.transform.DOMoveX(pointMoving.x, movingDuration);
        }
        
        private void OnDrawGizmos()
        {
            if(_platforms == null || _platforms.Length == 0)
                return;
            
            var nextPlatform = _platforms[_currentPlatform + 1];
            var leftPoint = nextPlatform.GetTopLeftPosition();
            var rightPoint = nextPlatform.GetTopRightPosition();
            
            Gizmos.DrawSphere(leftPoint , 0.5f);
            Gizmos.DrawSphere(rightPoint , 0.5f);
        }
    }
}