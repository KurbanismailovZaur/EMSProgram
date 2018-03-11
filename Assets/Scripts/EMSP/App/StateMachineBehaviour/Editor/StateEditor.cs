using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using ScriptEditor = UnityEditor.Editor;

namespace EMSP.App.StateMachineBehaviour.Editor
{
    [CustomEditor(typeof(State), true)]
    public class StateEditor : ScriptEditor
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
        #endregion

        #region Events
        #endregion

        #region Behaviour
        #region Properties
        #endregion

        #region Constructors
        #endregion

        #region Methods
        public override void OnInspectorGUI()
        {
            //SerializedProperty scriptProperty = serializedObject.FindProperty("m_Script");

            //GUI.enabled = false;
            //EditorGUILayout.PropertyField(scriptProperty);
            //GUI.enabled = true;
            base.OnInspectorGUI();

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Debug Info", EditorStyles.boldLabel);

            State state = (State)serializedObject.targetObject;
            FieldInfo stateMachineFieldInfo = typeof(State).GetField("_parentStateMachine", BindingFlags.NonPublic | BindingFlags.Instance);
            StateMachine stateMachine = (StateMachine)stateMachineFieldInfo.GetValue(state);
            GUI.enabled = false;
            EditorGUILayout.ObjectField("Parent State Machine", stateMachine, typeof(StateMachine), true);
            GUI.enabled = true;

            FieldInfo isSubStateMachineFieldInfo = typeof(State).GetField("_isSubStateMachine", BindingFlags.NonPublic | BindingFlags.Instance);
            bool isSubStateMachine = (bool)isSubStateMachineFieldInfo.GetValue(state);
            GUI.enabled = false;
            EditorGUILayout.Toggle("Is Sub-State Machine", isSubStateMachine);
            GUI.enabled = true;

            serializedObject.ApplyModifiedProperties();
        }
        #endregion

        #region Indexers
        #endregion

        #region Events handlers
        #endregion
        #endregion
    }
}