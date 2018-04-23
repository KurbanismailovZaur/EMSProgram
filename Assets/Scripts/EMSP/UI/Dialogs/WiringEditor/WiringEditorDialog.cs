using EMSP.Communication;
using Numba;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace EMSP.UI.Dialogs.WiringEditor
{
    public class WiringEditorDialog : MonoSingleton<WiringEditorDialog>
    {
        #region Entities
        #region Enums
        #endregion

        #region Delegates
        #endregion

        #region Structures
        public class WireProperties
        {
            public float Amplitude;
            public float Frequency;
        }
        #endregion

        #region Classes
        #endregion

        #region Interfaces
        #endregion
        #endregion

        #region Fields
        [SerializeField]
        private Button _wireButtonPrefab;

        [SerializeField]
        private Button _addWireButtonPrefab;

        [SerializeField]
        private Button _addPointButtonPrefab;

        [SerializeField]
        private PointEditPanel _pointEditPanelPrefab;

        [SerializeField]
        private GameObject _deleteWireDialog;

        [SerializeField]
        private Button _deleteWireDialogYesButton;

        [SerializeField]
        private Button _deleteWireDialogNoButton;

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

        [SerializeField]
        private InputField _amplitudeInput;

        [SerializeField]
        private InputField _frequencyInput;

        public Dictionary<int, List<Vector3>> Wiring = new Dictionary<int, List<Vector3>>();
        public Dictionary<int, string> WiresNames = new Dictionary<int, string>();
        public Dictionary<int, WireProperties> WiresProperties = new Dictionary<int, WireProperties>();

        private CanvasGroup _canvasGroup;
        private GameObject _tabNavigationObject;
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
            _canvasGroup = GetComponent<CanvasGroup>();
        }

        public void StartWiringEditing(Wiring wiring, Action<Wiring> onWiringEdited)
        {
            _saveButton.onClick.RemoveAllListeners();
            _cancelButton.onClick.RemoveAllListeners();
            _closeButton.onClick.RemoveAllListeners();
            Show();

            // Wire buttons
            int wireCount = 0;
            if (wiring)
            {
                foreach (Wire wire in wiring)
                {
                    Wiring.Add(wireCount, new List<Vector3>());
                    WiresNames.Add(wireCount, wire.GetName());
                    WiresProperties.Add(wireCount, new WireProperties() { Amplitude = wire.Amplitude, Frequency = wire.Frequency });

                    foreach (Vector3 point in wire)
                    {
                        Wiring[wireCount].Add(point);
                    }

                    Button wireButton = Instantiate(_wireButtonPrefab);
                    wireButton.transform.SetParent(_wireButtonContainer, false);
                    wireButton.GetComponent<WireButton>().WireNumber = wireCount;
                    wireButton.GetComponent<WireButton>().WireName = WiresNames[wireCount];

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


                        _amplitudeInput.onValueChanged.RemoveAllListeners();
                        _amplitudeInput.onEndEdit.RemoveAllListeners();
                        _frequencyInput.onValueChanged.RemoveAllListeners();
                        _frequencyInput.onEndEdit.RemoveAllListeners();

                        _amplitudeInput.text = WiresProperties[wireNumber].Amplitude.ToString();
                        _frequencyInput.text = WiresProperties[wireNumber].Frequency.ToString();



                        _amplitudeInput.onValueChanged.AddListener((str) =>
                        {
                            float newAmplitude;
                            if (float.TryParse(str, out newAmplitude))
                            {
                                WiresProperties[wireNumber].Amplitude = newAmplitude;
                            }
                        });

                        _amplitudeInput.onEndEdit.AddListener((str) =>
                        {
                            if (string.IsNullOrEmpty(str) || str == "-")
                            {
                                WiresProperties[wireNumber].Amplitude = 0;
                                _amplitudeInput.text = 0.ToString();
                            }
                        });

                        _frequencyInput.onValueChanged.AddListener((str) =>
                        {
                            float newFrequency;
                            if (float.TryParse(str, out newFrequency))
                            {
                                WiresProperties[wireNumber].Frequency = newFrequency;
                            }
                        });


                        _frequencyInput.onEndEdit.AddListener((str) =>
                        {
                            if (string.IsNullOrEmpty(str) || str == "-")
                            {
                                WiringEditorDialog.Instance.WiresProperties[wireNumber].Frequency = 0;
                                _frequencyInput.text = 0.ToString();
                            }
                        });


                        int pointCount = 0;
                        foreach (var point in Wiring[wireNumber])
                        {
                            PointEditPanel editPanel = Instantiate(_pointEditPanelPrefab);
                            editPanel.transform.SetParent(_pointsContainer, false);

                            editPanel.Initialize(wireNumber, pointCount);
                            editPanel.DeletePointButton.onClick.AddListener(() =>
                            {
                                int wireNum = WireButton.CurrentActiveWireButton.WireNumber;
                                Vector3 _point = editPanel.CurrentValue;

                                Wiring[wireNum].RemoveAt(editPanel.GetComponent<RectTransform>().GetSiblingIndex());
                                StartCoroutine(WaitAndUpdatePointsNumber());
                                Destroy(editPanel.gameObject);
                            });
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
                            editPanel.DeletePointButton.onClick.AddListener(() =>
                            {
                                int wireNum = WireButton.CurrentActiveWireButton.WireNumber;
                                Vector3 _point = editPanel.CurrentValue;

                                Wiring[wireNum].RemoveAt(editPanel.GetComponent<RectTransform>().GetSiblingIndex());
                                StartCoroutine(WaitAndUpdatePointsNumber());
                                Destroy(editPanel.gameObject);
                            });
                            addPointButton.transform.SetAsLastSibling();

                            StartCoroutine(WaitAndMovePointsContainer(editPanel.GetComponent<RectTransform>().rect.height));
                        });
                    });

                    wireButton.GetComponent<WireButton>().DeleteWireButton.onClick.AddListener(() =>
                    {
                        int wireNumber = wireButton.GetComponent<WireButton>().WireNumber;

                        _deleteWireDialog.gameObject.SetActive(true);
                        _deleteWireDialogYesButton.onClick.RemoveAllListeners();
                        _deleteWireDialogNoButton.onClick.RemoveAllListeners();

                        _deleteWireDialogYesButton.onClick.AddListener(() =>
                        {
                            Wiring.Remove(wireNumber);
                            WiresNames.Remove(wireNumber);
                            WiresProperties.Remove(wireNumber);

                            int wbNumber = wireButton.GetComponent<RectTransform>().GetSiblingIndex();
                            if(!(wbNumber == 0))
                            {
                                _wireButtonContainer.GetChild(wbNumber - 1).GetComponent<Button>().onClick.Invoke();
                            }
                            else
                            {
                                for (int i = 0; i < _pointsContainer.childCount; ++i)
                                {
                                    Destroy(_pointsContainer.GetChild(i).gameObject);
                                }
                            }

                            Destroy(wireButton.gameObject);
                            _deleteWireDialog.gameObject.SetActive(false);
                        });

                        _deleteWireDialogNoButton.onClick.AddListener(() =>
                        {
                            _deleteWireDialog.gameObject.SetActive(false);
                        });
                    });

                    ++wireCount;
                }
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

                    _amplitudeInput.onValueChanged.RemoveAllListeners();
                    _amplitudeInput.onEndEdit.RemoveAllListeners();
                    _frequencyInput.onValueChanged.RemoveAllListeners();
                    _frequencyInput.onEndEdit.RemoveAllListeners();

                    _amplitudeInput.text = WiresProperties[wireNumber].Amplitude.ToString();
                    _frequencyInput.text = WiresProperties[wireNumber].Frequency.ToString();


                    _amplitudeInput.onValueChanged.AddListener((str) =>
                    {
                        float newAmplitude;
                        if (float.TryParse(str, out newAmplitude))
                        {
                            WiringEditorDialog.Instance.WiresProperties[wireNumber].Amplitude = newAmplitude;
                        }
                    });

                    _amplitudeInput.onEndEdit.AddListener((str) =>
                    {
                        if (string.IsNullOrEmpty(str) || str == "-")
                        {
                            WiringEditorDialog.Instance.WiresProperties[wireNumber].Amplitude = 0;
                            _amplitudeInput.text = 0.ToString();
                        }
                    });

                    _frequencyInput.onValueChanged.AddListener((str) =>
                    {
                        float newFrequency;
                        if (float.TryParse(str, out newFrequency))
                        {
                            WiringEditorDialog.Instance.WiresProperties[wireNumber].Frequency = newFrequency;
                        }
                    });

                    _frequencyInput.onEndEdit.AddListener((str) =>
                    {
                        if (string.IsNullOrEmpty(str) || str == "-")
                        {
                            WiringEditorDialog.Instance.WiresProperties[wireNumber].Frequency = 0;
                            _frequencyInput.text = 0.ToString();
                        }
                    });



                    int pointCount = 0;
                    foreach (var point in Wiring[wireNumber])
                    {
                        PointEditPanel editPanel = Instantiate(_pointEditPanelPrefab);
                        editPanel.transform.SetParent(_pointsContainer, false);

                        editPanel.Initialize(wireNumber, pointCount);
                        editPanel.DeletePointButton.onClick.AddListener(() =>
                        {
                            int wireNum = WireButton.CurrentActiveWireButton.WireNumber;
                            Vector3 _point = editPanel.CurrentValue;

                            Wiring[wireNum].RemoveAt(editPanel.GetComponent<RectTransform>().GetSiblingIndex());
                            StartCoroutine(WaitAndUpdatePointsNumber());
                            Destroy(editPanel.gameObject);
                        });
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
                        editPanel.DeletePointButton.onClick.AddListener(() =>
                        {
                            int wireNum = WireButton.CurrentActiveWireButton.WireNumber;
                            Vector3 _point = editPanel.CurrentValue;

                            Wiring[wireNum].RemoveAt(editPanel.GetComponent<RectTransform>().GetSiblingIndex());
                            StartCoroutine(WaitAndUpdatePointsNumber());
                            Destroy(editPanel.gameObject);
                        });
                        addPointButton.transform.SetAsLastSibling();

                        StartCoroutine(WaitAndMovePointsContainer(editPanel.GetComponent<RectTransform>().rect.height));
                    });
                });

                WireButton wireButtonComponent = wireButton.GetComponent<WireButton>();

                wireButtonComponent.DeleteWireButton.onClick.AddListener(() =>
                {
                    int wireNumber = wireButton.GetComponent<WireButton>().WireNumber;

                    _deleteWireDialog.gameObject.SetActive(true);
                    _deleteWireDialogYesButton.onClick.RemoveAllListeners();
                    _deleteWireDialogNoButton.onClick.RemoveAllListeners();

                    _deleteWireDialogYesButton.onClick.AddListener(() =>
                    {
                        Wiring.Remove(wireNumber);
                        WiresNames.Remove(wireNumber);
                        WiresProperties.Remove(wireNumber);

                        int wbNumber = wireButton.GetComponent<RectTransform>().GetSiblingIndex();
                        if (!(wbNumber == 0))
                        {
                            _wireButtonContainer.GetChild(wbNumber - 1).GetComponent<Button>().onClick.Invoke();
                        }
                        else
                        {
                            for (int i = 0; i < _pointsContainer.childCount; ++i)
                            {
                                Destroy(_pointsContainer.GetChild(i).gameObject);
                            }
                        }

                        Destroy(wireButton.gameObject);
                        _deleteWireDialog.gameObject.SetActive(false);
                    });

                    _deleteWireDialogNoButton.onClick.AddListener(() =>
                    {
                        _deleteWireDialog.gameObject.SetActive(false);
                    });
                });

                var wiresNumbers = Wiring.Keys.ToList();
                int numberOfNewWire = (wiresNumbers.Count == 0)? 0: wiresNumbers.Max() + 1;

                Wiring.Add(numberOfNewWire, new List<Vector3>());
                WiresNames.Add(numberOfNewWire, string.Format("Wire{0}", numberOfNewWire));
                WiresProperties.Add(numberOfNewWire, new WireProperties() { Amplitude = 0, Frequency = 0 });


                wireButtonComponent.WireNumber = numberOfNewWire;
                wireButtonComponent.WireName = WiresNames[numberOfNewWire];

                addWireButton.transform.SetAsLastSibling();

                wireButton.onClick.Invoke();
                StartCoroutine(WaitAndMoveWireButtonsContainer(wireButton.GetComponent<RectTransform>().rect.width));
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

        private IEnumerator WaitAndMoveWireButtonsContainer(float offset)
        {
            yield return null;

            var wcParentWidth = _wireButtonContainer.parent.GetComponent<RectTransform>().rect.width;
            var wcWidth = _wireButtonContainer.rect.width;

            if (wcWidth > wcParentWidth)
            {
                _wireButtonContainer.anchoredPosition3D = new Vector3(
                       _wireButtonContainer.anchoredPosition3D.x - offset - 5,
                       0, 0);
            }
        }

        private IEnumerator WaitAndMovePointsContainer(float offset)
        {
            yield return null;

            var pcParentHeight = _pointsContainer.parent.GetComponent<RectTransform>().rect.height;
            var pcHeight = _pointsContainer.rect.height;

            if (pcHeight > pcParentHeight)
            {
                _pointsContainer.anchoredPosition3D = new Vector3(
                       0,
                       _pointsContainer.anchoredPosition3D.y + offset + 5,
                       0);
            }
        }

        private IEnumerator WaitAndUpdatePointsNumber()
        {
            yield return null;

            foreach (var editPointPanel in _pointsContainer.GetComponentsInChildren<PointEditPanel>())
            {
                editPointPanel.UpdatePointNumberAndSelectableImmediate();
            }
        }

        private void Show()
        {
            _canvasGroup.alpha = 1f;
            _canvasGroup.interactable = true;
            _canvasGroup.blocksRaycasts = true;

            ActivateTabNavigation();

            OnDialogActivated();
        }

        private void Hide()
        {
            _canvasGroup.alpha = 0f;
            _canvasGroup.interactable = false;
            _canvasGroup.blocksRaycasts = false;

            DeactivateTabNavigation();
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
            {
                Wiring.Clear();
                WiresNames.Clear();
            }

            WireButton.OnEditorClosing();
            OnDialogActivated = () => { };
            OnDialogDeactivated = () => { };

            Hide();
        }

        private void SaveAndClose(Action<Wiring> onWiringEdited)
        {
            Close(false);

            if (onWiringEdited != null)
                onWiringEdited(DictionaryToWiring());

            Wiring.Clear();
            WiresNames.Clear();
            WiresProperties.Clear();
        }

        private Wiring DictionaryToWiring()
        {
            Wiring wiring = new Wiring.Factory().Create();

            foreach (var kvp in Wiring)
            {
                // TODO: change it
                Wire wire = wiring.CreateWire(WiresNames[kvp.Key], WiresProperties[kvp.Key].Amplitude, WiresProperties[kvp.Key].Frequency);
                wire.AddRange(kvp.Value);
            }

            return wiring;
        }

        private void ActivateTabNavigation()
        {
            _tabNavigationObject = new GameObject("TabNavigation");
            _tabNavigationObject.AddComponent<TabNavigation>();
        }

        private void DeactivateTabNavigation()
        {
            Destroy(_tabNavigationObject);
        }

        #endregion

        #region Indexers
        #endregion

        #region Events handlers
        #endregion
        #endregion

        private void PrintWiring(Wiring wiring)
        {
            foreach(Wire wire in wiring)
            {
                Debug.Log("==========");
                Debug.Log(wire.GetName());
                foreach(var point in wire)
                {
                    Debug.Log(string.Format("x: {0}; y: {1}; z:{2}", point.x, point.y, point.z));
                }
            }
        }
    }
}
