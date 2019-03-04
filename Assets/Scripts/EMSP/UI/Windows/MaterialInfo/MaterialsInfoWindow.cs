using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine.UI;
using UnityEngine;

namespace EMSP.UI.Windows.MaterialInfo
{
    public class MaterialsInfoWindow : ModalWindow
    {
        public GameObject Page1;
        public GameObject Page2;
        public Button Page1ActivateButton;

        public RectTransform Page1Content;
        public RectTransform Page2Content;

        public MaterialInfoRow Page1RowPrefab;
        public WireInfoRow Page2RowPrefab;

        public Button AddRowButtonPrefab;


        private HSSFWorkbook _page1Book;
        private string _pathToBook1;
        private HSSFWorkbook _page2Book;
        private string _pathToBook2;


        public void ShowInfo()
        {
            ImportPage1();
            ImportPage2();


            var _addPointButtonPage1 = Instantiate(AddRowButtonPrefab);
            _addPointButtonPage1.transform.SetParent(Page1Content, false);
            _addPointButtonPage1.onClick.AddListener(() =>
            {
                MaterialInfoRow matRow = Instantiate(Page1RowPrefab);
                matRow.transform.SetParent(Page1Content, false);

                matRow.Initialize(0, "нет", 0, 0, 0);
                matRow.DeleteButton.onClick.AddListener(() =>
                {

                    StartCoroutine(WaitAndUpdatePointsNumberPage1());
                    Destroy(matRow.gameObject);
                });
                _addPointButtonPage1.transform.SetAsLastSibling();

                StartCoroutine(WaitAndMoveContainerPage1(matRow.GetComponent<RectTransform>().rect.height));
            });


            var _addPointButtonPage2 = Instantiate(AddRowButtonPrefab);
            _addPointButtonPage2.transform.SetParent(Page2Content, false);
            _addPointButtonPage2.onClick.AddListener(() =>
            {
                WireInfoRow wireRow = Instantiate(Page2RowPrefab);
                wireRow.transform.SetParent(Page2Content, false);

                wireRow.Initialize("нет", "нет", 0, "нет", 0, 0, 0);
                wireRow.DeleteButton.onClick.AddListener(() =>
                {
                    Destroy(wireRow.gameObject);
                });
                _addPointButtonPage2.transform.SetAsLastSibling();

                StartCoroutine(WaitAndMoveContainerPage2(wireRow.GetComponent<RectTransform>().rect.height));
            });


            ShowModal();
            Page1ActivateButton.onClick.Invoke();
        }

        public void Save()
        {
            SavePage1();
            SavePage2();

            File.Delete(_pathToBook1);
            using (FileStream stream = new FileStream(_pathToBook1, FileMode.Create))
            {
                _page1Book.Write(stream);
            }
            _page1Book = null;

            File.Delete(_pathToBook2);
            using (FileStream stream = new FileStream(_pathToBook2, FileMode.Create))
            {
                _page2Book.Write(stream);
            }
            _page1Book = null;


            Page1.SetActive(true);
            Page2.SetActive(true);
            Hide();
        }



        private void AddPage1Row(int column1, string column2, float column3, float column4, float column5)
        {
            MaterialInfoRow editPanel = Instantiate(Page1RowPrefab);
            editPanel.transform.SetParent(Page1Content, false);
            editPanel.DeleteButton.interactable = false;

            editPanel.Initialize(column1, column2, column3, column4, column5);
        }

        private void AddPage2Row(string wireName, string column1, float column2, string column3, float column4, float column5, float column6)
        {
            WireInfoRow editPanel = Instantiate(Page2RowPrefab);
            editPanel.transform.SetParent(Page2Content, false);
            editPanel.DeleteButton.interactable = false;

            editPanel.Initialize(wireName, column1, column2, column3, column4, column5, column6);
        }



