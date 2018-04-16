using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using System;
using UnityEngine.UI;
using EMSP.Timing;

namespace EMSP.UI
{
    public class TimeLine : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        #region Entities
        #region Enums
        #endregion

        #region Delegates
        #endregion

        #region Structures
        #endregion

        #region Classes
        [Serializable]
        public class TimeLineEvent : UnityEvent<TimeLine, float> { }
        #endregion

        #region Interfaces
        #endregion
        #endregion

        #region Fields
        [SerializeField]
        private RectTransform _timeStepsRect;
        [SerializeField]
        private RectTransform _handleRect;
        [SerializeField]
        private RectTransform _timeStepPrefab;
        [SerializeField]
        private Text _textField;

        [SerializeField]
        private GameObject _playButton;
        [SerializeField]
        private GameObject _stopButton;


        private bool _isPlaying = false;
        private float _startTime;
        private float _endTime;
        private int _stepCount;


        private float m_Width;
        private float m_TimePerPixel;
        private float m_StepWidth;
        private float m_TimePerStep;
        private int m_TimeStepSignCount;
        private float m_InternalTime;

        private int m_ScreenWidth;
        private int m_ScreenHeight;
        

        private GameObject[] m_TimeSteps;
        private bool m_IsDragging = false;
        #endregion

        #region Events

        public TimeLineEvent Changed = new TimeLineEvent();
        #endregion

        #region Behaviour
        #region Properties
        public bool IsPlaying { get { return _isPlaying; } }
        #endregion

        #region Constructors
        #endregion

        #region Methods
        private void Start()
        {
            SetTimeParameters(TimeManager.Instance.StartTime, TimeManager.Instance.EndTime, TimeManager.Instance.StepsCount);
        }

        public void SetTimeParameters(float start, float end, int stepCount)
        {
            if (start < 0 || end <= 0 || start == end || stepCount < 1)
                throw new ArgumentOutOfRangeException("Incorrect time parameters");

            if (_isPlaying) Stop();

            _startTime = start;
            _endTime = end;
            _stepCount = stepCount;
            m_TimeStepSignCount = stepCount + 1;
            m_InternalTime = 0;
            _handleRect.anchoredPosition3D = Vector3.zero;
            _textField.text = _startTime.ToString();

            CalculateInternalValues();
            DrawTimeSteps();
        }

        public void SetCurrentTime(float time)
        {
            bool hasChanged = true;

            if (time < _startTime)
                time = _startTime;
            else if (time > _endTime)
                time = _endTime;

            int stepCountsInTime = Convert.ToInt32(time / m_TimePerStep) - Convert.ToInt32(_startTime / m_TimePerStep);
            float internalTimeCandidate = stepCountsInTime * m_TimePerStep;

            if (m_InternalTime == internalTimeCandidate)
            {
                if (m_IsDragging) return;
                hasChanged = false;
            }

            m_InternalTime = internalTimeCandidate;

            _handleRect.anchoredPosition3D = new Vector3(internalTimeCandidate / m_TimePerPixel, 0, 0);

            if (hasChanged)
            {
                _textField.text = (m_InternalTime + _startTime).ToString();
                Changed.Invoke(this, m_InternalTime + _startTime);
            }
        }

        public void Play()
        {
            if (_isPlaying) return;

            _isPlaying = true;
            StartCoroutine(PlayingProcess());

            _playButton.SetActive(false);
            _stopButton.SetActive(true);
        }

        public void Stop()
        {
            if (!_isPlaying) return;

            _isPlaying = false;

            _playButton.SetActive(true);
            _stopButton.SetActive(false);
        }

        private void Awake()
        {
            m_ScreenWidth = Screen.width;
            m_ScreenHeight = Screen.height;
        }

        private void Update()
        {
            if (m_ScreenWidth != Screen.width || m_ScreenHeight != Screen.height)
            {
                CalculateInternalValues();
                DrawTimeSteps();
                SetCurrentTime(m_InternalTime + _startTime);
            }
        }

        private void CalculateInternalValues()
        {
            m_ScreenWidth = Screen.width;
            m_ScreenHeight = Screen.height;

            m_Width = _timeStepsRect.rect.width;
            m_TimePerPixel = (_endTime - _startTime) / m_Width;
            m_StepWidth = m_Width / _stepCount;
            m_TimePerStep = (_endTime - _startTime) / _stepCount;
        }

        private float GetTimeFromAnchoredPosition(float posX)
        {
            if (posX <= 0)
                return _startTime;
            else if (posX >= _timeStepsRect.rect.width)
                return _endTime;
            else
                return _startTime + posX * m_TimePerPixel;
        }

        private void DrawTimeSteps()
        {
            Clear();

            m_TimeSteps = new GameObject[m_TimeStepSignCount];

            for (int i = 0; i < m_TimeStepSignCount; ++i)
            {
                RectTransform newStep = Instantiate(_timeStepPrefab);
                newStep.SetParent(_timeStepsRect, false);
                newStep.anchoredPosition3D = new Vector3(m_StepWidth * i, 0, 0);

                m_TimeSteps[i] = newStep.gameObject;
            }
        }

        private void Clear()
        {
            if (m_TimeSteps == null) return;

            foreach(var go in m_TimeSteps)
            {
                Destroy(go);
            }

            m_TimeSteps = null;
        }

        private IEnumerator PlayingProcess()
        {
            while (_isPlaying)
            {
                yield return new WaitForSeconds(m_TimePerStep);
                if (!_isPlaying) break;

                float time = _startTime + m_InternalTime + m_TimePerStep;
                if (time > _endTime) time = _startTime;
                SetCurrentTime(time);
            }
            yield return null;
        }

        #endregion

        #region Indexers
        #endregion

        #region Events handlers
        public void OnPointerDown(PointerEventData eventData)
        {
            _handleRect.position = eventData.pressPosition;
            _handleRect.ForceUpdateRectTransforms();
            SetCurrentTime(GetTimeFromAnchoredPosition(_handleRect.anchoredPosition3D.x));
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            m_IsDragging = true;
        }

        float _dragDelta;
        public void OnDrag(PointerEventData eventData)
        {
            _dragDelta += eventData.delta.x;
            if (Math.Abs(_dragDelta) >= m_StepWidth)
            {         
                SetCurrentTime(GetTimeFromAnchoredPosition(_handleRect.anchoredPosition3D.x + m_StepWidth * Math.Sign(_dragDelta)));
                _dragDelta = 0;
            }
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            m_IsDragging = false;
            _dragDelta = 0;
        }

        #endregion
        #endregion

        public float StTime;
        public float EnTime;
        public int StCount;
        [ContextMenu("SetTimeParameters")]
        void Test()
        {
            Changed.RemoveAllListeners();

            SetTimeParameters(StTime, EnTime, StCount);

            Changed.AddListener((tl, t) =>
            {
                Debug.Log(t.ToString());
            });
        }

        public float CurTime;
        [ContextMenu("Set Time")]
        void Test2()
        {
            SetCurrentTime(CurTime);
        }
    }
}