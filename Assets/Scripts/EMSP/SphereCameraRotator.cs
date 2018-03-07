using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EMSP.Control
{
    public class SphereCameraRotator : MonoBehaviour 
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

        [SerializeField]
        [Range(0.1f, 24f)]
        private float _interpolation = 12f;
        private Transform _transform;

        public bool InputEnabled = true;
        public float Distance = 5.0f;
        public float XSpeed = 120.0f;
        public float YSpeed = 120.0f;
        public float ZoomSpeed = 5f;

        public bool ClampY = true;
        public float YMinLimit = -90f;
        public float YMaxLimit = 90f;

        public float DistanceMin = .5f;
        public float DistanceMax = 100f;

        private float _x = 0.0f;
        private float _y = 0.0f;
        private float _z = 0.0f;
        private Vector3 _targetPosition;
        private Quaternion _targetRotation;

        #endregion

        #region Events
        #endregion

        #region Behaviour
        #region Properties
        #endregion

        #region Constructors
        #endregion

        #region Methods

        void Start()
        {
            _transform = GetComponent<Transform>();

            Vector3 angles = _transform.eulerAngles;
            _x = angles.y;
            _y = angles.x;

            _targetPosition = _transform.position;
            _targetRotation = _transform.rotation;
        }

        void Update()
        {
            if (!Input.GetMouseButton(0) && Input.GetAxis("Mouse ScrollWheel") == 0f)
            {
                return;
            }

            MoveCamera(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"), Input.GetAxis("Mouse ScrollWheel"));
        }

        void LateUpdate()
        {
            _transform.position = Vector3.Slerp(_transform.position, _targetPosition, _interpolation * Time.deltaTime);
            _transform.rotation = Quaternion.Slerp(_transform.rotation, _targetRotation, _interpolation * Time.deltaTime);
        }

        public void MoveCamera(float deltaX = 0, float deltaY = 0, float deltaZ = 0)
        {
            if (!InputEnabled)
                return;

            _x += deltaX * XSpeed * 3 * 0.02f;
            _y -= deltaY * YSpeed * 0.02f;
            _z = deltaZ;

            CalculateTransform();
        }

        private void CalculateTransform()
        {
            if(ClampY)
                _y = ClampAngle(_y, YMinLimit, YMaxLimit);

            Quaternion rotation = Quaternion.Euler(_y, _x, 0);

            Distance = Mathf.Clamp(Distance - _z * ZoomSpeed, DistanceMin, DistanceMax);

            Vector3 negDistance = new Vector3(0.0f, 0.0f, -Distance);
            Vector3 position = rotation * negDistance;

            _targetRotation = rotation;
            _targetPosition = position;
        }

        private float ClampAngle(float angle, float min, float max)
        {
            if (angle < -360F)
                angle += 360F;
            if (angle > 360F)
                angle -= 360F;

            return Mathf.Clamp(angle, min, max);
        }

        #endregion

        #region Indexers
        #endregion

        #region Events handlers
        #endregion
        #endregion
    }
}
