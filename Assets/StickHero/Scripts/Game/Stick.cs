using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

namespace StickHero.Scripts.Game
{
    public class Stick : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer stickRenderer;
        
        [SerializeField] private float speedChangeStickSize = 1.0f;
        [SerializeField] private float maxStickSize = 20.0f;
        [SerializeField] private float minStickSize = 2.0f;
        
        [SerializeField] private float durationToRotation = 1.0f;

        public Action<float> OnCompleteDropStick;
        public Action OnStartStickResize;
        public Action OnStickComplete;
        
        private StickAnimationStage _stickAnimationStage = StickAnimationStage.None;
        private float _currentTimeStickRotation;
        
        private void Update()
        {
            switch (_stickAnimationStage)
            {
                case StickAnimationStage.None:
                    _currentTimeStickRotation = 0;
                    transform.rotation = Quaternion.identity;
                    stickRenderer.size = new Vector2(stickRenderer.size.x, minStickSize);
                    if (IsStartChangeStickResize())
                    {
                        _stickAnimationStage = StickAnimationStage.Resize;
                        OnStartStickResize?.Invoke();
                    }
                    break;
                case StickAnimationStage.Resize:
                    if (IsStopChangeStickResize())
                    {
                        _stickAnimationStage = StickAnimationStage.Rotate;
                        OnStickComplete?.Invoke();
                        return;
                    }
                    var newSize = stickRenderer.size.y + speedChangeStickSize * Time.deltaTime;
                    stickRenderer.size = new Vector2(stickRenderer.size.x, Math.Clamp(newSize , minStickSize, maxStickSize));
                   
                    break;
                case StickAnimationStage.Rotate:
                    _currentTimeStickRotation += Time.deltaTime;
                    var t = _currentTimeStickRotation / durationToRotation;
                    transform.rotation = Quaternion.Slerp(Quaternion.identity, Quaternion.Euler(0,0,-90), t);
                    if (_currentTimeStickRotation > durationToRotation)
                    {
                        _stickAnimationStage = StickAnimationStage.Complete;
                        
                    }
                    break;
                case StickAnimationStage.Complete:
                    OnCompleteDropStick?.Invoke(stickRenderer.size.y);
                    _stickAnimationStage = StickAnimationStage.End;
                    break;
                case StickAnimationStage.End:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public void ResetStickStage()
        {
            _stickAnimationStage = StickAnimationStage.None;
            Update();
        }

        private bool IsStartChangeStickResize()
        {
            if (EventSystem.current.IsPointerOverGameObject())
                return false;
            
            return Input.GetMouseButtonDown(0);
        }

        private bool IsStopChangeStickResize()
        {
            return Input.GetMouseButtonUp(0);
        }
    }
}