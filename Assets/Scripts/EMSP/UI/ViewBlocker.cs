using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace EMSP.UI
{
    [RequireComponent(typeof(RawImage))]
	public class ViewBlocker : MonoBehaviour 
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
        private RawImage _image;
        #endregion

        #region Events
        #endregion

        #region Behaviour
        #region Properties
        #endregion

        #region Constructors
        #endregion

        #region Methods
        private void Awake()
        {
            _image = GetComponent<RawImage>();
        }

        public void BlockView()
        {
            Color color = _image.color;
            color.a = 1f;
            _image.color = color;

            _image.raycastTarget = true;
        }

        public void UnblockView()
        {
            Color color = _image.color;
            color.a = 0f;
            _image.color = color;

            _image.raycastTarget = false;
        }
        #endregion

        #region Indexers
        #endregion

        #region Events handlers
        #endregion
        #endregion
    }
}