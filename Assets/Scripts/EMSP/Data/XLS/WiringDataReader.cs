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

                for (int j = 5; j <= sheet.LastRowNum; j++)
                {
                    IRow row = sheet.GetRow(j);

                    if (!IsCorrectRow(row))
                    {
                        break;
                    }

                    wire.Add(ReadNode(row));
                }
            }

            return wiring;
        }

        private Vector3 ReadNode(IRow row)
        {
            Vector3 node;

            node.x = (float)row.GetCell(0).NumericCellValue;
            node.y = (float)row.GetCell(1).NumericCellValue;
            node.z = (float)row.GetCell(2).NumericCellValue;

            return node;
        }

        private bool IsCorrectRow(IRow row)
        {
            for (int i = 0; i < 3; i++)
            {
                ICell cell = row.GetCell(i);

                if (cell.CellType != CellType.Numeric)
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