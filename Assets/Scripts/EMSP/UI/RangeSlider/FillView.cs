using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FillView : MonoBehaviour
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
    private RectTransform _fillRect;
    private RectTransform _rectTransform;
    #endregion

    #region Events
    #endregion

    #region Behaviour
    #region Properties
    #endregion

    #region Constructors
    #endregion

    #region Methods
    private void Start()
    {
        _rectTransform = GetComponent<RectTransform>();
    }

    private void Update()
    {
        _rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, _fillRect.rect.height + 20f);
        _rectTransform.ForceUpdateRectTransforms();
        _rectTransform.anchoredPosition3D = new Vector3(_rectTransform.anchoredPosition3D.x, _fillRect.anchoredPosition3D.y, _rectTransform.anchoredPosition3D.z);
    }

    #endregion

    #region Indexers
    #endregion

    #region Events handlers
    #endregion
    #endregion
}
