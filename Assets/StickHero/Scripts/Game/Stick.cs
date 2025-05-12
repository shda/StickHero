using System;
using UnityEngine;

namespace StickHero.Scripts.Game
{
    public class Stick : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer stickRenderer;
        [SerializeField] private float speedSize = 1.0f;
        
        [SerializeField] private float maxSize = 20.0f;
        [SerializeField] private float minSize = 2.0f;
        
        [SerializeField] private float durationToRotation = 1.0f;

        public Action<float> OnCompleteDropStick;
        public Action OnStickResize;
        public Action OnStickComplete;
        
        private StickStage _stage = StickStage.None;
        private float _currentTimeRotation = 0;
        
        private void Update()
        {
            switch (_stage)
            {
                case StickStage.None:
                    _currentTimeRotation = 0;
                    transform.rotation = Quaternion.identity;
                    stickRenderer.size = new Vector2(stickRenderer.size.x, minSize);
                    if (IsStartSize())
                    {
                        _stage = StickStage.Resize;
                        OnStickResize?.Invoke();
                    }
                    break;
                case StickStage.Resize:
                    if (IsStopSize())
                    {
                        _stage = StickStage.Rotate;
                        OnStickComplete?.Invoke();
                        return;
                    }
                    var newSize = stickRenderer.size.y + speedSize * Time.deltaTime;
                    stickRenderer.size = new Vector2(stickRenderer.size.x, Math.Clamp(newSize , minSize, maxSize));
                   
                    break;
                case StickStage.Rotate:
                    _currentTimeRotation += Time.deltaTime;
                    var t = _currentTimeRotation / durationToRotation;
                    transform.rotation = Quaternion.Slerp(Quaternion.identity, Quaternion.Euler(0,0,-90), t);
                    if (_currentTimeRotation > durationToRotation)
                    {
                        _stage = StickStage.Complete;
                        
                    }
                    break;
                case StickStage.Complete:
                    OnCompleteDropStick?.Invoke(stickRenderer.size.y);
                    _stage = StickStage.End;
                    break;
                case StickStage.End:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public void ResetStick()
        {
            _stage = StickStage.None;
            Update();
        }

        private bool IsStartSize()
        {
            return Input.GetMouseButtonDown(0);
        }

        private bool IsStopSize()
        {
            return Input.GetMouseButtonUp(0);
        }
    }
}