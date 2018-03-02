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
        public List<Wire> ReadWiringFromFile(string pathToXLS)
        {
            HSSFWorkbook MyBook;

            using (FileStream stream = new FileStream(pathToXLS, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                MyBook = new HSSFWorkbook(stream);
            }

            List<Wire> wires = new List<Wire>();

            for (int i = 0; i < MyBook.NumberOfSheets; i++)
            {
                ISheet sheet = MyBook.GetSheetAt(i);

                Wire wire = new Wire();

                for (int j = 5; j <= sheet.LastRowNum; j++)
                {
                    IRow row = sheet.GetRow(j);

                    Segment segment = ReadSegment(row);
                    wire.Segments.Add(segment);
                }

                wires.Add(wire);
            }

            return wires;
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
		#endregion
		
		#region Indexers
		#endregion
		
		#region Events handlers
		#endregion
		#endregion
	}
}