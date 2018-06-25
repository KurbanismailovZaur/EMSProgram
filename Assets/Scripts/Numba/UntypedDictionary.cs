﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Numba
{
	public class UntypedDictionary<T> : IEnumerable<KeyValuePair<T, object>>
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
        private Dictionary<T, object> _dictionary;
        #endregion

        #region Events
        #endregion

        #region Behaviour
        #region Properties
        #endregion

        #region Constructors
        public UntypedDictionary()
        {
            _dictionary = new Dictionary<T, object>();
        }

        public UntypedDictionary(int capacity)
        {
            _dictionary = new Dictionary<T, object>(capacity);
        }

        public UntypedDictionary(IDictionary<T, object> dictionary)
        {
            _dictionary = new Dictionary<T, object>(dictionary);
        }
        #endregion

        #region Methods
        public IEnumerator<KeyValuePair<T, object>> GetEnumerator()
        {
            return _dictionary.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _dictionary.GetEnumerator();
        }

        public void Add(T key, object value)
        {
            _dictionary.Add(key, value);
        }

        public void Remove(T key)
        {
            _dictionary.Remove(key);
        }

        public T1 GetValue<T1>(T key)
        {
            return (T1)_dictionary[key];
        }
        #endregion

        #region Indexers
        #endregion

        #region Events handlers
        #endregion
        #endregion
    }
}
