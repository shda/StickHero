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
        
        private StickStage _stickStage = StickStage.None;
        private float _currentTimeStickRotation;
        
        private void Update()
        {
            switch (_stickStage)
            {
                case StickStage.None:
                    _currentTimeStickRotation = 0;
                    transform.rotation = Quaternion.identity;
                    stickRenderer.size = new Vector2(stickRenderer.size.x, minStickSize);
                    if (IsStartSize())
                    {
                        _stickStage = StickStage.Resize;
                        OnStartStickResize?.Invoke();
                    }
                    break;
                case StickStage.Resize:
                    if (IsStopSize())
                    {
                        _stickStage = StickStage.Rotate;
                        OnStickComplete?.Invoke();
                        return;
                    }
                    var newSize = stickRenderer.size.y + speedChangeStickSize * Time.deltaTime;
                    stickRenderer.size = new Vector2(stickRenderer.size.x, Math.Clamp(newSize , minStickSize, maxStickSize));
                   
                    break;
                case StickStage.Rotate:
                    _currentTimeStickRotation += Time.deltaTime;
                    var t = _currentTimeStickRotation / durationToRotation;
                    transform.rotation = Quaternion.Slerp(Quaternion.identity, Quaternion.Euler(0,0,-90), t);
                    if (_currentTimeStickRotation > durationToRotation)
                    {
                        _stickStage = StickStage.Complete;
                        
                    }
                    break;
                case StickStage.Complete:
                    OnCompleteDropStick?.Invoke(stickRenderer.size.y);
                    _stickStage = StickStage.End;
                    break;
                case StickStage.End:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public void ResetStick()
        {
            _stickStage = StickStage.None;
            Update();
        }

        private bool IsStartSize()
        {
            if (EventSystem.current.IsPointerOverGameObject())
                return false;
            
            return Input.GetMouseButtonDown(0);
        }

        private bool IsStopSize()
        {
            return Input.GetMouseButtonUp(0);
        }
    }
}