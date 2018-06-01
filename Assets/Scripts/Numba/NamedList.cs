using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Numba
{
    [Serializable]
    public class NamedList
    {
        [SerializeField]
        protected List<string> _names = new List<string>();

        [SerializeField]
        protected List<object> _objects = new List<object>();

        public int Count { get { return _objects.Count; } }

        public void Clear()
        {
            _names.Clear();
            _objects.Clear();
        }

        public void RemoveAt(int index)
        {
            _names.RemoveAt(index);
            _objects.RemoveAt(index);
        }

        public void Remove(string name)
        {
            RemoveAt(_names.IndexOf(name));
        }
    }

    [Serializable]
	public class NamedList<T> : NamedList
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
        protected new List<T> _objects = new List<T>();
        #endregion

        #region Events
        #endregion

        #region Behaviour
        #region Properties
        #endregion

        #region Constructors
        #endregion

        #region Methods
        public void Add(string name, T obj)
        {
            if (_names.Contains(name)) throw new ArgumentException(string.Format("Object with name \"{0}\" already exist", name));

            _names.Add(name);
            _objects.Add(obj);
        }
        #endregion

        #region Indexers
        public T this[int index]
        {
            get { return _objects[index]; }
            set { _objects[index] = value; }
        }

        public T this[string name]
        {
            get { return _objects[_names.IndexOf(name)]; }
            set { _objects[_names.IndexOf(name)] = value; }
        }
		#endregion
		
		#region Events handlers
		#endregion
		#endregion
	}
}
