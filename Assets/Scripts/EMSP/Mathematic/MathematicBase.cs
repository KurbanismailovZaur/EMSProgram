using EMSP.Communication;
using EMSP.Utility.Extensions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace EMSP.Mathematic
{
    public abstract class MathematicBase : MonoBehaviour
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
        public class CalculatedEvent : UnityEvent<MathematicBase> { }

        [Serializable]
        public class DestroyedEvent : UnityEvent<MathematicBase> { }

        [Serializable]
        public class VisibilityChangedEvent : UnityEvent<MathematicBase, bool> { }

        [Serializable]
        public class CurrentValueFilterRangeChangedEvent : UnityEvent<MathematicBase, Range> { }
        #endregion

        #region Interfaces
        #endregion
        #endregion

        #region Fields
      
        #endregion

        #region Events
        public CalculatedEvent Calculated = new CalculatedEvent();

        public DestroyedEvent Destroyed = new DestroyedEvent();

        public VisibilityChangedEvent VisibilityChanged = new VisibilityChangedEvent();

        public CurrentValueFilterRangeChangedEvent CurrentValueFilterRangeChanged = new CurrentValueFilterRangeChangedEvent();
        #endregion

        #region Behaviour
        #region Properties
        protected abstract MathematicCalculatorBase Calculator { get; }
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