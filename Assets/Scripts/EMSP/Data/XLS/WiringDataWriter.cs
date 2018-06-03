using EMSP.Communication;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using System.Collections;
using System.Collections.Generic;
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


        public void WriteWiring(string pathForWrite, Wiring wiring)
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

            using (FileStream stream = new FileStream(pathForWrite, FileMode.Create, FileAccess.Write, FileShare.ReadWrite))
            {
                workbook.Write(stream);
            }
        }


        #endregion

        #region Indexers
        #endregion

        #region Events handlers
        #endregion
        #endregion
    }
}