using EMSP.UI.Menu;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace Numba.Editor
{
	public class NamedListPropertyDrawer<T> : PropertyDrawer where T : NamedList
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
        private bool _foldout;
        #endregion

        #region Events
        #endregion

        #region Behaviour
        #region Properties
        #endregion

        #region Constructors
        #endregion

        #region Methods
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            _foldout = EditorGUI.Foldout(position, _foldout, label);

            if (!_foldout) return;

            #region Get states names and objects
            T concreteList = property.GetValue<T>();

            Type type = concreteList.GetType();
            Type parameterType = type.BaseType.GetGenericArguments()[0];

            bool isUnityEngineObject = typeof(UnityEngine.Object).IsAssignableFrom(parameterType);


            FieldInfo namesFieldInfo = typeof(NamedList).GetField("_names", BindingFlags.Instance | BindingFlags.NonPublic);
            List<string> names = (List<string>)namesFieldInfo.GetValue(concreteList);

            //FieldInfo statesFieldInfo = typeof(StateMachine).GetField("_states", BindingFlags.Instance | BindingFlags.NonPublic);
            //List<State> states = (List<State>)statesFieldInfo.GetValue(serializedObject.targetObject);
            #endregion
        }
        #endregion

        #region Indexers
        #endregion

        #region Events handlers
        #endregion
        #endregion
    }
}
