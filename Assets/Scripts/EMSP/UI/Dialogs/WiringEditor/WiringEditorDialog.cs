﻿using EMSP.Communication;
using Numba;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
        public static WiringEditorDialog Instance;

        [SerializeField]
        private Button _wireButtonPrefab;
        [SerializeField]
        private Button _addWireButtonPrefab;
        [SerializeField]
        private Button _addPointButtonPrefab;
        [SerializeField]
        private PointEditPanel _pointEditPanelPrefab;


        [SerializeField]
        private Button _saveButton;
        [SerializeField]
        private Button _cancelButton;
        [SerializeField]
        private Button _closeButton;

        [SerializeField]
        private RectTransform _wireButtonContainer;
        [SerializeField]
        private RectTransform _pointsContainer;

        public Dictionary<int, List<Vector3>> Wiring = new Dictionary<int, List<Vector3>>();
        private CanvasGroup _canvasGroup;

        #endregion

        #region Events

        public event Action OnDialogActivated = () => { };
        public event Action OnDialogDeactivated = () => { };

        #endregion

        #region Behaviour
        #region Properties
        #endregion

        #region Constructors
        #endregion

        #region Methods

        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            else
                DestroyImmediate(this);
        }

        private void Start()
        {
            _canvasGroup = GetComponent<CanvasGroup>();

            Wiring w = new Communication.Wiring.Factory().Create();

            for (int i = 0; i < 3; i++)
            {
                Wire wire = w.CreateWire();

                for (int j = 0; j < UnityEngine.Random.Range(1, 5); j++)
                {
                    wire.Add(UnityEngine.Random.value, UnityEngine.Random.value, UnityEngine.Random.value);
                }
            }

            StartWiringEditing(w, (we) =>
            {
                foreach (var wire1 in we.Values.ToList())
                {
                    foreach (var point in wire1)
                    {
                        Debug.Log(point);
                    }
                }
            });
        }
    

        public void StartWiringEditing(Wiring wiring, Action<Dictionary<int, List<Vector3>>> onWiringEdited)
        {
            _saveButton.onClick.RemoveAllListeners();
            _cancelButton.onClick.RemoveAllListeners();
            _closeButton.onClick.RemoveAllListeners();
            Show();

            // Wire buttons
            int wireCount = 0;
            foreach (Wire wire in wiring)
            {
                Wiring.Add(wireCount, new List<Vector3>());
                foreach (Vector3 point in wire)
                {
                    Wiring[wireCount].Add(point);
                }

                Button wireButton = Instantiate(_wireButtonPrefab);
                wireButton.transform.SetParent(_wireButtonContainer, false);
                wireButton.GetComponentInChildren<Text>().text = string.Format(" {0}", wireCount);
                wireButton.GetComponent<WireButton>().WireNumber = wireCount;

                wireButton.onClick.AddListener(() =>
                {
                    wireButton.GetComponent<WireButton>().OnClick();
                });

                wireButton.onClick.AddListener(() =>
                {
                    for (int i = 0; i < _pointsContainer.childCount; ++i)
                    {
                        Destroy(_pointsContainer.GetChild(i).gameObject);
                    }

                    int wireNumber = wireButton.GetComponent<WireButton>().WireNumber;
                    int pointCount = 0;
                    foreach (var point in Wiring[wireNumber])
                    {
                        PointEditPanel editPanel = Instantiate(_pointEditPanelPrefab);
                        editPanel.transform.SetParent(_pointsContainer, false);

                        editPanel.Initialize(wireNumber, pointCount);
                        ++pointCount;
                    }

                    Button addPointButton = Instantiate(_addPointButtonPrefab);
                    addPointButton.transform.SetParent(_pointsContainer, false);
                    addPointButton.onClick.AddListener(() =>
                    {
                        int _wireNumber = WireButton.CurrentActiveWireButton.WireNumber;
                        Wiring[_wireNumber].Add(Vector3.zero);

                        PointEditPanel editPanel = Instantiate(_pointEditPanelPrefab);
                        editPanel.transform.SetParent(_pointsContainer, false);

                        editPanel.Initialize(_wireNumber, Wiring[_wireNumber].Count - 1);
                        addPointButton.transform.SetAsLastSibling();
                    });
                });

                wireButton.GetComponent<WireButton>().DeleteWireButton.onClick.AddListener(() =>
                {
                    int wireNumber = wireButton.GetComponent<WireButton>().WireNumber;
                    Wiring.Remove(wireNumber);

                    for (int i = 0; i < _pointsContainer.childCount; ++i)
                    {
                        Destroy(_pointsContainer.GetChild(i).gameObject);
                    }

                    Destroy(wireButton.gameObject);
                });

                ++wireCount;
            }

            // AddWire Button
            Button addWireButton = Instantiate(_addWireButtonPrefab);
            addWireButton.transform.SetParent(_wireButtonContainer, false);
            addWireButton.onClick.AddListener(() =>
            {
                Button wireButton = Instantiate(_wireButtonPrefab);
                wireButton.transform.SetParent(_wireButtonContainer, false);

                wireButton.onClick.AddListener(() =>
                {
                    wireButton.GetComponent<WireButton>().OnClick();
                });

                wireButton.onClick.AddListener(() =>
                {
                    for (int i = 0; i < _pointsContainer.childCount; ++i)
                    {
                        Destroy(_pointsContainer.GetChild(i).gameObject);
                    }

                    int wireNumber = wireButton.GetComponent<WireButton>().WireNumber;
                    int pointCount = 0;
                    foreach (var point in Wiring[wireNumber])
                    {
                        PointEditPanel editPanel = Instantiate(_pointEditPanelPrefab);
                        editPanel.transform.SetParent(_pointsContainer, false);

                        editPanel.Initialize(wireNumber, pointCount);
                        ++pointCount;
                    }

                    Button addPointButton = Instantiate(_addPointButtonPrefab);
                    addPointButton.transform.SetParent(_pointsContainer, false);
                    addPointButton.onClick.AddListener(() =>
                    {
                        int _wireNumber = WireButton.CurrentActiveWireButton.WireNumber;
                        Wiring[_wireNumber].Add(Vector3.zero);

                        PointEditPanel editPanel = Instantiate(_pointEditPanelPrefab);
                        editPanel.transform.SetParent(_pointsContainer, false);

                        editPanel.Initialize(_wireNumber, Wiring[_wireNumber].Count - 1);
                        addPointButton.transform.SetAsLastSibling();
                    });
                });

                wireButton.GetComponent<WireButton>().DeleteWireButton.onClick.AddListener(() =>
                {
                    int wireNumber = wireButton.GetComponent<WireButton>().WireNumber;
                    Wiring.Remove(wireNumber);

                    for (int i = 0; i < _pointsContainer.childCount; ++i)
                    {
                        Destroy(_pointsContainer.GetChild(i).gameObject);
                    }

                    Destroy(wireButton.gameObject);
                });


                var wireNumbers = Wiring.Keys.ToList();
                int numberOfNewWire = wireNumbers[wireNumbers.Count - 1] + 1;

                Wiring.Add(numberOfNewWire, new List<Vector3>());

                wireButton.GetComponent<WireButton>().WireNumber = numberOfNewWire;
                wireButton.GetComponentInChildren<Text>().text = string.Format(" {0}", numberOfNewWire);

                addWireButton.transform.SetAsLastSibling();
            });


            _saveButton.onClick.AddListener(() =>
            {
                SaveAndClose(onWiringEdited);
            });

            _cancelButton.onClick.AddListener(() =>
            {
                Close();
            });

            _closeButton.onClick.AddListener(() =>
            {
                Close();
            });

            if (_wireButtonContainer.childCount > 0)
                _wireButtonContainer.GetChild(0).GetComponent<Button>().onClick.Invoke();
        }

        private void Show()
        {
            _canvasGroup.alpha = 1f;
            _canvasGroup.interactable = true;
            _canvasGroup.blocksRaycasts = true;

            OnDialogActivated();
        }

        private void Hide()
        {
            _canvasGroup.alpha = 0f;
            _canvasGroup.interactable = false;
            _canvasGroup.blocksRaycasts = false;
        }

        private void Close(bool needClearChangedWiring = true)
        {
            for (int i = 0; i < _pointsContainer.childCount; ++i)
            {
                Destroy(_pointsContainer.GetChild(i).gameObject);
            }

            for (int i = 0; i < _wireButtonContainer.childCount; ++i)
            {
                Destroy(_wireButtonContainer.GetChild(i).gameObject);
            }

            OnDialogDeactivated();

            if (needClearChangedWiring)
                Wiring.Clear();

            WireButton.OnEditorClosing();
            OnDialogActivated = () => { };
            OnDialogDeactivated = () => { };

            Hide();
        }

        private void SaveAndClose(Action<Dictionary<int, List<Vector3>>> onWiringEdited)
        {
            Close(false);

            if (onWiringEdited != null)
                onWiringEdited(Wiring);

            Wiring.Clear();
        }

        #endregion

        #region Indexers
        #endregion

        #region Events handlers
        #endregion
        #endregion
    }
}
