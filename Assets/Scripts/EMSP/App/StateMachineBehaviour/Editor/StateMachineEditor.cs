using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using ScriptEditor = UnityEditor.Editor;

namespace EMSP.App.StateMachineBehaviour.Editor
{
    [CustomEditor(typeof(StateMachine))]
    public class StateMachineEditor : ScriptEditor
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
        private bool _foldoutFlag = true;
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
            #region Script reference
            SerializedProperty scriptProperty = serializedObject.FindProperty("m_Script");

            GUI.enabled = false;
            EditorGUILayout.PropertyField(scriptProperty);
            GUI.enabled = true;
            #endregion

            Color oldColor = GUI.color;

            EditorGUILayout.BeginVertical("Box");
            EditorGUI.indentLevel = 1;
            _foldoutFlag = EditorGUILayout.Foldout(_foldoutFlag, "States", true);

            if (_foldoutFlag)
            {
                #region Get states names and objects
                FieldInfo statesNamesFieldInfo = typeof(StateMachine).GetField("_statesNames", BindingFlags.Instance | BindingFlags.NonPublic);
                List<string> statesNames = (List<string>)statesNamesFieldInfo.GetValue(serializedObject.targetObject);

                FieldInfo statesFieldInfo = typeof(StateMachine).GetField("_states", BindingFlags.Instance | BindingFlags.NonPublic);
                List<State> states = (List<State>)statesFieldInfo.GetValue(serializedObject.targetObject);
                #endregion

                if (statesNames.Count == 0)
                {
                    EditorGUILayout.LabelField("No one states was created.");
                }
                else
                {
                    EditorGUI.indentLevel = 2;

                    #region States header
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField("Name");
                    EditorGUILayout.LabelField("State");
                    EditorGUILayout.EndHorizontal();
                    #endregion

                    #region States
                    for (int i = 0; i < statesNames.Count; i++)
                    {
                        EditorGUILayout.BeginHorizontal();
                        statesNames[i] = EditorGUILayout.TextField(statesNames[i]);
                        states[i] = (State)EditorGUILayout.ObjectField(states[i], typeof(State), true);

                        #region State edit buttons
                        GUI.color = Color.yellow;

                        // Move down.
                        if (GUILayout.Button("↓"))
                        {
                            if (i + 1 <= statesNames.Count - 1)
                            {
                                string tempString = statesNames[i + 1];
                                State tempState = states[i + 1];

                                statesNames[i + 1] = statesNames[i];
                                states[i + 1] = states[i];

                                statesNames[i] = tempString;
                                states[i] = tempState;
                            }
                        }

                        // Move up.
                        if (GUILayout.Button("↑"))
                        {
                            if (i - 1 >= 0)
                            {
                                string tempString = statesNames[i - 1];
                                State tempState = states[i - 1];

                                statesNames[i - 1] = statesNames[i];
                                states[i - 1] = states[i];

                                statesNames[i] = tempString;
                                states[i] = tempState;
                            }
                        }

                        // remove.
                        GUI.color = Color.red;
                        if (GUILayout.Button("-"))
                        {
                            statesNames.RemoveAt(i);
                            states.RemoveAt(i);
                        }

                        GUI.color = oldColor;
                        #endregion
                        EditorGUILayout.EndHorizontal();
                    }
                    #endregion
                    
                    EditorGUILayout.LabelField(string.Empty, GUI.skin.horizontalSlider);
                }

                #region Calculate rect for "+" button
                EditorGUILayout.BeginHorizontal();
                Rect plusButtonRect = EditorGUILayout.GetControlRect();
                plusButtonRect.x = plusButtonRect.x + plusButtonRect.width - 64f;
                plusButtonRect.width = 64f;
                #endregion

                #region "+" button
                GUI.color = Color.green;
                if (GUI.Button(new Rect(plusButtonRect.position, new Vector2(plusButtonRect.width, plusButtonRect.height)), "+"))
                {
                    statesNames.Add(string.Empty);
                    states.Add(null);
                }
                #endregion

                GUI.color = oldColor;

                EditorGUILayout.EndHorizontal();
                EditorGUI.indentLevel = 1;
            }

            EditorGUI.indentLevel = 0;
            EditorGUILayout.EndVertical();

            #region Auto start
            EditorGUILayout.BeginVertical("Box");
            SerializedProperty autoStartSettings = serializedObject.FindProperty("_onStartSettings");

            SerializedProperty autoStartFlag = autoStartSettings.FindPropertyRelative("_enterToStateOnStart");
            EditorGUILayout.PropertyField(autoStartFlag);

            if (autoStartFlag.boolValue)
            {
                SerializedProperty stateName = autoStartSettings.FindPropertyRelative("_startStateName");
                EditorGUILayout.PropertyField(stateName);
            }

            EditorGUILayout.EndVertical();
            #endregion

            #region Current state
            StateMachine game = (StateMachine)serializedObject.targetObject;
            FieldInfo gameStateFieldInfo = typeof(StateMachine).GetField("_currentState", BindingFlags.NonPublic | BindingFlags.Instance);
            State gameState = (State)gameStateFieldInfo.GetValue(game);

            GUI.enabled = false;
            GUI.color = Color.green;
            EditorGUILayout.ObjectField("Current State", gameState, typeof(State), true);
            GUI.color = oldColor;
            GUI.enabled = true;
            #endregion

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