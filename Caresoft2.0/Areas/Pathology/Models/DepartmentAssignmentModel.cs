using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Caresoft2._0.Areas.Pathology.Models
{
    public class DepartmentAssignmentModel
    {

        public int Id { get; set; }
        public int User { get; set; }
        public string UserName { get; set; }
        public int UserType { get; set; }
        public string Departments { get; set; }
        public HttpPostedFileBase SingatureImage { get; set; }

    }
}