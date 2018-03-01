using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Numba.UI.Menu
{
    public class Panel : MonoBehaviour
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
        private List<Group> _groups;

        private bool _autoShowContexts;

        private Group _activeGroup;
        
        private Coroutine _hideContextOnMouseDownRoutine;
        #endregion

        #region Events
        #endregion

        #region Behaviour
        #region Properties
        public ReadOnlyCollection<Group> Groups { get { return _groups.AsReadOnly(); } }
        #endregion

        #region Constructors
        #endregion

        #region Methods
        private void Awake()
        {
            foreach (Group menuGroup in _groups)
            {
                menuGroup.Button.onClick.AddListener(Button_OnClick);
                menuGroup.Context.Showed.AddListener(MenuContext_Showed);
                menuGroup.Context.Hided.AddListener(MenuContext_Hided);
            }
        }

        public void StartAutoShowContexts()
        {
            foreach (Group menuGroup in _groups)
            {
                menuGroup.ShowContextOnMouseEnter = true;
            }

            _autoShowContexts = true;

            _hideContextOnMouseDownRoutine = StartCoroutine(HideContextOnMouseDownRoutine());
        }

        public void StopAutoShowContexts()
        {
            foreach (Group menuGroup in _groups)
            {
                menuGroup.ShowContextOnMouseEnter = false;
            }

            _autoShowContexts = false;

            StopCoroutine(_hideContextOnMouseDownRoutine);
        }

        public void ToggleAutoShowContexts()
        {
            if (!_autoShowContexts)
            {
                StartAutoShowContexts();
            }
            else
            {
                StopAutoShowContexts();
            }
        }

        public IEnumerator HideContextOnMouseDownRoutine()
        {
            while (true)
            {
                if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
                {
                    if (!IsChildOfPanel(GetGameObjectUnderMouseCursor()))
                    {
                        HideActiveContextAndStopAutoShow();
                    }
                }

                yield return null;
            }
        }

        private bool IsChildOfPanel(GameObject child)
        {
            if (!child)
            {
                return false;
            }

            return child.transform.IsChildOf(transform);
        }

        private GameObject GetGameObjectUnderMouseCursor()
        {
            PointerEventData pointerEventData = new PointerEventData(EventSystem.current)
            {
                pointerId = -1
            };

            pointerEventData.position = Input.mousePosition;

            List<RaycastResult> results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(pointerEventData, results);

            return results.Count == 0 ? null : results[0].gameObject;
        }

        public void HideActiveContextAndStopAutoShow()
        {
            if (_activeGroup != null)
            {
                _activeGroup.Context.Hide();
                StopAutoShowContexts();
            }
        }
        #endregion

        #region Events handlers 
        private void Button_OnClick()
        {
            ToggleAutoShowContexts();
        }

        private void MenuContext_Showed(Context menuContext)
        {
            if (_activeGroup)
            {
                _activeGroup.Context.Hide();
            }

            _activeGroup = (Group)menuContext.ContextContainer;
        }

        private void MenuContext_Hided(Context menuContext)
        {
            _activeGroup = null;
        }
        #endregion
        #endregion
    }
}