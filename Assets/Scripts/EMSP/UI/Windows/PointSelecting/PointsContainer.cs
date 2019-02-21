using EMSP.UI.Toggle;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EMSP.UI.Windows.PointSelecting
{
    public class PointsContainer : MonoBehaviour
    {
        [SerializeField]
        private CanvasGroup _canvasGroup;

        public void ToggleBehaviour_StateChanged(ToggleBehaviour toggleBehaviour, bool state)
        {
            _canvasGroup.interactable = state;
        }
    }
}
