using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Utilities;

namespace Core.Input
{
    [CreateAssetMenu(fileName = "Pointer Data", menuName = "Utilities/Reference Data/Pointer", order = 0)]
    public class PointerData : ScriptableObject
    {
        public enum PointerStates
        {
            UP,
            DOWN,
            DRAGGING,
        }
        [Header("References")]
        [SerializeField] CameraData cameraData;

        [Header("Variables")]
        [SerializeField] [ReadOnly] PointerStates m_pointerState = PointerStates.UP;
        [SerializeField] [ReadOnly] PointerStates m_pointerOldState = PointerStates.UP;
        [SerializeField] [ReadOnly] Vector2 m_pointerPositionRaw = Vector2.zero;
        [SerializeField] [ReadOnly] Vector2 m_pointerPosition = Vector2.zero;
        [SerializeField] [ReadOnly] Vector2 m_pointerOldPosition = Vector2.zero;
        [SerializeField] [ReadOnly] Vector2 m_pointerDelta = Vector2.zero;

        public PointerStates PointerState => m_pointerState;
        public PointerStates PointerOldState => m_pointerOldState;
        public Vector2 PointerScreenPosition => m_pointerPositionRaw;
        public Vector2 PointerPosition => m_pointerPosition;
        public Vector2 PointerOldPosition => m_pointerOldPosition;
        public Vector2 PointerDelta => m_pointerDelta;

        public event System.Action<PointerData> OnPointerDown;
        public event System.Action<PointerData> OnPointerUp;
        public event System.Action<PointerData> OnPointerDragStart;
        public event System.Action<PointerData> OnPointerDragEnd;
        public event System.Action<PointerData> OnPointerMove;

        void SetCurrentPointerState(PointerStates pointerState)
        {
            m_pointerOldState = m_pointerState;
            m_pointerState = pointerState;
        }

        public virtual void TrackPointer(Vector2 rawPosition)
        {
            m_pointerOldPosition = m_pointerPosition;

            m_pointerPositionRaw = rawPosition;
            m_pointerPosition = ScreenToWorld(UnityEngine.Camera.main, m_pointerPositionRaw);

            //m_pointerDelta = (m_pointerPosition - m_pointerOldPosition);

            if (m_pointerDelta == Vector2.zero)
                OnPointerMove?.Invoke(this);

            if (m_pointerState == PointerStates.DOWN && m_pointerDelta != Vector2.zero)
            {
                SetCurrentPointerState(PointerStates.DRAGGING);
                OnPointerDragStart?.Invoke(this);
            }
            else if (m_pointerState == PointerStates.DRAGGING && m_pointerDelta == Vector2.zero)
            {
                SetCurrentPointerState(PointerStates.DOWN);
                OnPointerDragEnd?.Invoke(this);
            }
        }

        public void TrackDelta(Vector2 delta) => m_pointerDelta = ScreenToWorld(UnityEngine.Camera.main, delta);

        public void PointerDown(InputAction.CallbackContext inputCallback)
        {
            SetCurrentPointerState(PointerStates.DOWN);

            OnPointerDown?.Invoke(this);
        }

        public void PointerUp(InputAction.CallbackContext inputCallback)
        {
            SetCurrentPointerState(PointerStates.UP);

            if (m_pointerOldState == PointerStates.DRAGGING)
                OnPointerDragEnd?.Invoke(this);

            OnPointerUp?.Invoke(this);
        }

        Vector2 ScreenToWorld(Camera camera, Vector2 pointer)
        {
            return camera.ScreenToWorldPoint(pointer);
            //return camera.ScreenToWorldPoint(CorrectByRect(camera, pointer));
        }


        public Vector2 CorrectByRect(Camera camera, Vector2 pointer)
        {
            pointer.x = pointer.x * camera.rect.size.x + camera.pixelRect.position.x;
            pointer.y = pointer.y * camera.rect.size.y + camera.pixelRect.position.y;
            return pointer;
        }
    }

}
