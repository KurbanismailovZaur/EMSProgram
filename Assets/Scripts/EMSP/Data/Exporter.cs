using EMSP.Communication;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using System.IO;
using EMSP.Mathematic.MagneticTension;
using EMSP.Mathematic;
using System.Collections.ObjectModel;

namespace EMSP.Data
{
    public class Exporter
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
        public void ExportMagneticTensionInSpace(string path)
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

                headerCell0.CellStyle.Alignment = HorizontalAlignment.Center;
                headerCell1.CellStyle.Alignment = HorizontalAlignment.Center;
                headerCell2.CellStyle.Alignment = HorizontalAlignment.Center;
                headerCell3.CellStyle.Alignment = HorizontalAlignment.Center;

                headerCell0.SetCellValue("X");
                headerCell1.SetCellValue("Y");
                headerCell2.SetCellValue("Z");
                headerCell3.SetCellValue("Magnetic Tension");

                ReadOnlyCollection<MagneticTensionPoint> mtPoints = MathematicManager.Instance.MagneticTensionInSpace.MTPoints;

                for (int i = 0; i < mtPoints.Count; i++)
                {
                    IRow row = sheet.CreateRow(i + 1);

                    ICell cell0 = row.CreateCell(0, CellType.Numeric);
                    ICell cell1 = row.CreateCell(1, CellType.Numeric);
                    ICell cell2 = row.CreateCell(2, CellType.Numeric);
                    ICell cell3 = row.CreateCell(3, CellType.Numeric);

                    cell0.CellStyle.Alignment = HorizontalAlignment.Center;
                    cell1.CellStyle.Alignment = HorizontalAlignment.Center;
                    cell2.CellStyle.Alignment = HorizontalAlignment.Center;
                    cell3.CellStyle.Alignment = HorizontalAlignment.Center;

                    cell0.SetCellValue(mtPoints[i].transform.position.x);
                    cell1.SetCellValue(mtPoints[i].transform.position.y);
                    cell2.SetCellValue(mtPoints[i].transform.position.z);
                    cell3.SetCellValue(mtPoints[i].MagneticTensionsInTime[0].MagneticTension);
                }

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