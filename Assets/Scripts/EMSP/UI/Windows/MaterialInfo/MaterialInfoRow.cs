using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace EMSP.UI.Windows.MaterialInfo
{
    public class MaterialInfoRow : MonoBehaviour
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
        public Button DeleteButton;

        public InputField col0;
        public InputField col1;
        public InputField col2;
        public InputField col3;
        public InputField col4;


        #endregion

        #region Events
        #endregion

        #region Behaviour

        #region Properties
        #endregion

        #region Constructors
        #endregion

        #region Methods

        public void Initialize(int column0, string column1, float column2, float column3, float column4)
        {
            col0.contentType = InputField.ContentType.IntegerNumber;
            col1.contentType = InputField.ContentType.Standard;
            col2.contentType = InputField.ContentType.Standard;
            col3.contentType = InputField.ContentType.Standard;
            col4.contentType = InputField.ContentType.Standard;

            col0.text = column0.ToString();
            col1.text = column1;
            col2.text = column2.ToString();
            col3.text = column3.ToString();
            col4.text = column4.ToString();


            col0.onEndEdit.AddListener((str) =>
            {
                if (string.IsNullOrEmpty(str) || str == "-")
                {
                    col1.text = 0.ToString();
                }
            });
        }


        public int GetColumn0()
        {
            return int.Parse(col0.text);
        }

        public string GetColumn1()
        {
            return col1.text;
        }

        public float GetColumn2()
        {
            return float.Parse(col2.text);
        }

        public float GetColumn3()
        {
            return float.Parse(col3.text);
        }


        public float GetColumn4()
        {
            return float.Parse(col4.text);
        }
        #endregion

        #region Indexers
        #endregion

        #region Events handlers
        #endregion
        #endregion
    }
}