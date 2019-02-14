using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CaresoftHMISDataAccess;
namespace Caresoft2._0.CustomData
{
    public class WardsMasterData
    {
        public List<HSBuilding> Buildings  { get; set; }
        public List<HSWard> Wards { get; set; }
        public List<HSFloor> Floors { get; set; }
        public List<HSWardCategory> Categories { get; set; }

    }
}