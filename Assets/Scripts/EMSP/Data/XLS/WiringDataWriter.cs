using EMSP.Communication;
using EMSP.Mathematic;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using UnityEngine;

namespace EMSP.Data.XLS
{
    public class WiringDataWriter
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
        public void ExportMagneticTensionInSpace(string path, IEnumerable<CalculationPoint> mtPoints)
        {
            using (FileStream stream = new FileStream(path, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite))
            {
                HSSFWorkbook workbook = new HSSFWorkbook();

                ISheet sheet = workbook.CreateSheet("Magnetic Tensions");

                IRow headerRow = sheet.CreateRow(0);

                ICell headerCell0 = headerRow.CreateCell(0, CellType.String);
                ICell headerCell1 = headerRow.CreateCell(1, CellType.String);
                ICell headerCell2 = headerRow.CreateCell(2, CellType.String);
                ICell headerCell3 = headerRow.CreateCell(3, CellType.String);
                ICell headerCell4 = headerRow.CreateCell(4, CellType.String);

                headerCell0.CellStyle.Alignment = HorizontalAlignment.Center;
                headerCell1.CellStyle.Alignment = HorizontalAlignment.Center;
                headerCell2.CellStyle.Alignment = HorizontalAlignment.Center;
                headerCell3.CellStyle.Alignment = HorizontalAlignment.Center;
                headerCell4.CellStyle.Alignment = HorizontalAlignment.Center;

                headerCell0.SetCellValue("X");
                headerCell1.SetCellValue("Y");
                headerCell2.SetCellValue("Z");
                headerCell3.SetCellValue("Magnetic Tension (calculated amperage)");
                headerCell4.SetCellValue("Magnetic Tension (precomputed amperage)");

                int i = 0;
                foreach (var point in mtPoints)
                {
                    IRow row = sheet.CreateRow(++i);

                    ICell cell0 = row.CreateCell(0, CellType.Numeric);
                    ICell cell1 = row.CreateCell(1, CellType.Numeric);
                    ICell cell2 = row.CreateCell(2, CellType.Numeric);
                    ICell cell3 = row.CreateCell(3, CellType.Numeric);
                    ICell cell4 = row.CreateCell(4, CellType.Numeric);

                    cell0.CellStyle.Alignment = HorizontalAlignment.Center;
                    cell1.CellStyle.Alignment = HorizontalAlignment.Center;
                    cell2.CellStyle.Alignment = HorizontalAlignment.Center;
                    cell3.CellStyle.Alignment = HorizontalAlignment.Center;
                    cell4.CellStyle.Alignment = HorizontalAlignment.Center;

                    cell0.SetCellValue(point.transform.position.x);
                    cell1.SetCellValue(point.transform.position.y);
                    cell2.SetCellValue(point.transform.position.z);
                    cell3.SetCellValue(point.CalculatedMagneticTensionsInTime[0].CalculatedValue);
                    cell4.SetCellValue(point.PrecomputedMagneticTension);

                    i++;
                }

                workbook.Write(stream);
            }
        }

