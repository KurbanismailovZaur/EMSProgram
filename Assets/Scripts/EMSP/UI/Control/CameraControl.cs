using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EMSP.UI.Control
{
	public class CameraControl : MonoBehaviour 
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
        private OrbitController _orbitController;

        [SerializeField]
        [Range(0f, 64f)]
        private float _deltaValue = 1f;

        [SerializeField]
        [Range(0f, 32f)]
        private float _distanceDelta = 1f;
		#endregion
		
		#region Events
		#endregion
		
		#region Behaviour
		#region Properties
		#endregion
		
		#region Constructors
		#endregion
		
		#region Methods
        private void MoveLeft()
        {
            _orbitController.ApplyDeltasToTransform(new Vector2(_deltaValue, 0f) * Time.deltaTime);
        }

        private void MoveRight()
        {
            _orbitController.ApplyDeltasToTransform(new Vector2(-_deltaValue, 0f) * Time.deltaTime);
        }

        private void MoveUp()
        {
            _orbitController.ApplyDeltasToTransform(new Vector2(0f, -_deltaValue) * Time.deltaTime);
        }

        private void MoveDown()
        {
            _orbitController.ApplyDeltasToTransform(new Vector2(0f, _deltaValue) * Time.deltaTime);
        }

        private void ZoomIn()
        {
            _orbitController.Distance -= _distanceDelta * Time.deltaTime;
        }

        private void ZoomOut()
        {
            _orbitController.Distance += _distanceDelta * Time.deltaTime;
        }
        #endregion

        #region Indexers
        #endregion

        #region Events handlers
        public void RepeatButton_Repeated(RepeatButton repeatButton, RepeatButton.Type type)
        {
            switch (type)
            {
                case RepeatButton.Type.Left:
                    MoveLeft();
                    break;
                case RepeatButton.Type.Up:
                    MoveUp();
                    break;
                case RepeatButton.Type.Right:
                    MoveRight();
                    break;
                case RepeatButton.Type.Down:
                    MoveDown();
                    break;
                case RepeatButton.Type.ZoomIn:
                    ZoomIn();
                    break;
                case RepeatButton.Type.ZoomOut:
                    ZoomOut();
                    break;
            }
        }
		#endregion
		#endregion
	}
}