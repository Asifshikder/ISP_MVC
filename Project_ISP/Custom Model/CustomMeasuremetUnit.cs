using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project_ISP.ViewModel
{
    public class CustomMeasuremetUnit
    {
        public int MeasurementUnitID { get; set; }
        public string UnitName { get; set; }
        public int TableStatus { get; set; }
        
        public bool UpdateMeasurementUnit { get; set; }
    }
}