using Numba;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EMSP.Mathematic
{
	public class Data : IEnumerable<KeyValuePair<string, object>>
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
        private UntypedDictionary<string> _dictionary;
        #endregion

        #region Events
        #endregion

        #region Behaviour
        #region Properties
        #endregion

        #region Constructors
        public Data()
        {
            _dictionary = new UntypedDictionary<string>();
        }

        public Data(int capacity)
        {
            _dictionary = new UntypedDictionary<string>(capacity);
        }

        public Data(IDictionary<string, object> dictionary)
        {
            _dictionary = new UntypedDictionary<string>(dictionary);
        }
        #endregion

        #region Methods
        public IEnumerator<KeyValuePair<string, object>> GetEnumerator()
        {
            return _dictionary.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _dictionary.GetEnumerator();
        }

        public void Add(string key, object value)
        {
            _dictionary.Add(key, value);
        }

        public void Remove(string key)
        {
            _dictionary.Remove(key);
        }

        public T GetValue<T>(string key)
        {
            return _dictionary.GetValue<T>(key);
        }
        #endregion

        #region Indexers
        #endregion

        #region Events handlers
        #endregion
        #endregion
    }
}
