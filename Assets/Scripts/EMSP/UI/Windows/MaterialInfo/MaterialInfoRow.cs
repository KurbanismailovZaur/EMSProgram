using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    public Text NumberField;
    public InputField col1;
    public InputField col2;
    public InputField col3;
    public InputField col4;


    private int _currentPointNumber;



    #endregion

    #region Events
    #endregion

    #region Behaviour

    #region Properties
    #endregion

    #region Constructors
    #endregion

    #region Methods

    public void Initialize(int num, string column1, float column2, float column3, float column4)
    {
        col1.contentType = InputField.ContentType.Standard;
        col2.contentType = InputField.ContentType.Standard;
        col3.contentType = InputField.ContentType.Standard;
        col4.contentType = InputField.ContentType.Standard;


        StartCoroutine(UpdateRowNumber());

        col1.text = column1;
        col2.text = column2.ToString();
        col3.text = column3.ToString();
        col4.text = column4.ToString();

        //col2.onValueChanged.AddListener((str) =>
        //{
        //    float newX;
        //    if (float.TryParse(str, out newX))
        //    {
        //    }
        //});

        //col2.onEndEdit.AddListener((str) =>
        //{
        //    if (string.IsNullOrEmpty(str) || str == "-")
        //    {
        //        col1.text = 0.ToString();
        //    }
        //});

        //col3.onValueChanged.AddListener((str) =>
        //{
        //    float newY;
        //    if (float.TryParse(str, out newY))
        //    {
        //    }
        //});

        //col3.onEndEdit.AddListener((str) =>
        //{
        //    if (string.IsNullOrEmpty(str) || str == "-")
        //    {
        //        col2.text = 0.ToString();
        //    }
        //});

        //col4.onValueChanged.AddListener((str) =>
        //{
        //    float newZ;
        //    if (float.TryParse(str, out newZ))
        //    {
        //    }
        //});


        //col4.onEndEdit.AddListener((str) =>
        //{
        //    if (string.IsNullOrEmpty(str) || str == "-")
        //    {
        //        col4.text = 0.ToString();
        //    }
        //});

    }

    public void UpdateRowNumberImmediate()
    {
        int sibIndex = GetComponent<RectTransform>().GetSiblingIndex();

        NumberField.text = sibIndex.ToString();
        _currentPointNumber = sibIndex;
    }

    public IEnumerator UpdateRowNumber()
    {
        yield return null;

        UpdateRowNumberImmediate();
    }


    public int GetColumn0()
    {
        return int.Parse(NumberField.text);
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
