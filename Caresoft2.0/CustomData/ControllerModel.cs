using CaresoftHMISDataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Caresoft2._0.CustomData
{
    public class ControllerModel
    {
        //create a two lists both of type controller 
        public List<TblController> ActionsToBeAdded { get; set; }
        public List<TblController> ActionsToBeRemoved { get; set; }
    }
}