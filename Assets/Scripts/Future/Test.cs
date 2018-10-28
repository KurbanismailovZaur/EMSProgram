using EMSP.Communication;
using EMSP.Data.XLS;
using EMSP.Future;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Namespace
{
	public class Test : MonoBehaviour 
	{
		private void Start()
		{
            string path = @"C:\Users\Zaur Magomedovich\Desktop\TestLeads2.xls";
            WiringDataReader reader = new WiringDataReader();

            Wiring wiring;
            reader.ReadWiringFromFile(path, out wiring);

            Math2 math2 = new Math2();
            var results =  math2.Calculate(wiring);

            for (int i = 0; i < results.Length; i++)
            {
                Debug.Log("M[" + i + "] = " + results[i]);
            }
        }
	}
}