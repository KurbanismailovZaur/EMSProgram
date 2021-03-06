﻿using UnityEngine;
using System.Collections.Generic;
using System;
using System.Threading;

namespace Numba.Threading
{
    /// <summary>
    /// This dispatcher give you ability to run your async code in main thread.
    /// </summary>
    public class ThreadDispatcher : MonoBehaviour
    {
        #region Singleton
        private static ThreadDispatcher _instance;
        public static ThreadDispatcher Instance { get { return _instance; } }
        #endregion

        /// <summary>
        /// The main thread ID.
        /// Useful in some situation.
        /// </summary>
        private int _mainThreadID;

        /// <summary>
        /// Queue of actions that we need execute.
        /// </summary>
        private Queue<Action> _mainQueue = new Queue<Action>();

        /// <summary>
        /// Represent state of dispatcher.
        /// Busy means that actions execute in update right now.
        /// </summary>
        private bool _isBusy;

        /// <summary>
        /// Get ID of the main thread.
        /// </summary>
        public int MainThreadID { get { return _mainThreadID; } }

        /// <summary>
        /// Check if current thread is main.
        /// </summary>
        public bool IsMainThread { get { return Thread.CurrentThread.ManagedThreadId == _mainThreadID; } }

        /// <summary>
        /// In awake we save this dispatcher to static field.
        /// Later, in oother scripts we use it (like singletons).
        /// </summary>
        private void Awake()
        {
            _instance = this;

            _mainThreadID = Thread.CurrentThread.ManagedThreadId;
        }

        /// <summary>
        /// Add new action to actions queue.
        /// </summary>
        /// <param name="method"></param>
        public void InvokeFromMainThread(Action method)
        {
            _mainQueue.Enqueue(method);
        }

        private void Update()
        {
            // Executing actions.
            while (_mainQueue.Count > 0)
            {
                _mainQueue.Dequeue()();
            }
        }
    }
}