        private void ImportPage1()
        {
            for (int i = 0; i < Page1Content.childCount; ++i)
            {
                Destroy(Page1Content.GetChild(i).gameObject);
            }


            _pathToBook1 = Path.Combine(Application.streamingAssetsPath, "MatsInfo.xls");


            using (FileStream stream = new FileStream(_pathToBook1, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                _page1Book = new HSSFWorkbook(stream);
            }

            ISheet sheet = _page1Book.GetSheetAt(0);

            for (int i = 3; i <= sheet.LastRowNum; i++)
            {
                IRow row = sheet.GetRow(i);

                var c1 = (int)row.GetCell(0).NumericCellValue;
                var c2 = (string)row.GetCell(1).StringCellValue;
                var c3 = (float)row.GetCell(2).NumericCellValue;
                var c4 = (float)row.GetCell(3).NumericCellValue;
                var c5 = (float)row.GetCell(4).NumericCellValue;

                AddPage1Row(c1, c2, c3, c4, c5);
            }
        }

        private void ImportPage2()
        {
            for (int i = 0; i < Page2Content.childCount; ++i)
            {
                Destroy(Page2Content.GetChild(i).gameObject);
            }


            _pathToBook2 = Path.Combine(Application.streamingAssetsPath, "wire_mat_table.xls");

            using (FileStream stream = new FileStream(_pathToBook2, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                _page2Book = new HSSFWorkbook(stream);
            }

            ISheet sheet = _page2Book.GetSheetAt(0);

            for (int i = 1; i <= sheet.LastRowNum; i++)
            {
                IRow row = sheet.GetRow(i);

                var c1 = row.GetCell(0).StringCellValue;
                var c2 = row.GetCell(1).StringCellValue;
                var c3 = (float)row.GetCell(2).NumericCellValue;
                var c4 = row.GetCell(3).StringCellValue;
                var c5 = (float)row.GetCell(4).NumericCellValue;
                var c6 = (float)row.GetCell(5).NumericCellValue;
                var c7 = (float)row.GetCell(6).NumericCellValue;

                AddPage2Row(c1, c2, c3, c4, c5, c6, c7);
            }
        }



        private void SavePage1()
        {
            ISheet sheet = _page1Book.GetSheetAt(0);


            for (int rowNum = 0; rowNum < Page1Content.childCount - 1; ++rowNum)
            {
                IRow row = sheet.GetRow(rowNum + 3);
                if (row == null)
                {
                    row = sheet.CreateRow(rowNum + 3);

                    row.CreateCell(0);
                    row.CreateCell(1);
                    row.CreateCell(2);
                    row.CreateCell(3);
                    row.CreateCell(4);
                }

                MaterialInfoRow innerRow = Page1Content.GetChild(rowNum).GetComponent<MaterialInfoRow>();


                row.Cells[0].SetCellValue(innerRow.GetColumn0());
                row.Cells[1].SetCellValue(innerRow.GetColumn1());
                row.Cells[2].SetCellValue(innerRow.GetColumn2());
                row.Cells[3].SetCellValue(innerRow.GetColumn3());
                row.Cells[4].SetCellValue(innerRow.GetColumn4());
            }
        }

        private void SavePage2()
        {
            ISheet sheet = _page2Book.GetSheetAt(0);


            for (int rowNum = 0; rowNum < Page2Content.childCount - 1; ++rowNum)
            {
                IRow row = sheet.GetRow(rowNum + 1);
                if (row == null)
                {
                    row = sheet.CreateRow(rowNum + 1);

                    row.CreateCell(0);
                    row.CreateCell(1);
                    row.CreateCell(2);
                    row.CreateCell(3);
                    row.CreateCell(4);
                    row.CreateCell(5);
                    row.CreateCell(6);
                }
                WireInfoRow innerRow = Page2Content.GetChild(rowNum).GetComponent<WireInfoRow>();

                row.Cells[0].SetCellValue(innerRow.GetColumn0());
                row.Cells[1].SetCellValue(innerRow.GetColumn1());
                row.Cells[2].SetCellValue(innerRow.GetColumn2());
                row.Cells[3].SetCellValue(innerRow.GetColumn3());
                row.Cells[4].SetCellValue(innerRow.GetColumn4());
                row.Cells[5].SetCellValue(innerRow.GetColumn5());
                row.Cells[6].SetCellValue(innerRow.GetColumn6());
            }
        }



        private IEnumerator WaitAndUpdatePointsNumberPage1()
        {
            yield return null;

            foreach (var panel in Page1Content.GetComponentsInChildren<MaterialInfoRow>())
            {
                panel.UpdateRowNumberImmediate();
            }
        }

        private IEnumerator WaitAndMoveContainerPage1(float offset)
        {
            yield return null;

            var pcParentHeight = Page1Content.parent.GetComponent<RectTransform>().rect.height;
            var pcHeight = Page1Content.rect.height;

            if (pcHeight > pcParentHeight)
            {
                Page1Content.anchoredPosition3D = new Vector3(
                       0,
                       Page1Content.anchoredPosition3D.y + offset + 5,
                       0);
            }
        }

        private IEnumerator WaitAndMoveContainerPage2(float offset)
        {
            yield return null;

            var pcParentHeight = Page2Content.parent.GetComponent<RectTransform>().rect.height;
            var pcHeight = Page2Content.rect.height;

            if (pcHeight > pcParentHeight)
            {
                Page2Content.anchoredPosition3D = new Vector3(
                       0,
                       Page2Content.anchoredPosition3D.y + offset + 5,
                       0);
            }
        }
    }
}