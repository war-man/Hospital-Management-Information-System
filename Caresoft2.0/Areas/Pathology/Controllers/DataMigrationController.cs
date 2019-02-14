using CaresoftHMISDataAccess;
using LabsDataAccess;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Caresoft2._0.Areas.Pathology.Controllers
{
    public class DataMigrationController : Controller
    {
        private QuantaLabsDataAccess.embutown_labEntities db = new QuantaLabsDataAccess.embutown_labEntities();
        private CareSoftLabsEntities db_labs = new CareSoftLabsEntities();
        private CaresoftHMISEntities db_main = new CaresoftHMISEntities();
        private int companyid = 1;

        // GET: Pathology/DataMigration
        public ActionResult Index()
        {
            return View();
        }

        public int MigrateDepartments()
        {

            var departments = db.Headings_n.Where(e => e.CompanyId == companyid).ToList();

            foreach (var m in departments)
            {
                var main_department = db_main.Departments.FirstOrDefault(e => e.DepartmentName == m.MainDept);

                if (main_department != null)
                {

                    db_labs.Departments.Add(
                    new LabsDataAccess.Department()
                    {
                        Main_Department = main_department.Id,
                        Department1 = m.Heading,
                        Department_Code = m.HCode,
                        Modality = "",
                        Profile_Department = (bool)m.ProfileFlag,
                        Remark = "",
                        DepartmentRadPath = main_department.Id

                    });
                }

            }

            return db_labs.SaveChanges();
        }
        
         public int MigrateMachine()
        {
            var main_department_RADIOLOGY = db_main.Departments.FirstOrDefault(e => e.DepartmentName == "RADIOLOGY");
            var main_department_PATHOLOGY = db_main.Departments.FirstOrDefault(e => e.DepartmentName == "PATHOLOGY");
            var main_department_HISTOCYTO  = db_main.Departments.FirstOrDefault(e => e.DepartmentName == "HISTOCYTO");

            var machines = db.machinemasters.Where(e => e.companyid == companyid).ToList();

            foreach (var m in machines)
            {
                db_labs.Machines.Add(
                    new Machine()
                    {
                        Id = m.mid,
                        Machine_Name = m.machinename,
                        CreatedUtc = DateTime.Now,
                        DepartmentRadPath = main_department_PATHOLOGY.Id
                    });

                db_labs.Machines.Add(
                    new Machine()
                    {
                        Id = m.mid,
                        Machine_Name = m.machinename,
                        CreatedUtc = DateTime.Now,
                        DepartmentRadPath = main_department_RADIOLOGY.Id
                    });

                db_labs.Machines.Add(
                    new Machine()
                    {
                        Id = m.mid,
                        Machine_Name = m.machinename,
                        CreatedUtc = DateTime.Now,
                        DepartmentRadPath = main_department_HISTOCYTO.Id
                    });
            }

            return db_labs.SaveChanges();
        }
        
        public int MigrateSampleTypes()
        {
            var main_department_RADIOLOGY = db_main.Departments.FirstOrDefault(e => e.DepartmentName == "RADIOLOGY");
            var main_department_PATHOLOGY = db_main.Departments.FirstOrDefault(e => e.DepartmentName == "PATHOLOGY");
            var main_department_HISTOCYTO = db_main.Departments.FirstOrDefault(e => e.DepartmentName == "HISTOCYTO");

            var data = db.SampleMasters.Where(e => e.companyid == companyid);

            foreach (var m in data)
            {
                db_labs.Samples.Add(
                    new LabsDataAccess.Sample()
                    {
                        Id = m.sampleid,
                        Sample_Type = m.Sampletype,
                        CreatedUtc = DateTime.Now,
                        DepartmentRadPath = main_department_PATHOLOGY.Id
                    });

                db_labs.Samples.Add(
                   new LabsDataAccess.Sample()
                   {
                       Id = m.sampleid,
                       Sample_Type = m.Sampletype,
                       CreatedUtc = DateTime.Now,
                       DepartmentRadPath = main_department_RADIOLOGY.Id
                   });

                db_labs.Samples.Add(
                   new LabsDataAccess.Sample()
                   {
                       Id = m.sampleid,
                       Sample_Type = m.Sampletype,
                       CreatedUtc = DateTime.Now,
                       DepartmentRadPath = main_department_HISTOCYTO.Id
                   });
            }

            return db_labs.SaveChanges();
        }

        int res = 0;
        int res2 = 0;

        public ActionResult MigrateTests()
        {
            var data = db.Titles_n.Where(c => c.companyid == companyid);
            var test_data = db.Test_n.Where(c => c.companyid == companyid);

            foreach (var m in data)
            {
                var dep = db_labs.Departments.FirstOrDefault(e => e.Department_Code == m.HCode);

                if (dep == null)
                {
                    continue;
                }

                var SexFlag = db_labs.Genders;
                bool tempbool = false;
                bool param = false;
                
                if (test_data.Any(e => e.TLCode.Contains(m.TLCode)))
                {
                    param = true;
                }

                var Sex = SexFlag.FirstOrDefault(e => e.Type == "All").Id;

                var _Sex = SexFlag.FirstOrDefault(e => e.Type.StartsWith(m.SexFlag));
                if (_Sex != null)
                {
                    Sex = _Sex.Id;
                }

                    var labTest =
                    new LabsDataAccess.LabTest()
                    {
                        //Id = m.TitleID,
                        Test = m.Title,
                        Department = dep.Id,
                        Test_Code = m.TLCode,


                        Order_Number = m.ORDNO.ToString(),
                        Test_Method = m.TestMethod_temp,
                        ICD10 = "",

                        HCPCS_Code = m.HCPCSCode,
                        CPT_Code = m.CPTCode,

                        Title_Machine_Name = m.testname_machine,
                        Shot_Form = "",
                        Preparation_Time = m.PreparationTime,
                        Specific_Instruction_For_Preparation = m.SpecificInstruction,
                        Sex = Sex,

                        Attach_Graph = bool.TryParse(m.graphflag.ToString(), out tempbool),

                        Update_All = false,
                        IsHide = m.IsHide,
                        GlocusePP = m.ISGlocusePP??false,
                        ParametersList = null,

                        Parameter = param,

                        DepartmentRadPath = dep.DepartmentRadPath
                    };

                    var _MachineName = db_labs.Machines.FirstOrDefault(e => e.Machine_Name == m.testname_machine);
                    if (_MachineName != null)
                    {
                        labTest.Machine_Name = _MachineName.Id;
                    }

                    if (m.Sampletype != null)
                    {
                        var _Sample = db_labs.Samples.FirstOrDefault(e => e.Sample_Type == m.Sampletype);

                        if (_Sample != null)
                        {
                            labTest.Sample_Type = _Sample.Id;
                        }
                    }

                    if (m.testname_machine != null && m.testname_machine != "Select Machine Name")
                    {
                        var _Machine = db_labs.Machines.FirstOrDefault(e => e.Machine_Name == m.Sampletype && e.DepartmentRadPath == dep.DepartmentRadPath);

                        if (_Machine != null)
                        {
                            labTest.Machine_Name = _Machine.Id;
                        }
                    }


                    db_labs.LabTests.Add(labTest);
                    res += db_labs.SaveChanges();

                    if (res > 0)
                    {
                        var userId = (int)Session["UserId"];
                        if (new Caresoft2._0.Controllers.MasterController().InsertTestsFromLab(labTest, userId))
                        {
                            res2 += 1;
                        }
                        else
                        {
                            db_labs.LabTests.Remove(labTest);
                            res -= db_labs.SaveChanges();
                        }

                    }

            }

            try
            {
               

                return Content(res.ToString() + " => " + res2.ToString());
            }
            catch (DbEntityValidationException ex)
            {
                //TO REMEMBER: How to return  specific validation error in EF
                // Retrieve the error messages as a list of strings.
                var errorMessages = ex.EntityValidationErrors
                        .SelectMany(x => x.ValidationErrors)
                        .Select(x => x.ErrorMessage);

                // Join the list to a single string.
                var fullErrorMessage = string.Join("; ", errorMessages);

                // Combine the original exception message with the new one.
                var exceptionMessage = string.Concat(ex.Message, " The validation errors are: ", fullErrorMessage);

                // Throw a new DbEntityValidationException with the improved exception message.
                //throw new DbEntityValidationException(exceptionMessage, ex.EntityValidationErrors);

                return Content(exceptionMessage);
            }

        }

        public ActionResult MigrateTestsParameters()
        {
            var data = db.Test_n.Where(c => c.companyid == companyid);

            foreach (var m in data)
            {
                var LabTest = db_labs.LabTests.FirstOrDefault(c => c.Test_Code == m.TLCode);
                var Machine = db_labs.Machines.FirstOrDefault(c => c.Machine_Name == m.testname_machine);
                int ORDNO = 0;
                try
                {
                    ORDNO = int.Parse(m.ORDNO.ToString());
                }
                catch (Exception e)
                {

                }

                if (LabTest != null)
                {
                    var par = new LabsDataAccess.Parameter()
                    {
                        Parameter_Name = m.TestName,
                        Parameter_Code = m.TLCode,
                        Parameter_Order = ORDNO,

                        Parameter_Method = m.DefaultTestMethod,
                        Title_Machine_Name = m.testname_machine,

                        Short_Form = m.scode,
                        Heading = null,
                        Test = LabTest.Id,


                        DepartmentRadPath = 0
                    };

                    if (Machine != null)
                    {
                        par.Machine_Name = Machine.Id;
                    }
                    db_labs.Parameters.Add(par);
                }

            }


            int res = 0;
            try
            {
                res = db_labs.SaveChanges();
                return Content(res.ToString());
            }
            catch (DbEntityValidationException ex)
            {
                //TO REMEMBER: How to return  specific validation error in EF
                // Retrieve the error messages as a list of strings.
                var errorMessages = ex.EntityValidationErrors
                        .SelectMany(x => x.ValidationErrors)
                        .Select(x => x.ErrorMessage);

                // Join the list to a single string.
                var fullErrorMessage = string.Join("; ", errorMessages);

                // Combine the original exception message with the new one.
                var exceptionMessage = string.Concat(ex.Message, " The validation errors are: ", fullErrorMessage);

                // Throw a new DbEntityValidationException with the improved exception message.
                //throw new DbEntityValidationException(exceptionMessage, ex.EntityValidationErrors);

                return Content(exceptionMessage);
            }
        }

        public int MigrateUsers()
        {
       
            var Employees = db.addusers.Where(e => e.companyid == companyid).ToList();

            foreach (var m in Employees)
            {
                var MainDepartment = db_main.Departments.FirstOrDefault(e => e.DepartmentName == m.MainDept);

                if (MainDepartment != null)
                {
                    var UserType = db_labs.UserTypes.FirstOrDefault(e => e.UserType1 == m.Usertype);
                    db_labs.DepartmentAssignments.Add(
                        new DepartmentAssignment()
                        {
                            User = 1,
                            UserName = m.username,
                            UserType = UserType.Id,
                            SingatureImage = "",
                            DepartmentRadPath = MainDepartment.Id
                        });
                }
                

            }
            return db_labs.SaveChanges();

            //db_main.Employees.Add(
            //new CaresoftHMISDataAccess.Employee() {
            //    RollNo = "NA",
            //    Salutation = "",
            //    FName = m.Fir,
            //    OtherName = "",
            //    Gender = "",

            //    DOB = "",
            //    IdNo = "",
            //    Mobile = "",
            //    Email = "",

            //    DesignationId = "",
            //    BranchId = "",
            //    DepartmentId = "",
            //    DateAdded = "",
            //});
        }

    }
}