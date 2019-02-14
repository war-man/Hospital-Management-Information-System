using LabsDataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Caresoft2._0.Areas.Radiology.Models
{
    public class RadiologyAddNewUserData
    {
        public DepartmentAssignment DepartmentAssignment { get; set; }
        public List<CaresoftHMISDataAccess.Employee> Employees { get; set; }
        public DepartmentAssignment UserDepartmentAssignment { get; set; }
        public List<UserType> UserType { get; set; }
        public List<Department> MainDepartments { get; set; }
        public List<CaresoftHMISDataAccess.Department> Department { get; set; }
    }
}