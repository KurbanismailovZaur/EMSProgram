using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace EMSP
{
    public class OrbitController : MonoBehaviour
    {
        #region Entities
        #region Enums
        #endregion

        #region Delegates
        #endregion

        #region Structures
        #endregion

        #region Classes
        #endregion

        #region Interfaces
        #endregion
        #endregion

        #region Fields
        private bool _isStartInViewport;

        [SerializeField]
        private Vector3 _origin;

        private Vector3 _targetVector;

        private Vector3 _targetUpVector;

        private Vector3 _currentVector;

        private Vector3 _currentUpVector;

        private float _distance;

        [SerializeField]
        [Range(1f, 32f)]
        private float _interpolation = 1f;

        [Header("Auto Control By Mouse")]
        [SerializeField]
        private bool _autoControlByMouse;

        [SerializeField]
        [Range(1f, 8f)]
        private float _autoControlByMouseDeltaMultiplier = 1f;

        [SerializeField]
        [Range(1f, 64)]
        private float _autoControlByMouseScrollMultiplier = 1f;

#if UNITY_EDITOR
        [Header("Debug")]
        [SerializeField]
        private bool _drawDebugLines;
#endif
        #endregion

        #region Events
        #endregion

        #region Behaviour
        #region Properties
        public bool AutoControlByMouse
        {
            get { return _autoControlByMouse; }
            set
            {
                _autoControlByMouse = value;
            }
        }

        public Vector3 Origin
        {
            get { return _origin; }
            set { _origin = value; }
        }

        public Vector3 TargetVector
        {
            get { return _targetVector; }
            set
            {
                value.Normalize();
                Quaternion fromToRotation = Quaternion.FromToRotation(_targetVector, value);

                _targetVector = value;
                _targetUpVector = fromToRotation * _targetUpVector;
            }
        }

        public Vector3 TargetUpVector
        {
            get { return _targetUpVector; }
            set
            {
                value.Normalize();
                Quaternion fromToRotation = Quaternion.FromToRotation(_targetUpVector, value);

                _targetUpVector = value;
                _targetVector = fromToRotation * _targetVector;
            }
        }

        public float Distance
        {
            get { return _distance; }
            set { _distance = Mathf.Max(value, 1f); }
        }

        public float Interpolation
        {
            get { return _interpolation; }
            set { _interpolation = Mathf.Max(value, 1f); }
        }
        #endregion

        #region Constructors
        #endregion

        #region Methods
        private void Awake()
        {
            _targetVector = transform.position - _origin;
            _distance = _targetVector.magnitude;

            _targetVector.Normalize();
            _targetUpVector = transform.up;

            _currentVector = _targetVector;
            _currentUpVector = _targetUpVector;
        }

        private void Update()
        {
            TryAutoControlByMouse();
            CalculateTransform();

#if UNITY_EDITOR
            if (_drawDebugLines)
            {
                DrawDebugLines();
            }
#endif
        }

        private void TryAutoControlByMouse()
        {
            if (_autoControlByMouse)
            {
                Vector2 mouseDelta = CalculateMouseInput();
                ApplyDeltasToTransform(mouseDelta * _autoControlByMouseDeltaMultiplier);
            }
        }

        private void CalculateViewportStartFlag()
        {
            if (Input.GetMouseButtonDown(0) || Input.mouseScrollDelta.y != 0f)
            {
                ForceCalculateViewportStartFlag();
            }
        }

        private void ForceCalculateViewportStartFlag()
        {
            if (EventSystem.current.IsPointerOverGameObject())
            {
                PointerEventData pointerData = new PointerEventData(EventSystem.current)
                {
                    pointerId = -1,
                };

                pointerData.position = Input.mousePosition;

                List<RaycastResult> results = new List<RaycastResult>();
                EventSystem.current.RaycastAll(pointerData, results);

                if (results.TrueForAll(x => x.gameObject.layer == LayerMask.NameToLayer("Gizmos") || x.gameObject.layer == LayerMask.NameToLayer("CalculatedDataLayer")))
                {
                    _isStartInViewport = true;
                }
                else
                {
                    _isStartInViewport = false;
                }
            }
            else
            {
                _isStartInViewport = true;
            }
        }

        private Vector2 CalculateMouseInput()
        {
            CalculateViewportStartFlag();

            float scrollWheel = -Input.GetAxis("Mouse ScrollWheel");
            if (_isStartInViewport)
            {
                Distance += scrollWheel * _autoControlByMouseScrollMultiplier;
            }

            if (Input.GetMouseButton(0) && _isStartInViewport)
            {
                return new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
            }
            else
            {
                return Vector2.zero;
            }
        }

        public void ApplyDeltasToTransform(Vector2 deltas)
        {
            ApplyDeltasToTransform(deltas.x, deltas.y);
        }

        public void ApplyDeltasToTransform(float deltaX, float deltaY)
        {
            Quaternion deltaXRotation = Quaternion.Euler(0f, deltaX, 0f);
            _targetVector = deltaXRotation * _targetVector;
            _targetUpVector = deltaXRotation * _targetUpVector;

            Quaternion yRotation = Quaternion.LookRotation(_targetVector, _targetUpVector) * Quaternion.Euler(deltaY, 0f, 0f);
            _targetVector = yRotation * Vector3.forward;
            _targetUpVector = yRotation * Vector3.up;
        }

        private void CalculateTransform()
        {
            _currentVector = Quaternion.Lerp(Quaternion.LookRotation(_currentVector, _targetUpVector), Quaternion.LookRotation(_targetVector, _targetUpVector), _interpolation * Time.deltaTime) * Vector3.forward;
            _currentUpVector = Quaternion.Lerp(Quaternion.LookRotation(_currentUpVector, _currentVector), Quaternion.LookRotation(_targetUpVector, _targetVector), _interpolation * Time.deltaTime) * Vector3.forward;

            transform.position = _origin + (_currentVector * _distance);
            transform.LookAt(_origin, _currentUpVector);
            //transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, 0f);
        }

        public void SetTargetVectorAndZAngle(Vector3 vector, float angle)
        {
            _targetVector = vector.normalized;

            if (_targetVector == Vector3.up)
            {
                _targetUpVector = Quaternion.AngleAxis(angle, Vector3.down) * Vector3.forward;
                return;
            }

            if (_targetVector == Vector3.down)
            {
                _targetUpVector = Quaternion.AngleAxis(angle, Vector3.up) * Vector3.back;
                return;
            }

            Vector3 relativeXAxis = Vector3.Cross(_targetVector, Vector3.up).normalized;
            Vector3 relativeYAxis = Vector3.Cross(relativeXAxis, _targetVector).normalized;

            Quaternion axisRotation = Quaternion.AngleAxis(angle, _targetVector);

            _targetUpVector = axisRotation * relativeYAxis;
        }

#if UNITY_EDITOR
        private void DrawDebugLines()
        {
            Debug.DrawLine(_origin, _origin + _targetVector, Color.red);
            Debug.DrawLine(_origin, _origin + _targetUpVector, Color.green);
            Debug.DrawLine(_origin, _origin + _currentVector, Color.yellow);
            Debug.DrawLine(_origin, _origin + _currentUpVector, Color.white);
        }
#endif
        #endregion

        #region Indexers
        #endregion

        #region Events handlers
        #endregion
        #endregion
    }
}