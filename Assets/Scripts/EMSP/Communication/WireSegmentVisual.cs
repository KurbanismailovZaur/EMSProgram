﻿using EMSP.Mathematic;
using EMSP.Utility;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace EMSP.Communication
{
    [RequireComponent(typeof(CapsuleCollider))]
    public class WireSegmentVisual : MonoBehaviour
    {
        #region Entities
        #region Enums
        #endregion

        #region Delegates
        #endregion

        #region Structures
        #endregion

        #region Classes
        public class Factory
        {
            private Wire _generalWire;
            private float _segmentWidth = 0.02f;

            public Factory(Wire generaltWire)
            {
                _generalWire = generaltWire;
            }

            public WireSegmentVisual Create(Vector3 startPoint, Vector3 endPoint, int segmentNumber)
            {
                // Create
                WireSegmentVisual segment = new GameObject("WireSegment_" + segmentNumber).AddComponent<WireSegmentVisual>();

                // Transform and Collider
                CapsuleCollider collider = segment.GetComponent<CapsuleCollider>();
                segment.transform.position = startPoint + (endPoint - startPoint) / 2;
                segment.transform.LookAt(startPoint);
                segment.gameObject.layer = LayerMask.NameToLayer("CalculatedDataLayer");
                collider.height = (endPoint - startPoint).magnitude;
                collider.radius = _segmentWidth * 5; //multiple by 5 for more clickable colliders
                collider.direction = 2;

                // EventTrigger
                EventTrigger eventTrigger = segment.gameObject.AddComponent<EventTrigger>();
                var triggerEntries = new List<EventTrigger.Entry>();

                var pointerDownEntry = new EventTrigger.Entry
                {
                    eventID = EventTriggerType.PointerDown
                };

                pointerDownEntry.callback.AddListener((eventData) =>
                {
                    Mathematic.MathematicManager.Instance.Induction.ShowCalculatedFor(segment);
                });

                triggerEntries.Add(pointerDownEntry);
                eventTrigger.triggers = triggerEntries;

                // LineRenderer
                var lineRenderer = new GameObject("Line").AddComponent<LineRenderer>();
                lineRenderer.transform.SetParent(segment.transform, true);
                lineRenderer.widthMultiplier = _segmentWidth;
                lineRenderer.numCornerVertices = 4;
                lineRenderer.numCapVertices = 4;
                lineRenderer.useWorldSpace = false;
                lineRenderer.material = WiringManager.Instance.LineMaterial;
                lineRenderer.positionCount = 2;
                lineRenderer.SetPositions(new Vector3[] { startPoint, endPoint });

                //Initialize
                segment._id = segmentNumber;
                segment._generalWire = _generalWire;
                segment._line = lineRenderer;
                segment._defaultColor = lineRenderer.sharedMaterial.color;


                return segment;
            }
        }
        #endregion

        #region Interfaces
        #endregion
        #endregion

        #region Fields
        private int _id;
        private Wire _generalWire;

        private LineRenderer _line;
        private Color _defaultColor;

        private Dictionary<int, Color> _colorsByTime = new Dictionary<int, Color>(); // <timeIndex, color>; timeIndex = -1 - precomputed color

        private bool _colorChangedByClick = false;
        private bool _colorChangedByFilter = false;

        #endregion

        #region Events
        #endregion

        #region Behaviour
        #region Properties
        public int ID { get { return _id; } }

        public Wire GeneralWire { get { return _generalWire; } }

        #endregion

        #region Constructors
        private WireSegmentVisual() { }
        #endregion

        #region Methods

        public void SetColorByInductionValue(AmperageMode mode, int currentTimeStep)
        {
            //if (mode == AmperageMode.Precomputed)
            //{
            //    _line.material.color = _colorsByTime[-1];
            //}
            //else if (mode == AmperageMode.Computational)
            //{
            //    _line.material.color = _colorsByTime[currentTimeStep];
            //}
            //else
            //    Debug.LogError("Unexpected AmperageMode");

            _line.material.color = _colorsByTime[-1];
        }

        public void SetHighlight(Color color, bool fromClick = false, bool fromFilter = false)
        {
            if(fromFilter)
            {
                _colorChangedByFilter = true;
                if (_colorChangedByClick) return;
            }
            else if(fromClick)
            {
                _colorChangedByClick = true;
            }



            _line.material.color = color;
        }

        public void DisableHighlight(bool fromClick = false, bool fromFilter = false)
        {
            if(fromClick)
            {
                if(fromFilter)
                {
                    _colorChangedByClick = false;
                    _colorChangedByFilter = false;
                    _line.material.color = _defaultColor;
                    return;
                }

                _colorChangedByClick = false;
                if (_colorChangedByFilter)
                {
                    _line.material.color = MathematicManager.Instance.Induction.TransparentColor;
                    return;
                }
            }
            else if(fromFilter)
            {
                _colorChangedByFilter = false;
                if (_colorChangedByClick) return;
            }


            _line.material.color = _colorsByTime[-1];
        }

        public void FillGradientColors(Dictionary<int, Color> colorsByTime)
        {
            _colorsByTime = colorsByTime;
        }

        #endregion

        #region Indexers

        #endregion

        #region Events handlers

        #endregion
        #endregion
    }
}