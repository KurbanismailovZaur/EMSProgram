using UnityEngine;
using System.Collections;

namespace EMSP.Control
{
    public class MouseOrbitImproved : MonoBehaviour
    {
        public Transform target;

        private Vector3 _targetPosition;
        private Quaternion _targetRotation;

        [SerializeField]
        [Range(0.1f, 16f)]
        private float _interpolation = 1f;

        public float distance = 5.0f;
        public float xSpeed = 120.0f;
        public float ySpeed = 120.0f;

        public float yMinLimit = -20f;
        public float yMaxLimit = 80f;

        public float distanceMin = .5f;
        public float distanceMax = 15f;

        private Rigidbody rigidbody;

        float x = 0.0f;
        float y = 0.0f;

        void Start()
        {
            Vector3 angles = transform.eulerAngles;
            x = angles.y;
            y = angles.x;

            _targetPosition = transform.position;
            _targetRotation = transform.rotation;
        }

        void LateUpdate()
        {
            transform.position = Vector3.Lerp(transform.position, _targetPosition, _interpolation * Time.deltaTime);
            transform.rotation = Quaternion.Lerp(transform.rotation, _targetRotation, _interpolation * Time.deltaTime);

            if (!Input.GetMouseButton(0) && Input.GetAxis("Mouse ScrollWheel") == 0f)
            {
                return;
            }

            CalculateTransform();
        }

        private void CalculateTransform()
        {
            if (target)
            {
                x += Input.GetAxis("Mouse X") * xSpeed * distance * 0.02f;
                y -= Input.GetAxis("Mouse Y") * ySpeed * 0.02f;

                y = ClampAngle(y, yMinLimit, yMaxLimit);

                Quaternion rotation = Quaternion.Euler(y, x, 0);

                distance = Mathf.Clamp(distance - Input.GetAxis("Mouse ScrollWheel") * 5, distanceMin, distanceMax);

                Vector3 negDistance = new Vector3(0.0f, 0.0f, -distance);
                Vector3 position = rotation * negDistance + target.position;

                _targetRotation = rotation;
                _targetPosition = position;
            }
        }

        public float ClampAngle(float angle, float min, float max)
        {
            if (angle < -360F)
                angle += 360F;
            if (angle > 360F)
                angle -= 360F;
            return Mathf.Clamp(angle, min, max);
        }
    }
}