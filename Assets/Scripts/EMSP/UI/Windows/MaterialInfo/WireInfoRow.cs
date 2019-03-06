using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace EMSP.UI.Windows.MaterialInfo
{
    public class WireInfoRow : MonoBehaviour
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
        public InputField col5;
        public InputField col6;

        private MaterialsInfoWindow _window;

        #endregion

        #region Events
        #endregion

        #region Behaviour

        #region Properties
        #endregion

        #region Constructors
        #endregion

        #region Methods

        public void Initialize(MaterialsInfoWindow window, string wireName, int column1, float column2, int column3, float column4, float column5, float column6)
        {
            _window = window;

            col0.contentType = InputField.ContentType.Standard;
            col1.contentType = InputField.ContentType.IntegerNumber;
            col2.contentType = InputField.ContentType.DecimalNumber;
            col3.contentType = InputField.ContentType.IntegerNumber;
            col4.contentType = InputField.ContentType.DecimalNumber;
            col5.contentType = InputField.ContentType.DecimalNumber;
            col6.contentType = InputField.ContentType.DecimalNumber;


            col0.text = wireName;
            col1.text = column1.ToString();
            col2.text = column2.ToString();
            col3.text = column3.ToString();
            col4.text = column4.ToString();
            col5.text = column5.ToString();
            col6.text = column6.ToString();


            col1.onEndEdit.AddListener((str) =>
            {
                if (string.IsNullOrEmpty(str) || str == "-")
                {
                    col1.text = 0.ToString();
                }
                else if(!_window.HasMat(int.Parse(str)))
                {
                    col1.text = 0.ToString();
                }
            });

            col2.onEndEdit.AddListener((str) =>
            {
                if (string.IsNullOrEmpty(str) || str == "-")
                {
                    col2.text = 0.ToString();
                }
                else if (float.Parse(str) < 0)
                {
                    col2.text = 0.ToString();
                }
            });

            col3.onEndEdit.AddListener((str) =>
            {
                if (string.IsNullOrEmpty(str) || str == "-")
                {
                    col3.text = 0.ToString();
                }
                else if (!_window.HasMat(int.Parse(str)))
                {
                    col3.text = 0.ToString();
                }
            });

            col4.onEndEdit.AddListener((str) =>
            {
                if (string.IsNullOrEmpty(str) || str == "-")
                {
                    col4.text = 0.ToString();
                }
                else if (float.Parse(str) < 0)
                {
                    col4.text = 0.ToString();
                }
            });

            col5.onEndEdit.AddListener((str) =>
            {
                if (string.IsNullOrEmpty(str) || str == "-")
                {
                    col5.text = 0.ToString();
                }
                else if (float.Parse(str) < 0)
                {
                    col5.text = 0.ToString();
                }
            });

            col6.onEndEdit.AddListener((str) =>
            {
                if (string.IsNullOrEmpty(str) || str == "-")
                {
                    col6.text = 0.ToString();
                }
                else if (float.Parse(str) < 0)
                {
                    col6.text = 0.ToString();
                }
            });
        }

        public string GetColumn0()
        {
            return col0.text;
        }

        public string GetColumn1()
        {
            return col1.text;
        }

        public float GetColumn2()
        {
            return float.Parse(col2.text);
        }

        public string GetColumn3()
        {
            return col3.text;
        }

        public float GetColumn4()
        {
            return float.Parse(col4.text);
        }

        public float GetColumn5()
        {
            return float.Parse(col5.text);
        }

        public float GetColumn6()
        {
            return float.Parse(col6.text);
        }
        #endregion

        #region Indexers
        #endregion

        #region Events handlers
        #endregion
        #endregion
    }
}