        public void ExportWiring(string path, Wiring wiring)
        {
            HSSFWorkbook workbook = new HSSFWorkbook();

            foreach (Wire wire in wiring)
            {
                var sheet = workbook.CreateSheet(wire.Name);

                int rowNumber = 0;

                {// Amplitude
                    var row = sheet.CreateRow(rowNumber++);
                    var cell_0 = row.CreateCell(0, CellType.String);
                    cell_0.SetCellValue("Амплитуда");
                    var cell_1 = row.CreateCell(1, CellType.Numeric);
                    cell_1.SetCellValue(wire.Amplitude);
                    var cell_2 = row.CreateCell(2);
                }

                {// Frequency
                    var row = sheet.CreateRow(rowNumber++);
                    var cell_0 = row.CreateCell(0, CellType.String);
                    cell_0.SetCellValue("Частота");
                    var cell_1 = row.CreateCell(1, CellType.Numeric);
                    cell_1.SetCellValue(wire.Frequency);
                    var cell_2 = row.CreateCell(2);
                }

                {// Amperage
                    var row = sheet.CreateRow(rowNumber++);
                    var cell_0 = row.CreateCell(0, CellType.String);
                    cell_0.SetCellValue("Сила тока");
                    var cell_1 = row.CreateCell(1, CellType.Numeric);
                    cell_1.SetCellValue(wire.Amperage);
                    var cell_2 = row.CreateCell(2);
                }

                {// 
                    var row = sheet.CreateRow(rowNumber++);
                    var cell_0 = row.CreateCell(0);
                    var cell_1 = row.CreateCell(1, CellType.String);
                    cell_1.SetCellValue("Узлы");
                    var cell_2 = row.CreateCell(2);
                }

                {// 
                    var row = sheet.CreateRow(rowNumber++);
                    var cell_0 = row.CreateCell(0, CellType.String);
                    cell_0.SetCellValue("X");
                    var cell_1 = row.CreateCell(1, CellType.String);
                    cell_1.SetCellValue("Y");
                    var cell_2 = row.CreateCell(2, CellType.String);
                    cell_2.SetCellValue("Z");
                }

                foreach (Vector3 point in wire)
                {
                    var row = sheet.CreateRow(rowNumber++);
                    var cell_0 = row.CreateCell(0, CellType.Numeric);
                    cell_0.SetCellValue(point.x);
                    var cell_1 = row.CreateCell(1, CellType.Numeric);
                    cell_1.SetCellValue(point.y);
                    var cell_2 = row.CreateCell(2, CellType.Numeric);
                    cell_2.SetCellValue(point.z);
                }

            }

            using (FileStream stream = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.ReadWrite))
            {
                workbook.Write(stream);
            }
        }

        public void ExportModelVertices(string path, Model model)
        {
            MeshRenderer[] renderers = model.GetComponentsInChildren<MeshRenderer>();

            if (renderers.Length == 0) return;

            using (FileStream stream = new FileStream(path, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite))
            {
                HSSFWorkbook workbook = new HSSFWorkbook();

                ISheet sheet = CreateSheetForVertices(workbook, "0");

                List<Material> materials = new List<Material>();

                int rowIndex = 1;
                int listIndex = 1;

                foreach (var renderer in renderers)
                {
                    Material[] rendererMaterials = renderer.sharedMaterials;
                    Mesh mesh = renderer.GetComponent<MeshFilter>().sharedMesh;
                    Vector3[] vertices = mesh.vertices;

                    for (int i = 0; i < rendererMaterials.Length; i++)
                    {
                        int[] triangles = mesh.GetTriangles(i);

                        for (int j = 0; j < triangles.Length; j++)
                        {
                            Vector3 vertex = renderer.transform.localToWorldMatrix.MultiplyPoint3x4(vertices[triangles[j]]);

                            IRow row = sheet.CreateRow(rowIndex);

                            row.CreateCell(0, CellType.Numeric).SetCellValue(RoundWithPrecision(vertex.x));
                            row.CreateCell(1, CellType.Numeric).SetCellValue(RoundWithPrecision(vertex.y));
                            row.CreateCell(2, CellType.Numeric).SetCellValue(RoundWithPrecision(vertex.z));

                            if (!materials.Contains(rendererMaterials[i])) materials.Add(rendererMaterials[i]);

                            row.CreateCell(3, CellType.Numeric).SetCellValue(materials.IndexOf(rendererMaterials[i]));

                            if (++rowIndex == 60000)
                            {
                                sheet = CreateSheetForVertices(workbook, listIndex++.ToString());
                                rowIndex = 1;
                            };
                        }
                    }
                }

                workbook.Write(stream);
            }

        }

        private ISheet CreateSheetForVertices(HSSFWorkbook workbook, string name)
        {
            ISheet sheet = workbook.CreateSheet(name);

            IRow headerRow = sheet.CreateRow(0);

            headerRow.CreateCell(0, CellType.String).SetCellValue("X");
            headerRow.CreateCell(1, CellType.String).SetCellValue("Y");
            headerRow.CreateCell(2, CellType.String).SetCellValue("Z");
            headerRow.CreateCell(3, CellType.String).SetCellValue("Material ID");

            return sheet;
        }

        private double RoundWithPrecision(float value)
        {
            double result = Math.Round(value, 7);
            int sign = Math.Sign(value);
            double absoluteResult = Math.Abs(result);
            double roundedAbsoluteResult = Math.Round(absoluteResult);

            if (Math.Abs(Math.Round(roundedAbsoluteResult - absoluteResult, 7)) <= 0.000001d)
                return roundedAbsoluteResult * sign;
            else
                return result;
         }
        #endregion

        #region Indexers
        #endregion

        #region Events handlers
        #endregion
        #endregion
    }
}