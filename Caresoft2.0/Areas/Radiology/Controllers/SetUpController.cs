using Caresoft2._0.Areas.Radiology.Models;
using Caresoft2.Areas;
using CaresoftHMISDataAccess;
using LabsDataAccess;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;


namespace Caresoft2._0.Areas.Radiology.Controllers
{
    [Auth]
    public class SetUpController : Controller
    {
        private CaresoftHMISEntities db = new CaresoftHMISEntities();
        private CareSoftLabsEntities dblabs = new CareSoftLabsEntities();

        int main_department_id = new CaresoftHMISEntities().Departments.FirstOrDefault(d => d.DepartmentName.Equals("Radiology")).Id;

        // GET: HMS
        public ActionResult Index()
        {
            if (!dblabs.Status.Any() || !dblabs.PathKeyValuePairs.Any())
            {
                new Startup().setUpSettingsParameters();
            }
            return View();
        }
        public ActionResult AssignNewDepartment()
        {
            RadiologyAddNewUserData RadiologyAddNewUserData = new RadiologyAddNewUserData();
            RadiologyAddNewUserData.Department = db.Departments.Where(d => d.Id.Equals(main_department_id)).ToList();
            RadiologyAddNewUserData.MainDepartments = dblabs.Departments.Where(e => e.DepartmentRadPath.Equals(main_department_id)).ToList();
            RadiologyAddNewUserData.UserType = dblabs.UserTypes.ToList();

            List<int> existingUserIds = dblabs.DepartmentAssignments.Where(e => e.DepartmentRadPath.Equals(main_department_id)).Select(b => b.User).ToList();

            RadiologyAddNewUserData.Employees = db.Employees.Where(c => !existingUserIds.Contains(c.Id) && c.DepartmentId == main_department_id).ToList();


            return View(RadiologyAddNewUserData);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<int> AssignNewDepartment(DepartmentAssignmentModel newUserData)
        {

           

            var ueDeps = dblabs.User_Department.Where(ud => ud.DepartmentAssignment.User == newUserData.User && ud.DepartmentRadPath == main_department_id).ToList();
            dblabs.User_Department.RemoveRange(ueDeps);
            dblabs.SaveChanges();

            RadiologyAddNewUserData RadiologyAddNewUserData = new RadiologyAddNewUserData();
            RadiologyAddNewUserData.Department = db.Departments.Where(d => d.Id.Equals(main_department_id)).ToList();
            RadiologyAddNewUserData.MainDepartments = dblabs.Departments.Where(e => e.DepartmentRadPath.Equals(main_department_id)).ToList();
            RadiologyAddNewUserData.UserType = dblabs.UserTypes.ToList();

            List<int> existingUserIds = dblabs.DepartmentAssignments.Where(e => e.DepartmentRadPath.Equals(main_department_id)).Select(b => b.User).ToList();
            RadiologyAddNewUserData.Employees = db.Employees.Where(c => !existingUserIds.Contains(c.Id) && c.DepartmentId == main_department_id).ToList();

            if (ModelState.IsValid)
            {
                string[] departs = null;
                if (newUserData.Departments != null && newUserData.Departments.Length > 0)
                {
                    departs = newUserData.Departments.Split(',');
                }

                DepartmentAssignment assignments = new DepartmentAssignment();

                assignments.User = newUserData.User;
                assignments.DepartmentRadPath = main_department_id;

                var local_emp = db.Employees.Find(newUserData.User);
                var da = dblabs.DepartmentAssignments.FirstOrDefault(e => e.User == newUserData.User && e.DepartmentRadPath == main_department_id);
                if (da == null)
                {
                    bool checkUsernameExists = dblabs.DepartmentAssignments.Any(e => e.DepartmentRadPath == main_department_id &&
                    e.UserName.Equals(local_emp.FName + "" + local_emp.OtherName));
                    if (!checkUsernameExists)
                    {
                        assignments.UserName = local_emp.FName + "" + local_emp.OtherName;
                    }
                    else
                    {
                        assignments.UserName = local_emp.FName + "" + local_emp.OtherName + "1";
                    }
                }

                assignments.UserType = newUserData.UserType;

                if (da == null)
                {

                    var uid = dblabs.DepartmentAssignments.Add(assignments);

                    if (newUserData.SingatureImage != null)
                    {
                        assignments.SingatureImage = uid.Id.ToString() + Path.GetExtension(newUserData.SingatureImage.FileName);
                        UploadSignature(newUserData.SingatureImage, uid.Id);

                    }
                    else
                    {
                        da.SingatureImage = "";
                    }


                }
                else
                {

                    if (newUserData.SingatureImage != null)
                    {
                        da.SingatureImage = da.Id.ToString() + Path.GetExtension(newUserData.SingatureImage.FileName);
                        UploadSignature(newUserData.SingatureImage, da.Id);
                    }
                    else
                    {
                        da.SingatureImage = "";
                    }
                    da.UserType = assignments.UserType;
                    da.User_Department = assignments.User_Department;
                    da.DepartmentRadPath = main_department_id;
                    dblabs.Entry(da).State = EntityState.Modified;
                }

                await dblabs.SaveChangesAsync();

                var User_Department = dblabs.User_Department.Where(e => e.User == assignments.User && e.DepartmentRadPath == main_department_id);
                dblabs.User_Department.RemoveRange(User_Department);


                if (departs != null)
                {
                    foreach (var d in departs.ToList())
                    {
                        int depId = Convert.ToInt32(d);

                        User_Department department = new User_Department();
                        department.Department = depId;
                        department.User = assignments.Id;
                        department.DepartmentRadPath = main_department_id;
                        assignments.User_Department.Add(department);
                    }
                }
                await dblabs.SaveChangesAsync();

                //return Content(new JavaScriptSerializer().Serialize(departs.ToList().Count));

                return 1;

            }



            return 0;
        }

        private int UploadSignature(HttpPostedFileBase SingatureImage, int User)
        {
            if (SingatureImage != null)
            {
                string path = null;
                string FileName = null;

                string directory = "~/Areas/Radiology/Images/Signatures";
                bool exists = System.IO.Directory.Exists(Server.MapPath("directory"));

                if (!exists)
                    System.IO.Directory.CreateDirectory(Server.MapPath(directory));

                FileName = User + Path.GetExtension(SingatureImage.FileName);
                path = Path.Combine(Server.MapPath(directory), FileName);
                SingatureImage.SaveAs(path);
                return 1;


            }
            return 0;

        }

        public async Task<ActionResult> EditUser(int? user_type)
        {

            ViewBag.UserType = dblabs.UserTypes.ToList();

            var users = dblabs.DepartmentAssignments.Where(e => e.DepartmentRadPath == main_department_id).ToList();

            if (user_type != null)
            {
                users = users.Where(u => u.UserType == user_type).ToList();
            }

            return View(users.ToList());
        }

        public async Task<ActionResult> Edit(int Id)
        {
            var user = dblabs.DepartmentAssignments.SingleOrDefault(u => u.Id == Id);
            RadiologyAddNewUserData RadiologyAddNewUserData = new RadiologyAddNewUserData();
            RadiologyAddNewUserData.Department = db.Departments.Where(d => d.DepartmentName == "Radiology").ToList();
            RadiologyAddNewUserData.MainDepartments = dblabs.Departments.Where(e => e.DepartmentRadPath == main_department_id).ToList();
            RadiologyAddNewUserData.UserDepartmentAssignment = user;

            RadiologyAddNewUserData.Employees = db.Employees.Where(e => e.Id == user.User).ToList();
            RadiologyAddNewUserData.UserType = dblabs.UserTypes.ToList();


            return View(RadiologyAddNewUserData);

        }

        // GET: Departments
        public async Task<ActionResult> Departments()
        {
            return PartialView(await dblabs.Departments.Where(d => d.DepartmentRadPath.Equals(main_department_id)).ToListAsync());

        }

        // GET: Departments/Create
        public ActionResult AddDepartment()
        {
            ViewBag.Main_Department = db.Departments.Where(d => d.Id.Equals(main_department_id)).ToList().Select(
                    c => new SelectListItem
                    {
                        Text = c.DepartmentName,
                        Value = c.Id.ToString(),
                        Selected = (true),
                    }
                );
            return View();
        }

        // POST: Departments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddDepartment([Bind(Include = "Main_Department,Department1,Department_Code,Modality,Department_Order_No,Remark,Profile_Department")] LabsDataAccess.Department department)
        {
            if (ModelState.IsValid)
            {
                department.DepartmentRadPath = main_department_id;
                dblabs.Departments.Add(department);
                dblabs.SaveChanges();
                return RedirectToAction("AddDepartment");


            }

            return View(department);
        }

        // GET: Departments/Edit/5
        public async Task<ActionResult> EditDepartment(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LabsDataAccess.Department department = await dblabs.Departments.FindAsync(id);
            if (department == null)
            {
                return HttpNotFound();
            }
            return Json(new
            {
                Id = department.Id,
                Main_Department = department.Main_Department,
                Modality = department.Modality,
                Profile_Department = department.Profile_Department,
                Remark = department.Remark,
                Department1 = department.Department1,
                Department_Order_No = department.Department_Order_No,
                Department_Code = department.Department_Code,
            }, JsonRequestBehavior.AllowGet);
        }

        // POST: Departments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditDepartment([Bind(Include = "Id,Main_Department,Department1,Department_Code,Modality,Department_Order_No,Remark,Profile_Department")] LabsDataAccess.Department department)
        {
            if (ModelState.IsValid)
            {
                department.DepartmentRadPath = main_department_id;
                dblabs.Entry(department).State = EntityState.Modified;
                await dblabs.SaveChangesAsync();
                return RedirectToAction("AddDepartment");
            }
            ViewBag.Main_Department = new SelectList(dblabs.MainDepartments, "Id", "MainDepartment1", department.Main_Department);
            return View(department);
        }

        // POST: Departments/Delete/5
        [HttpPost, ActionName("DeleteDepartment")]
        public async Task<int> DeleteDepartment(int id)
        {
            LabsDataAccess.Department department = await dblabs.Departments.FindAsync(id);
            dblabs.Departments.Remove(department);
            return await dblabs.SaveChangesAsync();
        }

        public async Task<ActionResult> TestMaster(int? Departments, string TestCode, string TestName)
        {
            ViewBag.TestName = TestName;
            ViewBag.TestCode = TestCode;
            ViewBag.Departments = new SelectList(dblabs.Departments.Where(e => e.DepartmentRadPath.Equals(main_department_id)), "Id", "Department1");
            ViewBag.Departments1 = dblabs.Departments.Where(e => e.DepartmentRadPath.Equals(main_department_id)).ToList();
            ViewBag.MainDepartments = new SelectList(dblabs.MainDepartments.Where(e => e.DepartmentRadPath.Equals(main_department_id)), "Id", "DepartmentName");

            if (!Request.IsAjaxRequest())
            {
                ViewBag.Layout = "~/Areas/Radiology/Views/Shared/_LayoutRadiology.cshtml";
            }


            if (TestCode != null && !TestCode.Equals(""))
            {
                return View(await dblabs.LabTests.Where(e => e.Test_Code == TestCode && e.DepartmentRadPath.Equals(main_department_id)).ToListAsync());
            }
            else if (TestName != null && !TestName.Equals(""))
            {
                return View(await dblabs.LabTests.Where(e => e.Test == TestName && e.DepartmentRadPath.Equals(main_department_id)).ToListAsync());
            }
            else if (Departments != null)
            {
                return View(await dblabs.LabTests.Where(e => e.Department == Departments && e.DepartmentRadPath.Equals(main_department_id)).ToListAsync());
            }

            else
            {
                return View(await dblabs.LabTests.Where(e => e.DepartmentRadPath.Equals(main_department_id)).ToListAsync());
            }


        }

        public ActionResult AddTest()
        {
            ViewBag.Last_Test = dblabs.LabTests.OrderByDescending(e => e.Id).FirstOrDefault();
            ViewBag.Department = new SelectList(dblabs.Departments.Where(e => e.DepartmentRadPath.Equals(main_department_id)), "Id", "Department1");
            ViewBag.Sample = new SelectList(dblabs.Samples.Where(e => e.DepartmentRadPath.Equals(main_department_id)), "Id", "Sample_Type");
            ViewBag.Gender = new SelectList(dblabs.Genders, "Id", "Type");
            ViewBag.Machine = new SelectList(dblabs.Machines.Where(e => e.DepartmentRadPath.Equals(main_department_id)), "Id", "Machine_Name");

            return PartialView();


        }

        [System.Web.Mvc.HttpPost]
        [ValidateAntiForgeryToken]
        public int AddTest(/*[Bind(Include = "Id,Test,Department,TestName,TestCode,OrderNumber,TestMethod,ICD10,HCPCSCode,CPTCode,MachineName,TitleMachineName,SampleType,ShotForm,PreparationTime,SpecificInstructionForPreparation,Sex,AttachGraph,UpdateAll,IsHide,GlocusePP,Parameter")]*/ LabsDataAccess.LabTest labTests)
        {

            labTests.DepartmentRadPath = main_department_id;

            var last_labtest = dblabs.LabTests.OrderByDescending(e => e.Id).FirstOrDefault();
            
            if (last_labtest != null)
            {
                labTests.Test_Code = (last_labtest.Id + 1).ToString();
            }
            dblabs.LabTests.Add(labTests);

            var res =  dblabs.SaveChanges();

            if (res > 0)
            {
                var userId = (int)Session["UserId"];
                if(new Caresoft2._0.Controllers.MasterController().InsertTestsFromLab(labTests, userId))
                {
                    return res;
                }
                else
                {
                    dblabs.LabTests.Remove(labTests);
                    dblabs.SaveChanges();
                }

            }
            

           
            return 0;
        }

        public async Task<ActionResult> EditTest(int? id)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LabTest labTest = await dblabs.LabTests.FindAsync(id);

            ViewBag.Department = new SelectList(dblabs.Departments.Where(e => e.DepartmentRadPath.Equals(main_department_id)), "Id", "Department1", labTest.Department);
            ViewBag.Sample = new SelectList(dblabs.Samples.Where(e => e.DepartmentRadPath.Equals(main_department_id)), "Id", "Sample_Type", labTest.Sample);
            ViewBag.Gender = new SelectList(dblabs.Genders, "Id", "Type", labTest.Gender);
            ViewBag.Machine = new SelectList(dblabs.Machines.Where(e => e.DepartmentRadPath.Equals(main_department_id)), "Id", "Machine_Name", labTest.Machine);

            if (labTest == null)
            {
                return HttpNotFound();
            }

            if (Request.IsAjaxRequest())
            {
                return PartialView(labTest);
            }

            ViewBag.Layout = "~/Areas/Radiology/Views/Shared/_LayoutRadiology.cshtml";
            return View(labTest);

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<int> EditTest(LabTest labTest)
        {
            if (ModelState.IsValid)
            {
                labTest.DepartmentRadPath = main_department_id;
                dblabs.Entry(labTest).State = EntityState.Modified;
                return await dblabs.SaveChangesAsync();
            }
            return 0;
        }

        // GET: Test/Delete/5
        public async Task<ActionResult> DeleteTest(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LabTest labTest = await dblabs.LabTests.FindAsync(id);
            if (labTest == null)
            {
                return HttpNotFound();
            }

            if (Request.IsAjaxRequest())
            {
                return PartialView(labTest);
            }
            ViewBag.Layout = "~/Areas/Radiology/Views/Shared/_LayoutRadiology.cshtml";
            return View(labTest);
        }

        // POST: Test/Delete/5
        [HttpPost, ActionName("DeleteTest")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmedTest(int id)
        {
            LabTest labTest = await dblabs.LabTests.FindAsync(id);

            dblabs.LabTests.Remove(labTest);
            await dblabs.SaveChangesAsync();
            return RedirectToAction("TestMaster");
        }


        public async Task<ActionResult> MachineMaster()
        {
            ViewBag.Machines = await dblabs.Machines.Where(m => m.DepartmentRadPath.Equals(main_department_id)).ToListAsync();
            return PartialView();

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> MachineMaster(Machine machine)
        {
            machine.CreatedUtc = DateTime.Now;
            if (ModelState.IsValid)
            {
                if (machine.Id != null && machine.Id > 0)
                {
                    var mach = dblabs.Machines.Find(machine.Id);
                    mach.Machine_Name = machine.Machine_Name;

                }
                else
                {
                    machine.DepartmentRadPath = main_department_id;
                    dblabs.Machines.Add(machine);
                }
                dblabs.SaveChanges();

                ViewBag.Machines = await dblabs.Machines.Where(m => m.DepartmentRadPath.Equals(main_department_id)).ToListAsync();

                return PartialView();
            }
            ViewBag.Machines = await dblabs.Machines.Where(m => m.DepartmentRadPath.Equals(main_department_id)).ToListAsync();

            return PartialView(machine);
        }

        public async Task<ActionResult> EditMachine(int? id)
        {
            var machine = dblabs.Machines.Find(id);

            ViewBag.Machines = await dblabs.Machines.Where(m => m.DepartmentRadPath.Equals(main_department_id)).ToListAsync();
            return PartialView("MachineMaster", machine);

        }
        public async Task<ActionResult> DeleteMachine(int? id)
        {
            var machine = dblabs.Machines.Find(id);
            dblabs.Machines.Remove(machine);
            dblabs.SaveChanges();

            ViewBag.Machines = await dblabs.Machines.Where(m => m.DepartmentRadPath.Equals(main_department_id)).ToListAsync();
            return PartialView("MachineMaster");

        }
        public async Task<ActionResult> ProfileMaster()
        {
            return View(await dblabs.Profiles.Where(m => m.DepartmentRadPath.Equals(main_department_id)).ToListAsync());

        }


        public ActionResult AddProfile()
        {
            ViewBag.Profile_Department = new SelectList(dblabs.Departments.Where(d => d.Profile_Department == true &&
            d.DepartmentRadPath.Equals(main_department_id)), "Id", "Department1");
            ViewBag.Departments = dblabs.Departments.Where(m => m.DepartmentRadPath.Equals(main_department_id)).ToList();

            return PartialView();


        }
        [System.Web.Mvc.HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<int> AddProfile(Profile profile)
        {
            profile.DepartmentRadPath = main_department_id;
            dblabs.Profiles.Add(profile);
            return await dblabs.SaveChangesAsync();

        }


        public async Task<ActionResult> EditProfile(int? id)
        {


            ViewBag.Profile_Department = new SelectList(dblabs.Departments.Where(d => d.Profile_Department == true &&
               d.DepartmentRadPath.Equals(main_department_id)), "Id", "Department1");
            ViewBag.Departments = dblabs.Departments.Where(m => m.DepartmentRadPath.Equals(main_department_id)).ToList();


            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Profile profile = await dblabs.Profiles.FindAsync(id);
            if (profile == null)
            {
                return HttpNotFound();
            }

            if (Request.IsAjaxRequest())
            {
                return PartialView(profile);
            }
            ViewBag.Layout = "~/Areas/Radiology/Views/Shared/_LayoutRadiology.cshtml";
            return View(profile);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditProfile(Profile profile)
        {
            return Json(profile, JsonRequestBehavior.AllowGet);
            var prof = dblabs.Profiles.Find(profile.Id);

            prof.Group_Name = profile.Group_Name;
            prof.Group_Code = profile.Group_Code;
            prof.Profile_Method = profile.Profile_Method;
            prof.Profile_Department = profile.Profile_Department;
            prof.Selected_Tests = profile.Selected_Tests;

            //return await dblabs.SaveChangesAsync();



        }


        public async Task<ActionResult> ParameterMaster(int? id)
        {
            ViewBag.LabTests = dblabs.LabTests.Find(id);

            var par = await dblabs.Parameters.ToListAsync();
            if (id != null)
            {
                par = await dblabs.Parameters.Where(e => e.Test == id).ToListAsync();
            }
            else
            {
                par = await dblabs.Parameters.ToListAsync();

            }

            if (Request.IsAjaxRequest())
            {
                return PartialView(par);
            }
            else
            {
                ViewBag.Layout = "~/Areas/Radiology/Views/Shared/_LayoutRadiology.cshtml"; ;
                return View(par);
            }

        }

        public ActionResult AddParameter(int? TestId)
        {
            ViewBag.Last_Parameter = dblabs.Parameters.OrderByDescending(e => e.Id).FirstOrDefault();
            ViewBag.LabTests = dblabs.LabTests.Find(TestId);      
            ViewBag.Headings = new SelectList(dblabs.Headings.Where(e => e.DepartmentRadPath.Equals(main_department_id)), "Id", "Heading1");

            ViewBag.Machine = new SelectList(dblabs.Machines.Where(e => e.DepartmentRadPath.Equals(main_department_id)), "Id", "Machine_Name");

            if (Request.IsAjaxRequest())
            {
                return PartialView();
            }
            ViewBag.Layout = "~/Areas/Radiology/Views/Shared/_LayoutRadiology.cshtml";
            return PartialView();

        }
        [System.Web.Mvc.HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddParameter(Parameter parameter)
        {

            int labTest = (int)parameter.Test;
            parameter.DepartmentRadPath = main_department_id;

            var Last_Parameter = dblabs.Parameters.OrderByDescending(e => e.Id).FirstOrDefault();
            if (Last_Parameter != null)
            {
                parameter.Parameter_Code = Last_Parameter.Id.ToString();
            }

            if (ModelState.IsValid)
            {
                dblabs.Parameters.Add(parameter);

                await dblabs.SaveChangesAsync();
            }
            return RedirectToAction("ParameterMaster", new { id = labTest });
        }

        public async Task<ActionResult> EditParameter(int? id)
        {
            ViewBag.Machine = new SelectList(dblabs.Machines, "Id", "Machine_Name");
            ViewBag.Headings = new SelectList(dblabs.Headings, "Id", "Heading1");

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Parameter parameter = await dblabs.Parameters.FindAsync(id);
            if (parameter == null)
            {
                return HttpNotFound();
            }

            if (Request.IsAjaxRequest())
            {
                return PartialView(parameter);
            }
            ViewBag.Layout = "~/Areas/Radiology/Views/Shared/_LayoutRadiology.cshtml";
            return View(parameter);

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditParameter([Bind(Include = "Id,Parameter_Name,Parameter_Code,Parameter_Order,Parameter_Method,Machine_Name,Title_Machine_Name,Short_Form,Heading")] Parameter parameter)
        {
            int labTest = dblabs.Parameters.Find(parameter.Id).LabTest.Id;
            if (ModelState.IsValid)
            {
                var _parameter = dblabs.Parameters.Find(parameter.Id);

                _parameter.Parameter_Code = parameter.Parameter_Code;
                _parameter.Parameter_Method = parameter.Parameter_Method;
                _parameter.Machine_Name = parameter.Machine_Name;
                _parameter.Title_Machine_Name = parameter.Title_Machine_Name;
                _parameter.Short_Form = parameter.Short_Form; 
                _parameter.Heading = parameter.Heading;
                _parameter.Parameter_Order = parameter.Parameter_Order;

                dblabs.Entry(_parameter).State = EntityState.Modified;
                await dblabs.SaveChangesAsync();
            }
            return RedirectToAction("ParameterMaster", new { id = labTest });
        }

        // GET: Test/Delete/5
        public async Task<ActionResult> DeleteParameter(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Parameter parameter = await dblabs.Parameters.FindAsync(id);
            if (parameter == null)
            {
                return HttpNotFound();
            }

            if (Request.IsAjaxRequest())
            {
                return PartialView(parameter);
            }
            ViewBag.Layout = "~/Areas/Radiology/Views/Shared/_LayoutRadiology.cshtml";
            return View(parameter);
        }

        // POST: Test/Delete/5
        [HttpPost, ActionName("DeleteParameter")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmedParameter(int id)
        {
            Parameter parameter = await dblabs.Parameters.FindAsync(id);
            var labTest = parameter.LabTest;
            dblabs.Parameters.Remove(parameter);
            await dblabs.SaveChangesAsync();
            return RedirectToAction("ParameterMaster", new { id = labTest.Id });
        }


        public ActionResult AddNewHeading(int? TestId)
        {
            ViewBag.LabTests = dblabs.LabTests.Find(TestId);


            if (Request.IsAjaxRequest())
            {
                return PartialView();
            }
            ViewBag.Layout = "~/Areas/Radiology/Views/Shared/_LayoutRadiology.cshtml";
            return PartialView();

        }
        [System.Web.Mvc.HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<int> AddNewHeading(Heading heading)
        {
            heading.DepartmentRadPath = main_department_id;
            heading.CreatedUtc = DateTime.Now;
            if (ModelState.IsValid)
            {
                dblabs.Headings.Add(heading);

                return await dblabs.SaveChangesAsync();
            }
            return 0;
        }

        //Normal values 

        public ActionResult NormalValues(int? id)
        {
            int _id = 0;

            if (id != null && id > 0)
            {
                _id = (int)id;
            }

            ViewBag.Days = new SelectList(dblabs.Periods, "Id", "Days");
            ViewBag.Gender = new SelectList(dblabs.Genders, "Id", "Type");
            ViewBag.Departments = new SelectList(dblabs.Departments.Where(d => d.DepartmentRadPath.Equals(main_department_id)), "Id", "Department1");
            ViewBag.Test = new SelectList(dblabs.LabTests.Where(lt => lt.Department1.Id == _id && lt.DepartmentRadPath.Equals(main_department_id)), "Id", "Test");
            ViewBag.Parameter = new SelectList(dblabs.Parameters.Where(p => p.LabTest.Id == _id && p.DepartmentRadPath.Equals(main_department_id)), "Id", "Test");


            if (Request.IsAjaxRequest())
            {
                return PartialView();
            }
            else
            {
                ViewBag.Layout = "~/Areas/Radiology/Views/Shared/_LayoutRadiology.cshtml"; ;
                return View();
            }

        }
        class Days { public string Id { get; set; } public string Value { get; set; } };
        [System.Web.Mvc.HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult NormalValues(NormalValue normalValue)
        {
            normalValue.DepartmentRadPath = main_department_id;
            normalValue.CreatedUtc = DateTime.Now;
            if (!Request.IsAjaxRequest())
            {
                ViewBag.Layout = "~/Areas/Radiology/Views/Shared/_LayoutRadiology.cshtml";
            }

            if (normalValue.Parameter == 0)
            {
                normalValue.Parameter = null;
            }

            if (ModelState.IsValid)
            {
                if (normalValue.Id > 0)
                {
                    dblabs.Entry(normalValue).State = EntityState.Modified;
                }
                else
                {
                    dblabs.NormalValues.Add(normalValue);
                }
                dblabs.SaveChanges();
                return RedirectToAction("NormalValueList", new { id = normalValue.Test });
            }

            return RedirectToAction("NormalValueList", new { id = normalValue.Test });

        }
        public ActionResult NormalValuesOptions(int? id)
        {
            var cont = ("<option value='" + 0 + "'> " + "Select Test Name" + "</option>");
            var options = dblabs.LabTests.Where(e => e.Department1.Id == (int)id && e.DepartmentRadPath.Equals(main_department_id)).ToList();
            foreach (var option in options)
            {
                cont += ("<option value='" + option.Id + "'> " + option.Test + "</option>");
            }

            return Content(cont);

        }
        public ActionResult NormalValueList(int? id)
        {
            if (id == null)
            {
                id = 0;
            }
            var normalvals = dblabs.NormalValues.Where(e => e.Test == id && e.DepartmentRadPath.Equals(main_department_id)).OrderByDescending(i => i.Id).ToList();
            return PartialView(normalvals);
        }
        public async Task<ActionResult> EditNormalValue(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LabsDataAccess.NormalValue nv = await dblabs.NormalValues.FindAsync(id);
            if (nv == null)
            {
                return HttpNotFound();
            }
            return Json(new
            {
                Id = nv.Id,
                Gender = nv.Gender,
                Lower_Age = nv.Lower_Age,
                Upper_Age = nv.Upper_Age,
                Days = nv.Days,
                LowerRange = nv.LowerRange,
                UpperRange = nv.UpperRange,
                Unit = nv.Unit,
                General = nv.General,
                Test = nv.LabTest.Id,
                Parameter = nv.Parameter
            }, JsonRequestBehavior.AllowGet);
        }

        //End of Normal Vales

        //Panic values 

        public ActionResult PanicValues(int? id)
        {
            int _id = 0;

            if (id != null && id > 0)
            {
                _id = (int)id;
            }


            ViewBag.Days = new SelectList(dblabs.Periods, "Id", "Days");
            ViewBag.Gender = new SelectList(dblabs.Genders, "Id", "Type");
            ViewBag.Departments = new SelectList(dblabs.Departments.Where(d => d.DepartmentRadPath.Equals(main_department_id)), "Id", "Department1");
            ViewBag.Test = new SelectList(dblabs.LabTests.Where(lt => lt.Department1.Id == _id && lt.DepartmentRadPath.Equals(main_department_id)), "Id", "Test");
            ViewBag.Parameter = new SelectList(dblabs.Parameters.Where(p => p.LabTest.Id == _id && p.DepartmentRadPath.Equals(main_department_id)), "Id", "Test");



            if (Request.IsAjaxRequest())
            {
                return PartialView();
            }
            else
            {
                ViewBag.Layout = "~/Areas/Radiology/Views/Shared/_LayoutRadiology.cshtml"; ;
                return View();
            }

        }
        [System.Web.Mvc.HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PanicValues(PanicValue panicValue)
        {
            panicValue.DepartmentRadPath = main_department_id;
            panicValue.CreatedUtc = DateTime.Now;
            if (!Request.IsAjaxRequest())
            {
                ViewBag.Layout = "~/Areas/Radiology/Views/Shared/_LayoutRadiology.cshtml";
            }

            if (ModelState.IsValid)
            {
                if (panicValue.Id > 0)
                {
                    dblabs.Entry(panicValue).State = EntityState.Modified;
                }
                else
                {
                    dblabs.PanicValues.Add(panicValue);
                }
                dblabs.SaveChanges();

                return RedirectToAction("PanicValuesList", new { id = panicValue.Test });
            }

            return RedirectToAction("PanicValuesList", new { id = panicValue.Test });

        }
        public ActionResult PanicValuesTestOptions(int? id)
        {
            var cont = ("<option value='" + "'> " + "Select Test Name" + "</option>");
            var options = dblabs.LabTests.Where(e => e.Department1.Id == (int)id && e.DepartmentRadPath.Equals(main_department_id)).ToList();
            foreach (var option in options)
            {
                cont += ("<option value='" + option.Id + "'> " + option.Test + "</option>");
            }

            return Content(cont);

        }
        public ActionResult PanicValuesList(int? id)
        {
            if (id == null)
            {
                id = 0;
            }
            var panicValue = dblabs.PanicValues.Where(e => e.Test == id && e.DepartmentRadPath.Equals(main_department_id)).OrderByDescending(i => i.Id).ToList();
            return PartialView(panicValue);
        }

        public String PanicValuesParametersOptions(int? id)
        {
            if (id == null)
            {
                id = 0;
            }
            var cont = ("<option value='" + 0 + "'> " + "Select Parameter Name" + "</option>");
            var options = dblabs.Parameters.Where(e => e.LabTest.Id == (int)id).ToList();
            foreach (var option in options)
            {
                cont += ("<option value='" + option.Id + "'> " + option.Parameter_Name + "</option>");
            }

            return cont;
        }
        public async Task<ActionResult> EditPanicValues(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PanicValue nv = await dblabs.PanicValues.FindAsync(id);
            if (nv == null)
            {
                return HttpNotFound();
            }
            return Json(new
            {
                Id = nv.Id,
                Gender = nv.Gender,
                Lower_Age = nv.Lower_Age,
                Upper_Age = nv.Upper_Age,
                Days = nv.Days,
                LowerRange = nv.Lower_Range,
                UpperRange = nv.UpperRange,
                Unit = nv.Unit,
                General = nv.General,
                Test = nv.LabTest.Id,
                Parameter = nv.Parameter
            }, JsonRequestBehavior.AllowGet);
        }

        //End of Normal Vales
        public ActionResult SampleMaster()
        {
            ViewBag.SampleListModel = dblabs.Samples.Where(e => e.DepartmentRadPath.Equals(main_department_id)).OrderByDescending(e => e.Id).ToList();
            if (Request.IsAjaxRequest())
            {
                return PartialView();
            }
            else
            {
                ViewBag.Layout = "~/Areas/Radiology/Views/Shared/_LayoutRadiology.cshtml"; ;
                return View();
            }

        }
        [System.Web.Mvc.HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SampleMaster(Sample sample)
        {
            sample.CreatedUtc = DateTime.Now;

            ViewBag.Layout = "~/Areas/Radiology/Views/Shared/_LayoutRadiology.cshtml"; ;
            ViewBag.SampleListModel = dblabs.Samples.Where(e => e.DepartmentRadPath.Equals(main_department_id)).OrderByDescending(e => e.Id).ToList();

            if (ModelState.IsValid)
            {
                sample.DepartmentRadPath = main_department_id;
                if (sample.Id > 0)
                {
                    Sample samplein = dblabs.Samples.Find(sample.Id);
                    samplein.Sample_Type = sample.Sample_Type;
                    samplein.CreatedUtc = DateTime.Now;
                    dblabs.Entry(samplein).State = EntityState.Modified;
                }
                else
                {
                    dblabs.Samples.Add(sample);
                }
                dblabs.SaveChanges();
                return RedirectToAction("SampleMasterList");
            }

            return View("SampleMaster", sample);
        }
        public ActionResult SampleMasterList()
        {
            if (Request.IsAjaxRequest())
            {
                return PartialView(dblabs.Samples.Where(e => e.DepartmentRadPath.Equals(main_department_id)).ToList());
            }
            else
            {
                ViewBag.Layout = "~/Areas/Radiology/Views/Shared/_LayoutRadiology.cshtml"; ;
                return View(dblabs.Samples.Where(e => e.DepartmentRadPath.Equals(main_department_id)).ToList());
            }
        }
        public async Task<ActionResult> DeleteSample(int? id)
        {
            if (id == null)
            {
                return Content("0");
            }
            Sample sample = await dblabs.Samples.FindAsync(id);
            if (sample == null)
            {
                return Content("0");
            }
            dblabs.Samples.Remove(sample);
            dblabs.SaveChanges();
            return RedirectToAction("SampleMasterList");
        }

        //End Sample Master


        //InterpretationEntry
        public ActionResult InterpretationEntry(string Type)
        {

            if (Type == null || Type.Length == 0)
            {
                Type = "TestCode";

            }
            if (Type.Equals("TestCode"))
            {
                ViewBag.Test = new SelectList(dblabs.LabTests.Where(e => e.DepartmentRadPath.Equals(main_department_id)), "Id", "Test");

            }
            else if (Type.Equals("Profiles"))
            {
                ViewBag.Test = new SelectList(dblabs.Profiles.Where(e => e.DepartmentRadPath.Equals(main_department_id)), "Id", "Group_Code");
            }
            else
            {
                ViewBag.Selected = "TestCode";
                ViewBag.Test = new SelectList(dblabs.LabTests.Where(e => e.DepartmentRadPath.Equals(main_department_id)), "Id", "Test");

            }

            ViewBag.Selected = Type;

            if (Request.IsAjaxRequest())
            {
                return PartialView();
            }
            else
            {
                ViewBag.Layout = "~/Areas/Radiology/Views/Shared/_LayoutRadiology.cshtml"; ;
                return View();
            }

        }
        [System.Web.Mvc.HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult InterpretationEntry(InterpretationEntry interpretationEntry, string Type)
        {
            interpretationEntry.CreatedUtc = DateTime.Now;
            interpretationEntry.DepartmentRadPath = main_department_id;
            ViewBag.Layout = "~/Areas/Radiology/Views/Shared/_LayoutRadiology.cshtml"; ;
            ViewBag.SampleListModel = dblabs.Samples.Where(e => e.DepartmentRadPath.Equals(main_department_id)).OrderByDescending(e => e.Id).ToList();

            if (Type.Equals("TestCode"))
            {
                ViewBag.Selected = "TestCode";

                interpretationEntry.Profile = null;
            }
            else if (Type.Equals("Profiles"))
            {
                interpretationEntry.Test = null;
                ViewBag.Selected = "Profiles";

            }
            if (ModelState.IsValid)
            {
                var prev_IE = dblabs.InterpretationEntries.FirstOrDefault(i => i.Test == interpretationEntry.Test &&
                i.DepartmentRadPath.Equals(main_department_id));
                if (prev_IE != null)
                {
                    prev_IE.Data = interpretationEntry.Data;
                    prev_IE.CreatedUtc = DateTime.Now;
                    dblabs.Entry(prev_IE).State = EntityState.Modified;
                }
                else
                {
                    dblabs.InterpretationEntries.Add(interpretationEntry);
                }
                dblabs.SaveChanges();
                return RedirectToAction("InterpretationEntry");
            }

            return View(interpretationEntry);
        }
        public ActionResult InterpretationEntryList()
        {
            if (Request.IsAjaxRequest())
            {
                return PartialView(dblabs.Samples.Where(e => e.DepartmentRadPath.Equals(main_department_id)).ToList());
            }
            else
            {
                ViewBag.Layout = "~/Areas/Radiology/Views/Shared/_LayoutRadiology.cshtml"; ;
                return View(dblabs.Samples.Where(e => e.DepartmentRadPath.Equals(main_department_id)).ToList());
            }
        }
        public ActionResult getInterpretationData(int id, string Type)
        {
            if (Type.Equals("Profiles"))
            {
                dblabs.Configuration.ProxyCreationEnabled = false;
                return Json(dblabs.InterpretationEntries.FirstOrDefault(i => i.Profiles == id && i.DepartmentRadPath.Equals(main_department_id)), JsonRequestBehavior.AllowGet);

            }
            else if (Type.Equals("TestCode"))
            {
                dblabs.Configuration.ProxyCreationEnabled = false;
                return Json(dblabs.InterpretationEntries.FirstOrDefault(i => i.Test == id && i.DepartmentRadPath.Equals(main_department_id)), JsonRequestBehavior.AllowGet);

            }
            else
            {
                return null;
            }
        }

        //End InterpretationEntry


        //Reasons master
        public ActionResult ReasonMaster()
        {
            if (Request.IsAjaxRequest())
            {
                return PartialView();
            }
            else
            {
                ViewBag.Layout = "~/Areas/Radiology/Views/Shared/_LayoutRadiology.cshtml"; ;
                return View();
            }

        }
        [System.Web.Mvc.HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ReasonMaster(ReasonMaster reasonMaster)
        {
            reasonMaster.DepartmentRadPath = main_department_id;
            reasonMaster.CreatedUtc = DateTime.Now;
            ViewBag.Layout = "~/Areas/Radiology/Views/Shared/_LayoutRadiology.cshtml";

            if (ModelState.IsValid)
            {
                if (reasonMaster.Id > 0)
                {
                    dblabs.Entry(reasonMaster).State = EntityState.Modified;
                }
                else
                {
                    dblabs.ReasonMasters.Add(reasonMaster);
                }
                dblabs.SaveChanges();
                return RedirectToAction("ReasonMaster");
            }

            return View("ReasonMaster", reasonMaster);
        }
        public ActionResult ReasonMasterList()
        {
            if (Request.IsAjaxRequest())
            {
                return PartialView(dblabs.ReasonMasters.Where(e => e.DepartmentRadPath.Equals(main_department_id)).ToList());
            }
            else
            {
                ViewBag.Layout = "~/Areas/Radiology/Views/Shared/_LayoutRadiology.cshtml"; ;
                return View(dblabs.ReasonMasters.ToList());
            }
        }

        //End Reasons

        //Start of Machine Details


        //Start of Machine Details
        public ActionResult MachineDetails()
        {
            ViewBag.MachineName = dblabs.Machines.Where(e => e.DepartmentRadPath.Equals(main_department_id)).ToList();
            ViewBag.TestName = dblabs.LabTests.Where(e => e.DepartmentRadPath.Equals(main_department_id)).ToList();

            //return main view
            return View(dblabs.MachineDetails.Where(e => e.DepartmentRadPath.Equals(main_department_id)).ToList());
        }

        //Return View for Adding Machine Details 
        public ActionResult AddMachineDetails()
        {
            ViewBag.MachineName = dblabs.Machines.Where(e => e.DepartmentRadPath.Equals(main_department_id)).ToList();
            ViewBag._Title = dblabs.Headings.Where(e => e.DepartmentRadPath.Equals(main_department_id)).ToList();
            ViewBag.Department = dblabs.Departments.Where(e => e.DepartmentRadPath.Equals(main_department_id)).ToList();
            return PartialView();
        }

        //Gets HTTP Request and persists the data for  Machine Details
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddMachineDetails(MachineDetail machineDetails)
        {
            machineDetails.CreatedUtc = DateTime.Now;
            machineDetails.DepartmentRadPath = main_department_id;
            //Check if the model is valid
            if (ModelState.IsValid)
            {
                dblabs.MachineDetails.Add(machineDetails);

                //persist the data
                dblabs.SaveChanges();

                return RedirectToAction("MachineDetails");
            }
            return PartialView(machineDetails);
        }

        //End of Machine Details

        //Start of SOPMaster

        public ActionResult SOPMaster()
        {
            ViewBag.Departments = dblabs.Departments.Where(e => e.DepartmentRadPath.Equals(main_department_id)).ToList();
            ViewBag.Test = dblabs.LabTests.Where(e => e.DepartmentRadPath.Equals(main_department_id)).ToList();
            //return main view
            return View();
        }

        public JsonResult searchSOPTests(int id, string search)
        {
            var tests = dblabs.LabTests.Where(e => e.Department == id && e.Test.Contains(search)).Select(x => new
            {
                id = x.Id,
                Test = x.Test
            });
            return Json(tests, JsonRequestBehavior.AllowGet);
        }

        public JsonResult getSOP(int id)
        {
            var Sop = dblabs.StandardOperative_Procedures.
                Select(x => new { x.Test, x.Standard_Operative_Procedure }).FirstOrDefault(e => e.Test == id);
            return Json(Sop, JsonRequestBehavior.AllowGet);
        }

        //Gets HTTP Request and persists the data for  Machine Details
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult SOPMaster(StandardOperative_Procedures procedure)
        {
            ViewBag.Departments = dblabs.Departments.Where(e => e.DepartmentRadPath.Equals(main_department_id)).ToList();
            ViewBag.Test = dblabs.LabTests.Where(e => e.DepartmentRadPath.Equals(main_department_id)).ToList();
            procedure.DepartmentRadPath = main_department_id;
            procedure.CreatedUtc = DateTime.Now;

            if (ModelState.IsValid)
            {
                dblabs.StandardOperative_Procedures.Add(procedure);
                //persist the data
                dblabs.SaveChanges();

                return RedirectToAction("SOPMaster");
            }

            return View(procedure);
        }

        //End of SOPMaster



        //Start of Test Turn Around Time


        public ActionResult TestTurnAroundTime()
        {
            ViewBag.VialTypes = dblabs.VialMasters.Where(e => e.DepartmentRadPath.Equals(main_department_id)).ToList();
            //return main view
            return View();
        }

        public JsonResult searchTTATTests(string search)
        {
            var tests = dblabs.LabTests.Where(e => e.Test.Contains(search) && e.DepartmentRadPath.Equals(main_department_id)).Select(x => new
            {
                id = x.Id,
                Test = x.Test
            });
            return Json(tests, JsonRequestBehavior.AllowGet);
        }

        public JsonResult getSingleTTAT(int id)
        {

            var test = (from x in dblabs.Set<Test_Turn_Around_Times>().Where(e => e.DepartmentRadPath == main_department_id)
                        select new
                        {
                            Id = x.Id,
                            Test = x.LabTest.Test,
                            TestId = x.Test,
                            TTAT_Days = x.TTAT_Days,
                            TTAT_Hours = x.TTAT_Hours,
                            RDD_Days = x.RDD_Days,
                            RDD_Hours = x.RDD_Hours,
                            Vial_Type = x.Vial_Type,
                            srno = x.srno,
                            srno1 = x.srno1
                        }).FirstOrDefault(e => e.TestId == id);


            if (test != null)
            {
                return Json(test, JsonRequestBehavior.AllowGet);
            }

            return Json("null", JsonRequestBehavior.AllowGet);



            //var test = dblabs.Test_Turn_Around_Times.
            //    Select(x => new Test_Turn_Around_Times {
            //        Id = x.Id,
            //        Test = x.Test,
            //        TTAT_Days = x.TTAT_Days,
            //        TTAT_Hours = x.TTAT_Hours,
            //        RDD_Days = x.RDD_Days,
            //        RDD_Hours = x.RDD_Hours,
            //        Vial_Type = x.Vial_Type,
            //        srno = x.srno,
            //        srno1 = x.srno1}).FirstOrDefault(t => t.Test.Equals(id) &&
            //                            t.DepartmentRadPath.Equals(main_department_id));

        }
        //Gets HTTP Request and persists the data for  Machine Details
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult TestTurnAroundTime(Test_Turn_Around_Times turn_Around_Time)
        {
            ViewBag.Departments = dblabs.Departments.Where(e => e.DepartmentRadPath.Equals(main_department_id)).ToList();
            ViewBag.Test = dblabs.LabTests.Where(e => e.DepartmentRadPath.Equals(main_department_id)).ToList();

            turn_Around_Time.DepartmentRadPath = main_department_id;
            //turn_Around_Time.CreatedUtc = DateTime.Now;

            if (turn_Around_Time.Vial_Type == 0)
            {
                turn_Around_Time.Vial_Type = null;
            }
            if (ModelState.IsValid)
            {
                var dataexists = dblabs.Test_Turn_Around_Times.FirstOrDefault(e => e.Test == turn_Around_Time.Test && e.DepartmentRadPath.Equals(main_department_id));
                if (dataexists == null)
                {
                    dblabs.Test_Turn_Around_Times.Add(turn_Around_Time);
                }
                else
                {
                    dataexists.RDD_Days = turn_Around_Time.RDD_Days;
                    dataexists.RDD_Hours = turn_Around_Time.RDD_Hours;
                    dataexists.srno = turn_Around_Time.srno;
                    dataexists.srno1 = turn_Around_Time.srno1;
                    dataexists.TTAT_Days = turn_Around_Time.TTAT_Days;
                    dataexists.TTAT_Hours = turn_Around_Time.TTAT_Hours;
                    dataexists.Vial_Type = turn_Around_Time.Vial_Type;
                }

                //persist the data
                dblabs.SaveChanges();

                return RedirectToAction("TestTurnAroundTime");
            }

            return RedirectToAction("TestTurnAroundTime");
        }

        //End of Test Turn Around Time


        //Start of Formula Setting
        public ActionResult FormulaSetting()
        {
            ViewBag.Departments = dblabs.Departments.Where(e => e.DepartmentRadPath.Equals(main_department_id)).ToList();

            //return main view
            if (Request.IsAjaxRequest())
            {
                return PartialView();
            }
            else
            {
                //universal layout 
                ViewBag.Layout = "~/Areas/Radiology/Views/Shared/_LayoutRadiology.cshtml"; ;
                return View();
            }
        }

        public ActionResult getDepartmentTests(int id)
        {
            List<string> departmentOptions = new List<string>();

            var data = dblabs.LabTests.Where(t => t.Department.Equals(id)).ToList();
            var cont = "";
            foreach (var i in data)
            {
                cont += ("<label style='width: 100% padding-right:10px; padding-left:10px' class='form-check-label' for=test" + i.Id + "'><input type = 'checkbox' class='form-check-input TestsCheckbox' identity= " + i.Id + " id=test'" + i.Id + "'> &nbsp; " + i.Test + "</label><br>");
            }

            return Content(cont);
        }


        public ActionResult getTestParameters(int id)
        {
            List<string> departmentOptions = new List<string>();

            var data = dblabs.Parameters.Where(p => p.Test != null && p.Test == id && p.DepartmentRadPath.Equals(main_department_id)).ToList();
            var cont = "";
            foreach (var i in data)
            {
                cont += ("<label style='width: 100% padding-right:10px; padding-left:10px' class='form-check-label' for=par" + i.Id + "'><input type = 'checkbox' class='form-check-input ParamsCheckbox' test = " + i.Test + "  identity= " + i.Id + " id =par'" + i.Id + "'> &nbsp; " + i.Parameter_Name + "</label><br>");
            }

            return Content(cont);
        }


        //End of Formula Setting

        // Start of VialTypeMaster

        public ActionResult VialTypeMaster()
        {
            ViewBag.Vials = dblabs.VialMasters.Where(e => e.DepartmentRadPath.Equals(main_department_id)).ToList();
            //universal layout 
            ViewBag.Layout = "~/Areas/Radiology/Views/Shared/_LayoutRadiology.cshtml"; ;
            return View();
        }

        //Post method for Vial
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult VialTypeMaster(VialMaster vial)
        {
            //universal layout 
            ViewBag.Layout = "~/Areas/Radiology/Views/Shared/_LayoutRadiology.cshtml";
            vial.DepartmentRadPath = main_department_id;
            if (ModelState.IsValid)
            {
                if (vial.Id == null || vial.Id == 0)
                {
                    dblabs.VialMasters.Add(vial);
                }
                else
                {
                    var dbvial = dblabs.VialMasters.Find(vial.Id);
                    dbvial.Vial_Type = vial.Vial_Type;
                    dbvial.CreatedUtc = DateTime.Now;

                }

                dblabs.SaveChanges();
                ViewBag.Vials = dblabs.VialMasters.Where(e => e.DepartmentRadPath.Equals(main_department_id)).ToList();

                return RedirectToAction("VialTypeMaster");
            }
            ViewBag.Vials = dblabs.VialMasters.Where(e => e.DepartmentRadPath.Equals(main_department_id)).ToList();

            return View(vial);
        }
        //End of VialTypeMaster

        // Start of Short form

        public ActionResult ShortFormMaster()
        {
            //universal layout 
            ViewBag.Layout = "~/Areas/Radiology/Views/Shared/_LayoutRadiology.cshtml";
            return View(dblabs.ShortForms.Where(e => e.DepartmentRadPath.Equals(main_department_id)).ToList());
        }

        public ActionResult ShortFormEntry()
        {
            ViewBag.Test = new SelectList(dblabs.LabTests.Where(e => e.DepartmentRadPath.Equals(main_department_id)), "Id", "Test");
            //universal layout 

            if (Request.IsAjaxRequest())
            {
                return PartialView();
            }
            else
            {
                //universal layout 
                ViewBag.Layout = "~/Areas/Radiology/Views/Shared/_LayoutRadiology.cshtml"; ;
                return View();
            }
        }

        //Post method for Vial
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ShortFormEntry(ShortForm shortForm)
        {
            //universal layout 
            ViewBag.Layout = "~/Areas/Radiology/Views/Shared/_LayoutRadiology.cshtml";
            shortForm.DepartmentRadPath = main_department_id;
            if (ModelState.IsValid)
            {
                if (shortForm.Id == null || shortForm.Id == 0)
                {
                    dblabs.ShortForms.Add(shortForm);
                }
                else
                {
                    var dbvial = dblabs.ShortForms.Find(shortForm.Id);
                    dbvial.Short_Form = shortForm.Short_Form;
                    dbvial.Test = shortForm.Test;
                    dbvial.Description = shortForm.Description;
                    dbvial.CreatedUtc = DateTime.Now;

                }

                dblabs.SaveChanges();

                return RedirectToAction("ShortFormMaster");
            }
            ViewBag.Vials = dblabs.VialMasters.Where(e => e.DepartmentRadPath.Equals(main_department_id)).ToList();

            return View(shortForm);
        }
        //End of  Short form


        //Start of Marquee
        public ActionResult MarqueeMaster()
        {
            //universal layout 
            ViewBag.Layout = "~/Areas/Radiology/Views/Shared/_LayoutRadiology.cshtml";

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult MarqueeMaster(PathKeyValuePair marquee)
        {
            //universal layout 
            ViewBag.Layout = "~/Areas/Radiology/Views/Shared/_LayoutRadiology.cshtml";
            marquee.DepartmentRadPath = main_department_id;
            if (ModelState.IsValid)
            {
                var marq = dblabs.PathKeyValuePairs.FirstOrDefault(e => e.Key_.Equals("RadiologyDepartmentMarquee"));

                if (marq != null)
                {
                    marq.Value = marquee.Value;
                }
                else
                {
                    marquee.Key_ = "RadiologyDepartmentMarquee";
                    dblabs.PathKeyValuePairs.Add(marquee);
                }

                dblabs.SaveChanges();

                return RedirectToAction("MarqueeMaster");
            }

            return View(marquee);
        }
        //End of  Marquee

        public ActionResult TestAvailability()
        {
            if (!Request.IsAjaxRequest())
            {
                ViewBag.Layout = "~/Areas/Radiology/Views/Shared/_LayoutRadiology.cshtml";
            }

            var data = dblabs.LabTests.Where(e => e.DepartmentRadPath == main_department_id);
            return View(data);
        }

        public ActionResult SearchTestAvailability(string search)
        {
            var data = dblabs.LabTests.Where(e => e.DepartmentRadPath == main_department_id);

            if (search != null && search.Length > 0)
            {
                data = data.Where(f => f.Test.Contains(search) || f.Test_Code.Contains(search));
            }

            return PartialView(data);
        }

        [HttpPost]
        public ActionResult TestAvailability(int id)
        {
            if (!Request.IsAjaxRequest())
            {
                ViewBag.Layout = "~/Areas/Radiology/Views/Shared/_LayoutRadiology.cshtml";
            }

            var LabTest = dblabs.LabTests.Find(id);
            if (LabTest != null)
            {
                if (LabTest.Available == true)
                {
                    LabTest.Available = false;
                }
                else
                {
                    LabTest.Available = true;
                }
            }
            dblabs.SaveChanges();
            var data = dblabs.LabTests.Where(e => e.DepartmentRadPath == main_department_id);
            return PartialView(data);
        }
    }

}