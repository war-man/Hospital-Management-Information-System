using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CaresoftHMISDataAccess;

namespace Caresoft2._0.CustomData
{
    public class FloorMasterData
    {
        public List<HSFloor> Floors { get; set; }
        public List<HSBuilding> Buildings { get; set; }

    }
}