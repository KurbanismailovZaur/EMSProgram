using EMSP.Mathematic;
using EMSP.Timing;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mynamespace
{
	public class Test : MonoBehaviour 
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
        private Coroutine _playingRoutine;
        #endregion

        #region Events
        #endregion

        #region Behaviour
        #region Properties
        #endregion

        #region Constructors
        #endregion

        #region Methods
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (_playingRoutine == null)
                {
                    _playingRoutine = StartCoroutine(PlayingRoutine());
                }
                else
                {
                    StopCoroutine(_playingRoutine);
                    _playingRoutine = null;
                }
            }
        }

        private IEnumerator PlayingRoutine()
        {
            while (true)
            {
                yield return new WaitForSeconds(TimeManager.Instance.Delta);

                TimeManager.Instance.MoveTimeToNextStep();
            }
        }
        #endregion

        #region Indexers
        #endregion

        #region Events handlers
        #endregion
        #endregion
    }
}