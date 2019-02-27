using CaresoftHMISDataAccess;
using CrystalDecisions.CrystalReports.Engine;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Caresoft2._0.CrystalReports.MOH710
{
    [Auth]
    public class MOH710Controller : Controller
    {
        private CaresoftHMISEntities db = new CaresoftHMISEntities();
        // GET: MOH710
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult GenerateReport(DateTime fromDate, DateTime toDate)
        {
           

            ReportDocument rd = new ReportDocument();
            rd.Load(Path.Combine(Server.MapPath("~/CrystalReports/MOH710/Main.rpt")));

            //var toDate = DateTime.Now;
            //var fromDate = new DateTime(2018, 12, 1);



            var AllImmunizations = db.ImmunizationAdmins
                                    .Where(p=>p.DateGiven>=fromDate && p.DateGiven<=toDate)
                                    .ToList();

            var allTypesOfImmunization = db.ImmunizationCategories.ToList();

            List<ImmunizationData> lstImmunizationData = new List<ImmunizationData>();

            foreach (var typeImmunization in allTypesOfImmunization)
            {
                var PatientImmunizeds = AllImmunizations.Where(p => p.ImmunizationMasterId == typeImmunization.Id).ToList();

                ImmunizationData immunizationData = new ImmunizationData()
                {
                    Name = typeImmunization.ImmunizationCategoryName

                };


                foreach (var pat in PatientImmunizeds)
                {
                    

                    
                    

                    var dateOfBirth = pat.OpdRegister.Patient.DOB;

                    if (dateOfBirth != null)
                    {
                        var age = pat.DateGiven.Year - dateOfBirth.Value.Year;

                        if (age > 1)
                        {
                            immunizationData.CountGreaterThanOneYear++;
                        }
                        else 
                        {
                            immunizationData.CountLessThanOneYear++;
                        }
                    }
                    
                    
                }

                lstImmunizationData.Add(immunizationData);

            }


            rd.Subreports["ChildImmunization.rpt"].SetDataSource(GetChildImmunizationData(lstImmunizationData));

            Response.Buffer = false;
            Response.ClearContent();
            Response.ClearHeaders();

            Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
            stream.Seek(0, SeekOrigin.Begin);
            string fileName = "Report Sample-" + DateTime.Today + ".pdf";
            return File(stream, "application/pdf", fileName);
        }

        private object GetChildImmunizationData(List<ImmunizationData> lstImmunizationData)
        {
            var dataSet = new Caresoft2._0.CrystalReports.MOH710.ChildImmunization.DataSetChildImmunization();

            foreach (var item in lstImmunizationData)
            {
                dataSet.ChildImmunization.AddChildImmunizationRow(item.Name, item.CountGreaterThanOneYear, item.CountLessThanOneYear);
            }

            return dataSet;
        }
    }

    public class ImmunizationData
    {
        public string Name { get; set; }
        public int CountGreaterThanOneYear { get; set; }
        public int CountLessThanOneYear { get; set; }
    }
}