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
    public class WireSegment : MonoBehaviour
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

            public WireSegment Create(Vector3 startPoint, Vector3 endPoint, int segmentNumber)
            {
                // Create and initialize
                WireSegment segment = new GameObject("WireSegment_" + segmentNumber).AddComponent<WireSegment>();
                segment._id = segmentNumber;
                segment._generalWire = _generalWire;

                // Transform and Collider
                CapsuleCollider collider = segment.GetComponent<CapsuleCollider>();
                segment.gameObject.layer = LayerMask.NameToLayer("CalculatedDataLayer");
                segment.transform.position = startPoint + (endPoint - startPoint) / 2;
                segment.transform.LookAt(startPoint);
                collider.height = (endPoint - startPoint).magnitude;
                collider.radius = _segmentWidth;
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
        #endregion

        #region Events
        #endregion

        #region Behaviour
        #region Properties
        public int ID { get { return _id; } }

        public Wire GeneralWire { get { return _generalWire; } }

        #endregion

        #region Constructors

        #endregion

        #region Methods

        #endregion

        #region Indexers

        #endregion

        #region Events handlers

        #endregion
        #endregion
    }
}