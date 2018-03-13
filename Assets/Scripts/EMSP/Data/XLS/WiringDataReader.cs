using EMSP.Communication;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace EMSP.Data.XLS
{
	public class WiringDataReader 
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
        public Wiring ReadWiringFromFile(string pathToXLS)
        {
            Wiring.Factory wiringFactory = new Wiring.Factory();
            Wiring wiring = wiringFactory.Create();

            HSSFWorkbook MyBook;

            using (FileStream stream = new FileStream(pathToXLS, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                MyBook = new HSSFWorkbook(stream);
            }

            Wire.Factory wireFactory = new Wire.Factory();

            for (int i = 0; i < MyBook.NumberOfSheets; i++)
            {
                ISheet sheet = MyBook.GetSheetAt(i);

                Wire wire = wiring.CreateWire();

                List<Segment> segments = new List<Segment>();
                for (int j = 5; j <= sheet.LastRowNum; j++)
                {
                    IRow row = sheet.GetRow(j);

                    if (!IsCorrectRow(row))
                    {
                        break;
                    }

                    Segment segment = ReadSegment(row);
                    segments.Add(segment);
                }

                for (int j = 0; j < segments.Count; j++)
                {
                    wire.Add(segments[j].pointA);
                }

                wire.Add(segments[segments.Count - 1].pointB);
            }

            return wiring;
        }

        private Segment ReadSegment(IRow row)
        {
            Segment segment = new Segment();

            segment.pointA.x = (float)row.GetCell(0).NumericCellValue;
            segment.pointA.y = (float)row.GetCell(1).NumericCellValue;
            segment.pointA.z = (float)row.GetCell(2).NumericCellValue;

            segment.pointB.x = (float)row.GetCell(3).NumericCellValue;
            segment.pointB.y = (float)row.GetCell(4).NumericCellValue;
            segment.pointB.z = (float)row.GetCell(5).NumericCellValue;

            return segment;
        }

        private bool IsCorrectRow(IRow row)
        {
            for (int i = 0; i <= 5; i++)
            {
                ICell cell = row.GetCell(i);

                if (cell.CellType != CellType.Numeric && cell.CellType != CellType.Formula)
                {
                    return false;
                }
            }

            return true;
        }
		#endregion
		
		#region Indexers
		#endregion
		
		#region Events handlers
		#endregion
		#endregion
	}
}