﻿using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using ScriptEditor = UnityEditor.Editor;

namespace EMSP.App.Editor
{
    [CustomEditor(typeof(Game))]
    public class GameEditor : ScriptEditor
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
            SerializedProperty scriptProperty = serializedObject.FindProperty("m_Script");

            GUI.enabled = false;
            EditorGUILayout.PropertyField(scriptProperty);
            GUI.enabled = true;

            SerializedProperty statesPoolProperty = serializedObject.FindProperty("_statesPool");
            EditorGUILayout.PropertyField(statesPoolProperty);

            Game game = (Game)serializedObject.targetObject;
            FieldInfo gameStateFieldInfo = typeof(Game).GetField("_gameState", BindingFlags.NonPublic | BindingFlags.Instance);
            GameState gameState = (GameState)gameStateFieldInfo.GetValue(game);

            GUI.enabled = false;
            EditorGUILayout.ObjectField("Current State", gameState, typeof(GameState), true);
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