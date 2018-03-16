using EMSP.Communication;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace EMSP.UI.Dialogs.WiringEditor
{
    public class WiringEditorDialog : MonoBehaviour
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
        public Button WireButtonPrefab;
        public PointEditPanel PointEditPanelPrefab;

        public RectTransform WireButtonContainer;
        public RectTransform PointsContainer;

        private Dictionary<int, List<Vector3>> _wiring = new Dictionary<int, List<Vector3>>();
        private CanvasGroup _canvasGroup;
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
            _canvasGroup = GetComponent<CanvasGroup>();
        }

        public void StartWiringEditing(Wiring wiring) 
        {
            _canvasGroup.alpha = 1;

            int wireCount = 0;
            foreach(Wire wire in wiring)
            {
                Button wireButton = Instantiate(WireButtonPrefab);
                wireButton.transform.parent = WireButtonContainer;
                wireButton.GetComponentInChildren<Text>().text = string.Format("Wire_{0}", wireCount);

                wireButton.onClick.AddListener(() =>
                {
                    for(int i = 0; i < PointsContainer.childCount; ++i)
                    {
                        Destroy(PointsContainer.GetChild(i).gameObject);
                    }

                    int pointCount = 0;
                    foreach (Vector3 point in wire)
                    {
                        PointEditPanel editPanel = Instantiate(PointEditPanelPrefab);
                        editPanel.transform.parent = PointsContainer;

                        editPanel.Initialize(pointCount, point);
                        ++pointCount;
                    }
                });

                ++wireCount;
            }
        }

        public void Save()
        {

        }

        public void Close()
        {
            for (int i = 0; i < WireButtonContainer.childCount; ++i)
            {
                Destroy(WireButtonContainer.GetChild(i).gameObject);
            }

            _canvasGroup.alpha = 0;
        }

        #endregion

        #region Indexers
        #endregion

        #region Events handlers
        #endregion
        #endregion

        public void Test()
        {
            Wiring.Factory factory = new Wiring.Factory();
            
        }
    }
}
