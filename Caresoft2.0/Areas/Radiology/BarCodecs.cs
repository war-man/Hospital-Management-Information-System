using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Caresoft2._0.Areas.Radiology
{
    public class BarCodecs
    {
        public string generateBarcode(int id)
        {
            return id.ToString().PadLeft(6, '0');
        }
    }
}