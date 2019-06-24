using CaresoftHMISDataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Caresoft2._0.Areas.HumanResource.Controllers
{
    [Auth]
    public class HomeController : Controller
    {
        CaresoftHMISEntities db = new CaresoftHMISEntities();
        // GET: HumanResource/Home
        #region Branch
        public ActionResult Index()
        {
            ViewBag.BranchMaster = db.BranchMasters.ToList();
            var data = db.BranchMasters.ToList();

            return View(data);
        }
        [HttpPost]
        public ActionResult SaveBranchesData(BranchMaster data)
        {
            data.UserId = (int)Session["UserId"];
            data.DateAdded = DateTime.Now;
            //data.BranchId = 1;


            db.BranchMasters.Add(data);
            db.SaveChanges();

            return RedirectToAction("Index");
        }
        #endregion
        #region Country
        public ActionResult Country()
        {
            ViewBag.HRCountry = db.HRCountries.ToList();
            var data = db.HRCountries.ToList();

            return View(data);
        }
        [HttpPost]
        public ActionResult SaveCountryData(HRCountry data)
        {
            data.UserId = (int)Session["UserId"];
            data.DateAdded = DateTime.Now;
            data.BranchId = 1;


            db.HRCountries.Add(data);
            db.SaveChanges();

            return RedirectToAction("Country");
        }
        #endregion
        #region Geographical Locaton
        public ActionResult GeographicalLocation()
        {
            ViewBag.HRGeoLocation = db.HRGeoLocations.ToList();
            var data = db.HRGeoLocations.ToList();

            return View(data);
        }
        [HttpPost]
        public ActionResult SaveGeographicalData(HRGeoLocation data)
        {
            data.UserId = (int)Session["UserId"];
            data.DateAdded = DateTime.Now;
            data.BranchId = 1;


            db.HRGeoLocations.Add(data);
            db.SaveChanges();

            return RedirectToAction("GeographicalLocation");
        }
        #endregion
        #region District
        public ActionResult District()
        {
            ViewBag.HRDistrict = db.HRDistricts.ToList();
            var data = db.HRDistricts.ToList();

            return View(data);
        }
        [HttpPost]
        public ActionResult SaveDistrictData(HRDistrict data)
        {
            data.UserId = (int)Session["UserId"];
            data.DateAdded = DateTime.Now;
            data.BranchId = 1;


            db.HRDistricts.Add(data);
            db.SaveChanges();

            return RedirectToAction("District");
        }
        #endregion
        #region Town
        public ActionResult Town()
        {
            ViewBag.HRTown = db.HRTowns.ToList();
            var data = db.HRTowns.ToList();

            return View(data);
        }
        [HttpPost]
        public ActionResult SaveTownData(HRTown data)
        {
            data.UserId = (int)Session["UserId"];
            data.DateAdded = DateTime.Now;
            data.BranchId = 1;


            db.HRTowns.Add(data);
            db.SaveChanges();

            return RedirectToAction("Town");
        }
        #endregion
        #region Departments
        public ActionResult DeptN()
        {
            ViewBag.Departments = db.Departments.ToList();
            var departments = db.Departments.Where(e => e.DepartmentName != null).ToList();

            return View(departments);
        }
        public ActionResult Departments()
        {
            ViewBag.HRDepartment = db.HRDepartments.ToList();
            var data = db.HRDepartments.ToList();

            return View(data);
        }
        [HttpPost]
        public ActionResult SaveDepartmentData(HRDepartment data)
        {
            data.UserId = (int)Session["UserId"];
            data.DateAdded = DateTime.Now;
            data.BranchId = 1;


            db.HRDepartments.Add(data);
            db.SaveChanges();

            return RedirectToAction("Departments");
        }
        #endregion
        #region Sections
        public ActionResult Section()
        {
            ViewBag.HRSection = db.HRSections.ToList();
            var data = db.HRSections.ToList();

            return View(data);
        }
        [HttpPost]
        public ActionResult SaveSectionData(HRSection data)
        {
            data.UserId = (int)Session["UserId"];
            data.DateAdded = DateTime.Now;
            data.BranchId = 1;


            db.HRSections.Add(data);
            db.SaveChanges();

            return RedirectToAction("Section");
        }
        #endregion
        #region Benches

        public ActionResult DepartmentsNames()
        {
            ViewBag.Departments = db.Departments.ToList();
            var departments = db.Departments.Where(e => e.DepartmentName != null).ToList();

            return View(departments);
        }

        public ActionResult Benches()
        {
            ViewBag.HRBench = db.HRBenches.ToList();
            var data = db.HRBenches.ToList();

            return View(data);
        }
        [HttpPost]
        public ActionResult SaveBenchesData(HRBench data)
        {
            data.UserId = (int)Session["UserId"];
            data.DateAdded = DateTime.Now;
            data.BranchId = 1;


            db.HRBenches.Add(data);
            db.SaveChanges();

            return RedirectToAction("Benches");
        }
        #endregion
        #region Departmental Layout
        public ActionResult DeptNames()
        {
            ViewBag.Departments = db.Departments.ToList();
            var departments = db.Departments.Where(e => e.DepartmentName != null).ToList();

            return View(departments);
        }

        public ActionResult DepartmentalLayout()
        {
            ViewBag.Departments = db.Departments.ToList();

            ViewBag.HRDepartmentalLayout = db.HRDepartmentalLayouts.ToList();
            var data = db.HRDepartmentalLayouts.ToList();

            return View(data);
        }
        [HttpPost]
        public ActionResult SaveDepartmentalData(HRDepartmentalLayout data)
        {
            data.UserId = (int)Session["UserId"];
            data.DateAdded = DateTime.Now;
            data.BranchId = 1;


            db.HRDepartmentalLayouts.Add(data);
            db.SaveChanges();

            return RedirectToAction("DepartmentalLayout");
        }
        #endregion
        #region Duty Station
        public ActionResult DutyStation()
        {
            ViewBag.HRDutyStation = db.HRDutyStations.ToList();
            var data = db.HRDutyStations.ToList();

            return View(data);
        }
        [HttpPost]
        public ActionResult SaveDutyStationData(HRDutyStation data)
        {
            data.UserId = (int)Session["UserId"];
            data.DateAdded = DateTime.Now;
            data.BranchId = 1;


            db.HRDutyStations.Add(data);
            db.SaveChanges();

            return RedirectToAction("DutyStation");
        }
        #endregion
        #region Meeting venues
        public ActionResult MeetingVenue()
        {
            ViewBag.HRMeetingVenue = db.HRMeetingVenues.ToList();
            var data = db.HRMeetingVenues.ToList();

            return View(data);
        }
        [HttpPost]
        public ActionResult SaveMeetingVenuesData(HRMeetingVenue data)
        {
            data.UserId = (int)Session["UserId"];
            data.DateAdded = DateTime.Now;
            data.BranchId = 1;


            db.HRMeetingVenues.Add(data);
            db.SaveChanges();

            return RedirectToAction("MeetingVenue");
        }
        #endregion
        #region Statutory Bodies
        public ActionResult StatutoryBodies()
        {
            ViewBag.HRStatutoryBody = db.HRStatutoryBodies.ToList();
            var data = db.HRStatutoryBodies.ToList();

            return View(data);
        }
        [HttpPost]
        public ActionResult SaveStatutoryBodyData(HRStatutoryBody data)
        {
            data.UserId = (int)Session["UserId"];
            data.DateAdded = DateTime.Now;
            data.BranchId = 1;


            db.HRStatutoryBodies.Add(data);
            db.SaveChanges();

            return RedirectToAction("StatutoryBodies");
        }
        #endregion
        #region Training Programme
        public ActionResult DepNames()
        {
            ViewBag.Departments = db.Departments.ToList();
            var departments = db.Departments.Where(e => e.DepartmentName != null).ToList();

            return View(departments);
        }

        public ActionResult TrainingProgrammes()
        {
            ViewBag.Departments = db.Departments.ToList();

            ViewBag.HRTrainingProgramme = db.HRTrainingProgrammes.ToList();
            var data = db.HRTrainingProgrammes.ToList();

            return View(data);
        }
        [HttpPost]
        public ActionResult SaveTrainingProgrammeData(HRTrainingProgramme data)
        {
            data.UserId = (int)Session["UserId"];
            data.DateAdded = DateTime.Now;
            data.BranchId = 1;


            db.HRTrainingProgrammes.Add(data);
            db.SaveChanges();

            return RedirectToAction("TrainingProgrammes");
        }
        #endregion
        #region ContractType
        public ActionResult ContractType()
        {
            ViewBag.HRContract = db.HRContracts.ToList();
            var data = db.HRContracts.ToList();

            return View(data);
        }
        [HttpPost]
        public ActionResult SaveContractsData(HRContract data)
        {
            data.UserId = (int)Session["UserId"];
            data.DateAdded = DateTime.Now;
            data.BranchId = 1;


            db.HRContracts.Add(data);
            db.SaveChanges();

            return RedirectToAction("ContractType");
        }
        #endregion
        #region Contract Termination
        public ActionResult ContractTermination()
        {
            ViewBag.HRContractTermination = db.HRContractTerminations.ToList();
            var data = db.HRContractTerminations.ToList();

            return View(data);
        }
        [HttpPost]
        public ActionResult SaveTerminationData(HRContractTermination data)
        {
            data.UserId = (int)Session["UserId"];
            data.DateAdded = DateTime.Now;
            data.BranchId = 1;


            db.HRContractTerminations.Add(data);
            db.SaveChanges();

            return RedirectToAction("ContractTermination");
        }
        #endregion
        #region Casual Levels
        public ActionResult Depts()
        {
            ViewBag.Departments = db.Departments.ToList();
            var departments = db.Departments.Where(e => e.DepartmentName != null).ToList();

            return View(departments);
        }

        public ActionResult CasualLevels()
        {
            ViewBag.Departments = db.Departments.ToList();

            ViewBag.HRJobSetUp = db.HRJobSetUps.ToList();
            var data = db.HRJobSetUps.ToList();

            return View(data);
        }
        [HttpPost]
        public ActionResult SaveCasualLevelsData(HRJobSetUp data)
        {
            data.UserId = (int)Session["UserId"];
            data.DateAdded = DateTime.Now;
            data.BranchId = 1;


            db.HRJobSetUps.Add(data);
            db.SaveChanges();

            return RedirectToAction("CasualLevels");
        }
        #endregion
        #region Job Description
        public ActionResult Dept()
        {
            ViewBag.Departments = db.Departments.ToList();
            var departments = db.Departments.Where(e => e.DepartmentName != null).ToList();

            return View(departments);
        }

        public ActionResult JobDescription()
        {
            ViewBag.Departments = db.Departments.ToList();

            ViewBag.HRJobDesc = db.HRJobDescs.ToList();
            var data = db.HRJobDescs.ToList();

            return View(data);
        }
        [HttpPost]
        public ActionResult SaveJobDesriptionData(HRJobDesc data)
        {
            data.UserId = (int)Session["UserId"];
            data.DateAdded = DateTime.Now;
            data.BranchId = 1;


            db.HRJobDescs.Add(data);
            db.SaveChanges();

            return RedirectToAction("JobDescription");
        }
        #endregion
        #region Job Category
        public ActionResult JobCategory()
        {
            ViewBag.HRJobCategory = db.HRJobCategories.ToList();
            var data = db.HRJobCategories.ToList();

            return View(data);
        }
        [HttpPost]
        public ActionResult SaveJobCategoryData(HRJobCategory data)
        {
            data.UserId = (int)Session["UserId"];
            data.DateAdded = DateTime.Now;
            data.BranchId = 1;


            db.HRJobCategories.Add(data);
            db.SaveChanges();

            return RedirectToAction("JobCategory");
        }
        #endregion
        #region Job Designations
        public ActionResult Dep()
        {
            ViewBag.Departments = db.Departments.ToList();
            var departments = db.Departments.Where(e => e.DepartmentName != null).ToList();

            return View(departments);
        }

        public ActionResult JobDesignation()
        {
            ViewBag.Departments = db.Departments.ToList();

            ViewBag.HRJobDesignation = db.HRJobDesignations.ToList();
            var data = db.HRJobDesignations.ToList();

            return View(data);
        }
        [HttpPost]
        public ActionResult SaveJobDesignationData(HRJobDesignation data)
        {
            data.UserId = (int)Session["UserId"];
            data.DateAdded = DateTime.Now;
            data.BranchId = 1;


            db.HRJobDesignations.Add(data);
            db.SaveChanges();

            return RedirectToAction("JobDesignation");
        }
        #endregion
        #region Leave Types
        public ActionResult LeaveType()
        {
           
            ViewBag.HRLeaveType = db.HRLeaveTypes.ToList();
            var data = db.HRLeaveTypes.ToList();

            return View(data);
        }
        [HttpPost]
        public ActionResult SaveLeaveTypeData(HRLeaveType data)
        {
            data.UserId = (int)Session["UserId"];
            data.DateAdded = DateTime.Now;
            data.BranchId = 1;


            db.HRLeaveTypes.Add(data);
            db.SaveChanges();

            return RedirectToAction("LeaveType");
        }
        #endregion
        #region Job Functions
        public ActionResult JobFunction()
        {
            ViewBag.HRJobFunction = db.HRJobFunctions.ToList();
            var data = db.HRJobFunctions.ToList();

            return View(data);
        }
        [HttpPost]
        public ActionResult SaveJobFunctionData(HRJobFunction data)
        {
            data.UserId = (int)Session["UserId"];
            data.DateAdded = DateTime.Now;
            data.BranchId = 1;


            db.HRJobFunctions.Add(data);
            db.SaveChanges();

            return RedirectToAction("JobFunction");
        }
        #endregion
        #region Job Family
        public ActionResult JobFamily()
        {
            ViewBag.HRJobFamily = db.HRJobFamilies.ToList();
            var data = db.HRJobFamilies.ToList();

            return View(data);
        }
        [HttpPost]
        public ActionResult SaveJobFamilyData(HRJobFamily data)
        {
            data.UserId = (int)Session["UserId"];
            data.DateAdded = DateTime.Now;
            data.BranchId = 1;


            db.HRJobFamilies.Add(data);
            db.SaveChanges();

            return RedirectToAction("JobFamily");
        }
        #endregion
        #region JobStatus
        public ActionResult JobStatus()
        {
            ViewBag.HRJobStatus = db.HRJobStatus.ToList();
            var data = db.HRJobStatus.ToList();

            return View(data);
        }
        [HttpPost]
        public ActionResult SaveJobStatusData(HRJobStatu data)
        {
            data.UserId = (int)Session["UserId"];
            data.DateAdded = DateTime.Now;
            data.BranchId = 1;


            db.HRJobStatus.Add(data);
            db.SaveChanges();

            return RedirectToAction("JobStatus");
        }
        #endregion
        #region Job Requisition Types
        public ActionResult JobRequisition()
        {
            ViewBag.HRApplication = db.HRApplications.ToList();
            var data = db.HRApplications.ToList();

            return View(data);
        }
        [HttpPost]
        public ActionResult SaveJobRequisitionData(HRApplication data)
        {
            data.UserId = (int)Session["UserId"];
            data.DateAdded = DateTime.Now;
            data.BranchId = 1;


            db.HRApplications.Add(data);
            db.SaveChanges();

            return RedirectToAction("JobRequisition");
        }
        #endregion
        #region Interview Decisions
        public ActionResult InterviewDecisions()
        {
            ViewBag.HRApplication = db.HRApplications.ToList();
            var data = db.HRApplications.ToList();

            return View(data);
        }
        [HttpPost]
        public ActionResult SaveInterviewDecisionsData(HRApplication data)
        {
            data.UserId = (int)Session["UserId"];
            data.DateAdded = DateTime.Now;
            data.BranchId = 1;


            db.HRApplications.Add(data);
            db.SaveChanges();

            return RedirectToAction("InterviewDecisions");
        }
        #endregion
        #region Interview Types
        public ActionResult InterviewTypes()
        {
            ViewBag.HRApplication = db.HRApplications.ToList();
            var data = db.HRApplications.ToList();

            return View(data);
        }
        [HttpPost]
        public ActionResult SaveInterviewTypesData(HRApplication data)
        {
            data.UserId = (int)Session["UserId"];
            data.DateAdded = DateTime.Now;
            data.BranchId = 1;


            db.HRApplications.Add(data);
            db.SaveChanges();

            return RedirectToAction("InterviewTypes");
        }
        #endregion
        #region Interview Disposition Reasons
        public ActionResult InterviewDisposition()
        {
            ViewBag.HRApplication = db.HRApplications.ToList();
            var data = db.HRApplications.ToList();

            return View(data);
        }
        [HttpPost]
        public ActionResult SaveInterviewDispostionData(HRApplication data)
        {
            data.UserId = (int)Session["UserId"];
            data.DateAdded = DateTime.Now;
            data.BranchId = 1;


            db.HRApplications.Add(data);
            db.SaveChanges();

            return RedirectToAction("InterviewDisposition");
        }
        #endregion
        #region Application Sources
        public ActionResult ApplicationSources()
        {
            ViewBag.HRApplication = db.HRApplications.ToList();
            var data = db.HRApplications.ToList();

            return View(data);
        }
        [HttpPost]
        public ActionResult SaveApplicationSourcesData(HRApplication data)
        {
            data.UserId = (int)Session["UserId"];
            data.DateAdded = DateTime.Now;
            data.BranchId = 1;


            db.HRApplications.Add(data);
            db.SaveChanges();

            return RedirectToAction("ApplicationSources");
        }
        #endregion
        #region Union Termination
        public ActionResult UnionTermination()
        {
            ViewBag.HRUnion = db.HRUnions.ToList();
            var data = db.HRUnions.ToList();

            return View(data);
        }
        [HttpPost]
        public ActionResult SaveUnionTerminationData(HRUnion data)
        {
            data.UserId = (int)Session["UserId"];
            data.DateAdded = DateTime.Now;
            data.BranchId = 1;


            db.HRUnions.Add(data);
            db.SaveChanges();

            return RedirectToAction("UnionTermination");
        }
        #endregion
        #region Subscription Periodicity
        public ActionResult SubscriptionPeriodicity()
        {
            ViewBag.HRUnion = db.HRUnions.ToList();
            var data = db.HRUnions.ToList();

            return View(data);
        }
        [HttpPost]
        public ActionResult SaveSubscriptionPeriodicityData(HRUnion data)
        {
            data.UserId = (int)Session["UserId"];
            data.DateAdded = DateTime.Now;
            data.BranchId = 1;


            db.HRUnions.Add(data);
            db.SaveChanges();

            return RedirectToAction("SubscriptionPeriodicity");
        }
        #endregion
        #region Union Authority
        public ActionResult UnionAuthority()
        {
            ViewBag.HRUnion = db.HRUnions.ToList();
            var data = db.HRUnions.ToList();

            return View(data);
        }
        [HttpPost]
        public ActionResult SaveUnionAuthorityData(HRUnion data)
        {
            data.UserId = (int)Session["UserId"];
            data.DateAdded = DateTime.Now;
            data.BranchId = 1;


            db.HRUnions.Add(data);
            db.SaveChanges();

            return RedirectToAction("UnionAuthority");
        }
        #endregion
        #region Education Levels
        public ActionResult EducationLevels()
        {
            ViewBag.HRApplicantQualification = db.HRApplicantQualifications.ToList();
            var data = db.HRApplicantQualifications.ToList();

            return View(data);
        }
        [HttpPost]
        public ActionResult SaveEducationLevelsData(HRApplicantQualification data)
        {
            data.UserId = (int)Session["UserId"];
            data.DateAdded = DateTime.Now;
            data.BranchId = 1;


            db.HRApplicantQualifications.Add(data);
            db.SaveChanges();

            return RedirectToAction("EducationLevels");
        }
        #endregion
        #region ProfessionalQualification
        public ActionResult DeptName()
        {
            ViewBag.Departments = db.Departments.ToList();
            var departments = db.Departments.Where(e => e.DepartmentName != null).ToList();

            return View(departments);
        }
        public ActionResult ProfessionalQualification()
        {
            ViewBag.Departments = db.Departments.ToList();
            ViewBag.HRApplicantQualification = db.HRApplicantQualifications.ToList();
            var data = db.HRApplicantQualifications.ToList();

            return View(data);
        }
        [HttpPost]
        public ActionResult SaveProfessionalQualificationData(HRApplicantQualification data)
        {
            data.UserId = (int)Session["UserId"];
            data.DateAdded = DateTime.Now;
            data.BranchId = 1;


            db.HRApplicantQualifications.Add(data);
            db.SaveChanges();

            return RedirectToAction("ProfessionalQualification");
        }
        #endregion
        #region Professional Licencies
        public ActionResult DeptNa()
        {
            ViewBag.Departments = db.Departments.ToList();
            var departments = db.Departments.Where(e => e.DepartmentName != null).ToList();

            return View(departments);
        }
        public ActionResult ProfessionalLicencies()
        {
            ViewBag.Departments = db.Departments.ToList();
            ViewBag.HRApplicantQualification = db.HRApplicantQualifications.ToList();
            var data = db.HRApplicantQualifications.ToList();

            return View(data);
        }
        [HttpPost]
        public ActionResult SaveProfessionalLicenciesData(HRApplicantQualification data)
        {
            data.UserId = (int)Session["UserId"];
            data.DateAdded = DateTime.Now;
            data.BranchId = 1;


            db.HRApplicantQualifications.Add(data);
            db.SaveChanges();

            return RedirectToAction("ProfessionalLicencies");
        }
        #endregion
        #region Roles and Resposibilities
        public ActionResult Dp()
        {
            ViewBag.Departments = db.Departments.ToList();
            var departments = db.Departments.Where(e => e.DepartmentName != null).ToList();

            return View(departments);
        }
        public ActionResult RolesAndResposibilities()
        {
            ViewBag.Departments = db.Departments.ToList();
            ViewBag.HRRole = db.HRRoles.ToList();
            var data = db.HRRoles.ToList();

            return View(data);
        }
        [HttpPost]
        public ActionResult SaveRolesAndResposibilitiesData(HRRole data)
        {
            data.UserId = (int)Session["UserId"];
            data.DateAdded = DateTime.Now;
            data.BranchId = 1;


            db.HRRoles.Add(data);
            db.SaveChanges();

            return RedirectToAction("RolesAndResposibilities");
        }
        #endregion
        #region Tasks Setup
        public ActionResult TaskSetup()
        {
           
            ViewBag.HRRole = db.HRRoles.ToList();
            var data = db.HRRoles.ToList();

            return View(data);
        }
        [HttpPost]
        public ActionResult SaveTaskSetupData(HRRole data)
        {
            data.UserId = (int)Session["UserId"];
            data.DateAdded = DateTime.Now;
            data.BranchId = 1;


            db.HRRoles.Add(data);
            db.SaveChanges();

            return RedirectToAction("TaskSetup");
        }
        #endregion
        #region Management Hierachies
        public ActionResult ManagementHierachies()
        {

            ViewBag.HRManagement = db.HRManagements.ToList();
            var data = db.HRManagements.ToList();

            return View(data);
        }
        [HttpPost]
        public ActionResult SaveManagementHierachiesData(HRManagement data)
        {
            data.UserId = (int)Session["UserId"];
            data.DateAdded = DateTime.Now;
            data.BranchId = 1;


            db.HRManagements.Add(data);
            db.SaveChanges();

            return RedirectToAction("ManagementHierachies");
        }
        #endregion
        #region Authorities
        public ActionResult Authorities()
        {

            ViewBag.HRManagement = db.HRManagements.ToList();
            var data = db.HRManagements.ToList();

            return View(data);
        }
        [HttpPost]
        public ActionResult SaveAuthoritiesData(HRManagement data)
        {
            data.UserId = (int)Session["UserId"];
            data.DateAdded = DateTime.Now;
            data.BranchId = 1;


            db.HRManagements.Add(data);
            db.SaveChanges();

            return RedirectToAction("Authorities");
        }
        #endregion
        #region Management Levels
        public ActionResult ManagementLevels()
        {

            ViewBag.HRManagement = db.HRManagements.ToList();
            var data = db.HRManagements.ToList();

            return View(data);
        }
        [HttpPost]
        public ActionResult SaveManagementLevelsData(HRManagement data)
        {
            data.UserId = (int)Session["UserId"];
            data.DateAdded = DateTime.Now;
            data.BranchId = 1;


            db.HRManagements.Add(data);
            db.SaveChanges();

            return RedirectToAction("ManagementLevels");
        }
        #endregion
        #region Resource Access Control
        public ActionResult ResourceAccess()
        {

            ViewBag.HRResourseAccess = db.HRResourseAccesses.ToList();
            var data = db.HRResourseAccesses.ToList();

            return View(data);
        }
        [HttpPost]
        public ActionResult SaveResourceAccessData(HRResourseAccess data)
        {
            data.UserId = (int)Session["UserId"];
            data.DateAdded = DateTime.Now;
            data.BranchId = 1;


            db.HRResourseAccesses.Add(data);
            db.SaveChanges();

            return RedirectToAction("ResourceAccess");
        }
        #endregion
        #region SocialClubs
        public ActionResult SocialClubs()
        {

            ViewBag.HRExtraMural = db.HRExtraMurals.ToList();
            var data = db.HRExtraMurals.ToList();

            return View(data);
        }
        [HttpPost]
        public ActionResult SaveSocialClubsData(HRExtraMural data)
        {
            data.UserId = (int)Session["UserId"];
            data.DateAdded = DateTime.Now;
            data.BranchId = 1;


            db.HRExtraMurals.Add(data);
            db.SaveChanges();

            return RedirectToAction("SocialClubs");
        }
        #endregion
        #region Extra Curricular Activity
        public ActionResult ExtraCurricularActivity()
        {

            ViewBag.HRExtraMural = db.HRExtraMurals.ToList();
            var data = db.HRExtraMurals.ToList();

            return View(data);
        }
        [HttpPost]
        public ActionResult SaveExtraCurricularActivityData(HRExtraMural data)
        {
            data.UserId = (int)Session["UserId"];
            data.DateAdded = DateTime.Now;
            data.BranchId = 1;


            db.HRExtraMurals.Add(data);
            db.SaveChanges();

            return RedirectToAction("ExtraCurricularActivity");
        }
        #endregion
        #region Recreational Facilities
        public ActionResult RecreationalFacilities()
        {

            ViewBag.HRExtraMural = db.HRExtraMurals.ToList();
            var data = db.HRExtraMurals.ToList();

            return View(data);
        }
        [HttpPost]
        public ActionResult SaveRecreationalFacilitiesData(HRExtraMural data)
        {
            data.UserId = (int)Session["UserId"];
            data.DateAdded = DateTime.Now;
            data.BranchId = 1;


            db.HRExtraMurals.Add(data);
            db.SaveChanges();

            return RedirectToAction("RecreationalFacilities");
        }
        #endregion
        #region Hobbies
        public ActionResult Hobbies()
        {

            ViewBag.HRExtraMural = db.HRExtraMurals.ToList();
            var data = db.HRExtraMurals.ToList();

            return View(data);
        }
        [HttpPost]
        public ActionResult SaveHobbiesData(HRExtraMural data)
        {
            data.UserId = (int)Session["UserId"];
            data.DateAdded = DateTime.Now;
            data.BranchId = 1;


            db.HRExtraMurals.Add(data);
            db.SaveChanges();

            return RedirectToAction("Hobbies");
        }
        #endregion
        #region Sports
        public ActionResult Sports()
        {

            ViewBag.HRExtraMural = db.HRExtraMurals.ToList();
            var data = db.HRExtraMurals.ToList();

            return View(data);
        }
        [HttpPost]
        public ActionResult SaveSportsData(HRExtraMural data)
        {
            data.UserId = (int)Session["UserId"];
            data.DateAdded = DateTime.Now;
            data.BranchId = 1;


            db.HRExtraMurals.Add(data);
            db.SaveChanges();

            return RedirectToAction("Sports");
        }
        #endregion
        #region Languages
        public ActionResult Languages()
        {

            ViewBag.HRExtraMural = db.HRExtraMurals.ToList();
            var data = db.HRExtraMurals.ToList();

            return View(data);
        }
        [HttpPost]
        public ActionResult SaveLanguagesData(HRExtraMural data)
        {
            data.UserId = (int)Session["UserId"];
            data.DateAdded = DateTime.Now;
            data.BranchId = 1;


            db.HRExtraMurals.Add(data);
            db.SaveChanges();

            return RedirectToAction("Languages");
        }
        #endregion
        #region KPI Definitions
        public ActionResult KPI()
        {

            ViewBag.HRPerformanceVariable = db.HRPerformanceVariables.ToList();
            var data = db.HRExtraMurals.ToList();

            return View(data);
        }
        [HttpPost]
        public ActionResult SaveKPIData(HRPerformanceVariable data)
        {
            data.UserId = (int)Session["UserId"];
            data.DateAdded = DateTime.Now;
            data.BranchId = 1;


            db.HRPerformanceVariables.Add(data);
            db.SaveChanges();

            return RedirectToAction("KPI");
        }
        #endregion
        #region Quality Indicators
        public ActionResult QualityIndicators()
        {

            ViewBag.HRPerformanceVariable = db.HRPerformanceVariables.ToList();
            var data = db.HRExtraMurals.ToList();

            return View(data);
        }
        [HttpPost]
        public ActionResult SaveQualityIndicatorsData(HRPerformanceVariable data)
        {
            data.UserId = (int)Session["UserId"];
            data.DateAdded = DateTime.Now;
            data.BranchId = 1;


            db.HRPerformanceVariables.Add(data);
            db.SaveChanges();

            return RedirectToAction("QualityIndicators");
        }
        #endregion
        #region Grades and Merits
        public ActionResult GradesAndMerits()
        {

            ViewBag.HRPerformanceVariable = db.HRPerformanceVariables.ToList();
            var data = db.HRExtraMurals.ToList();

            return View(data);
        }
        [HttpPost]
        public ActionResult SaveGradesAndMeritsData(HRPerformanceVariable data)
        {
            data.UserId = (int)Session["UserId"];
            data.DateAdded = DateTime.Now;
            data.BranchId = 1;


            db.HRPerformanceVariables.Add(data);
            db.SaveChanges();

            return RedirectToAction("GradesAndMerits");
        }
        #endregion
        #region Memo Grading
        public ActionResult MemoGrading()
        {

            ViewBag.HRPerformanceVariable = db.HRPerformanceVariables.ToList();
            var data = db.HRExtraMurals.ToList();

            return View(data);
        }
        [HttpPost]
        public ActionResult SaveMemoGradingData(HRPerformanceVariable data)
        {
            data.UserId = (int)Session["UserId"];
            data.DateAdded = DateTime.Now;
            data.BranchId = 1;


            db.HRPerformanceVariables.Add(data);
            db.SaveChanges();

            return RedirectToAction("MemoGrading");
        }
        #endregion
        #region Incentives Setup
        public ActionResult DeptIncentives()
        {
            ViewBag.Departments = db.Departments.ToList();
            var departments = db.Departments.Where(e => e.DepartmentName != null).ToList();

            return View(departments);
        }
        public ActionResult Incentives()
        {
            ViewBag.Departments = db.Departments.ToList();
            ViewBag.HRPerformanceVariable = db.HRPerformanceVariables.ToList();
            var data = db.HRExtraMurals.ToList();

            return View(data);
        }
        [HttpPost]
        public ActionResult SaveIncentivesData(HRPerformanceVariable data)
        {
            data.UserId = (int)Session["UserId"];
            data.DateAdded = DateTime.Now;
            data.BranchId = 1;


            db.HRPerformanceVariables.Add(data);
            db.SaveChanges();

            return RedirectToAction("Incentives");
        }
        #endregion
        #region Retirement Benefits
        public ActionResult RetirementBenefits()
        {
           
            ViewBag.HRInsuranceCover = db.HRInsuranceCovers.ToList();
            var data = db.HRInsuranceCovers.ToList();

            return View(data);
        }
        [HttpPost]
        public ActionResult SaveRetirementBenefitsData(HRInsuranceCover data)
        {
            data.UserId = (int)Session["UserId"];
            data.DateAdded = DateTime.Now;
            data.BranchId = 1;


            db.HRInsuranceCovers.Add(data);
            db.SaveChanges();

            return RedirectToAction("RetirementBenefits");
        }
        #endregion
        #region Policy Covers
        public ActionResult PolicyCovers()
        {

            ViewBag.HRInsuranceCover = db.HRInsuranceCovers.ToList();
            var data = db.HRInsuranceCovers.ToList();

            return View(data);
        }
        [HttpPost]
        public ActionResult SavePolicyCoverData(HRInsuranceCover data)
        {
            data.UserId = (int)Session["UserId"];
            data.DateAdded = DateTime.Now;
            data.BranchId = 1;


            db.HRInsuranceCovers.Add(data);
            db.SaveChanges();

            return RedirectToAction(" PolicyCover");
        }
        #endregion
        #region Typs of Claims
        public ActionResult TypesOfClaims()
        {

            ViewBag.HRInsuranceCover = db.HRInsuranceCovers.ToList();
            var data = db.HRInsuranceCovers.ToList();

            return View(data);
        }
        [HttpPost]
        public ActionResult SaveTypesOfClaimsData(HRInsuranceCover data)
        {
            data.UserId = (int)Session["UserId"];
            data.DateAdded = DateTime.Now;
            data.BranchId = 1;


            db.HRInsuranceCovers.Add(data);
            db.SaveChanges();

            return RedirectToAction("TypesOfClaims");
        }
        #endregion
        #region Eligibility Status
        public ActionResult EligibilityStatus()
        {

            ViewBag.HRInsuranceCover = db.HRInsuranceCovers.ToList();
            var data = db.HRInsuranceCovers.ToList();

            return View(data);
        }
        [HttpPost]
        public ActionResult SaveEligibilityStatusData(HRInsuranceCover data)
        {
            data.UserId = (int)Session["UserId"];
            data.DateAdded = DateTime.Now;
            data.BranchId = 1;


            db.HRInsuranceCovers.Add(data);
            db.SaveChanges();

            return RedirectToAction("EligibilityStatus");
        }
        #endregion
        #region Staff Accessing Policy
        public ActionResult AccessingPolicy()
        {

            ViewBag.HRInsuranceCover = db.HRInsuranceCovers.ToList();
            var data = db.HRInsuranceCovers.ToList();

            return View(data);
        }
        [HttpPost]
        public ActionResult SaveAccessingPolicyData(HRInsuranceCover data)
        {
            data.UserId = (int)Session["UserId"];
            data.DateAdded = DateTime.Now;
            data.BranchId = 1;


            db.HRInsuranceCovers.Add(data);
            db.SaveChanges();

            return RedirectToAction("AccessingPolicy");
        }
        #endregion
        #region Trust Fund
        public ActionResult TrustFund()
        {

            ViewBag.HRInsuranceCover = db.HRInsuranceCovers.ToList();
            var data = db.HRInsuranceCovers.ToList();

            return View(data);
        }
        [HttpPost]
        public ActionResult SaveTrustFundData(HRInsuranceCover data)
        {
            data.UserId = (int)Session["UserId"];
            data.DateAdded = DateTime.Now;
            data.BranchId = 1;


            db.HRInsuranceCovers.Add(data);
            db.SaveChanges();

            return RedirectToAction("TrustFund");
        }
        #endregion
        #region Policy Brokers
        public ActionResult PolicyBrokers()
        {

            ViewBag.HRInsuranceCover = db.HRInsuranceCovers.ToList();
            var data = db.HRInsuranceCovers.ToList();

            return View(data);
        }
        [HttpPost]
        public ActionResult SavePolicyBrokersData(HRInsuranceCover data)
        {
            data.UserId = (int)Session["UserId"];
            data.DateAdded = DateTime.Now;
            data.BranchId = 1;


            db.HRInsuranceCovers.Add(data);
            db.SaveChanges();

            return RedirectToAction("PolicyBrokers");
        }
        #endregion
        #region Employment Benefits
        public ActionResult DeptEmployment()
        {
            ViewBag.Departments = db.Departments.ToList();
            var departments = db.Departments.Where(e => e.DepartmentName != null).ToList();

            return View(departments);
        }
        public ActionResult EmploymentBenefits()
        {
            ViewBag.Departments = db.Departments.ToList();
            ViewBag.HREmploymentBenefit = db.HREmploymentBenefits.ToList();
            var data = db.HRInsuranceCovers.ToList();

            return View(data);
        }
        [HttpPost]
        public ActionResult SaveEmploymentBenefitsData(HREmploymentBenefit data)
        {
            data.UserId = (int)Session["UserId"];
            data.DateAdded = DateTime.Now;
            data.BranchId = 1;


            db.HREmploymentBenefits.Add(data);
            db.SaveChanges();

            return RedirectToAction("EmploymentBenefits");
        }
        #endregion
        #region Savings and Saccos
        public ActionResult SavingsAndSaccos()
        {
           
            ViewBag.HRSaccoAndStaffLoan = db.HRSaccoAndStaffLoans.ToList();
            var data = db.HRSaccoAndStaffLoans.ToList();

            return View(data);
        }
        [HttpPost]
        public ActionResult SaveSavingsAndSaccosData(HRSaccoAndStaffLoan data)
        {
            data.UserId = (int)Session["UserId"];
            data.DateAdded = DateTime.Now;
            data.BranchId = 1;


            db.HRSaccoAndStaffLoans.Add(data);
            db.SaveChanges();

            return RedirectToAction("SavingsAndSaccos");
        }
        #endregion
        #region Staff Loan Type
        public ActionResult StaffLoanType()
        {

            ViewBag.HRSaccoAndStaffLoan = db.HRSaccoAndStaffLoans.ToList();
            var data = db.HRSaccoAndStaffLoans.ToList();

            return View(data);
        }
        [HttpPost]
        public ActionResult SaveStaffLoanTypeData(HRSaccoAndStaffLoan data)
        {
            data.UserId = (int)Session["UserId"];
            data.DateAdded = DateTime.Now;
            data.BranchId = 1;


            db.HRSaccoAndStaffLoans.Add(data);
            db.SaveChanges();

            return RedirectToAction("StaffLoanType");
        }
        #endregion
        #region Loan Payment Mode
        public ActionResult PaymentMode()
        {

            ViewBag.HRSaccoAndStaffLoan = db.HRSaccoAndStaffLoans.ToList();
            var data = db.HRSaccoAndStaffLoans.ToList();

            return View(data);
        }
        [HttpPost]
        public ActionResult SavePaymentModeData(HRSaccoAndStaffLoan data)
        {
            data.UserId = (int)Session["UserId"];
            data.DateAdded = DateTime.Now;
            data.BranchId = 1;


            db.HRSaccoAndStaffLoans.Add(data);
            db.SaveChanges();

            return RedirectToAction("PaymentMode");
        }
        #endregion
        #region Loan Repayment Mode
        public ActionResult RepaymentMode()
        {

            ViewBag.HRSaccoAndStaffLoan = db.HRSaccoAndStaffLoans.ToList();
            var data = db.HRSaccoAndStaffLoans.ToList();

            return View(data);
        }
        [HttpPost]
        public ActionResult SaveRepaymentModeData(HRSaccoAndStaffLoan data)
        {
            data.UserId = (int)Session["UserId"];
            data.DateAdded = DateTime.Now;
            data.BranchId = 1;


            db.HRSaccoAndStaffLoans.Add(data);
            db.SaveChanges();

            return RedirectToAction("RepaymentMode");
        }
        #endregion
        #region Guarantors
        public ActionResult Guarantors()
        {

            ViewBag.HRSaccoAndStaffLoan = db.HRSaccoAndStaffLoans.ToList();
            var data = db.HRSaccoAndStaffLoans.ToList();

            return View(data);
        }
        [HttpPost]
        public ActionResult SaveGuarantorsData(HRSaccoAndStaffLoan data)
        {
            data.UserId = (int)Session["UserId"];
            data.DateAdded = DateTime.Now;
            data.BranchId = 1;


            db.HRSaccoAndStaffLoans.Add(data);
            db.SaveChanges();

            return RedirectToAction("Guarantors");
        }
        #endregion
        #region User Profile
        public ActionResult UserDep()
        {
            ViewBag.Departments = db.Departments.ToList();
            var departments = db.Departments.Where(e => e.DepartmentName != null).ToList();

            return View(departments);
        }
        public ActionResult UserProfile()
        {
            ViewBag.Departments = db.Departments.ToList();
            ViewBag.HRSystemProcedure = db.HRSystemProcedures.ToList();
            var data = db.HRSystemProcedures.ToList();

            return View(data);
        }
        [HttpPost]
        public ActionResult SaveUserProfileData(HRSystemProcedure data)
        {
            data.UserId = (int)Session["UserId"];
            data.DateAdded = DateTime.Now;
            data.BranchId = 1;


            db.HRSystemProcedures.Add(data);
            db.SaveChanges();

            return RedirectToAction("UserProfile");
        }
        #endregion
        #region Employee Category
        public ActionResult EmployeeCategory()
        {
            
            ViewBag.HREmployeeSetup = db.HREmployeeSetups.ToList();
            var data = db.HREmployeeSetups.ToList();

            return View(data);
        }
        [HttpPost]
        public ActionResult SaveEmployeeCategoryData(HREmployeeSetup data)
        {
            data.UserId = (int)Session["UserId"];
            data.DateAdded = DateTime.Now;
            data.BranchId = 1;


            db.HREmployeeSetups.Add(data);
            db.SaveChanges();

            return RedirectToAction("EmployeeCategory");
        }
        #endregion
        #region Employee Status
        public ActionResult EmployeeStatus()
        {
           
            ViewBag.HREmployeeSetup = db.HREmployeeSetups.ToList();
            var data = db.HREmployeeSetups.ToList();

            return View(data);
        }
        [HttpPost]
        public ActionResult SaveEmployeeStatusData(HREmployeeSetup data)
        {
            data.UserId = (int)Session["UserId"];
            data.DateAdded = DateTime.Now;
            data.BranchId = 1;


            db.HREmployeeSetups.Add(data);
            db.SaveChanges();

            return RedirectToAction("EmployeeStatus");
        }
        #endregion
        #region Displinary Action Reason
        public ActionResult DisplinaryReason()
        {

            ViewBag.HRDisplinaryAction = db.HRDisplinaryActions.ToList();
            var data = db.HRDisplinaryActions.ToList();

            return View(data);
        }
        [HttpPost]
        public ActionResult SaveDisplinaryReasonData(HRDisplinaryAction data)
        {
            data.UserId = (int)Session["UserId"];
            data.DateAdded = DateTime.Now;
            data.BranchId = 1;


            db.HRDisplinaryActions.Add(data);
            db.SaveChanges();

            return RedirectToAction("DisplinaryReason");
        }
        #endregion
        #region Types of Action
        public ActionResult TypesOfAction()
        {

            ViewBag.HRDisplinaryAction = db.HRDisplinaryActions.ToList();
            var data = db.HRDisplinaryActions.ToList();

            return View(data);
        }
        [HttpPost]
        public ActionResult SaveTypesOfActionData(HRDisplinaryAction data)
        {
            data.UserId = (int)Session["UserId"];
            data.DateAdded = DateTime.Now;
            data.BranchId = 1;


            db.HRDisplinaryActions.Add(data);
            db.SaveChanges();

            return RedirectToAction("TypesOfAction");
        }
        #endregion
        #region Safety Equipments
        public ActionResult SafetyEquipments()
        {

            ViewBag.HRHealthAndSafety = db.HRHealthAndSafeties.ToList();
            var data = db.HRHealthAndSafeties.ToList();

            return View(data);
        }
        [HttpPost]
        public ActionResult SaveSafetyEquipmentsData(HRHealthAndSafety data)
        {
            data.UserId = (int)Session["UserId"];
            data.DateAdded = DateTime.Now;
            data.BranchId = 1;


            db.HRHealthAndSafeties.Add(data);
            db.SaveChanges();

            return RedirectToAction("SafetyEquipments");
        }
        #endregion
        #region Medical Tests
        public ActionResult MedicalTests()
        {

            ViewBag.HRHealthAndSafety = db.HRHealthAndSafeties.ToList();
            var data = db.HRHealthAndSafeties.ToList();

            return View(data);
        }
        [HttpPost]
        public ActionResult SaveMedicalTestsData(HRHealthAndSafety data)
        {
            data.UserId = (int)Session["UserId"];
            data.DateAdded = DateTime.Now;
            data.BranchId = 1;


            db.HRHealthAndSafeties.Add(data);
            db.SaveChanges();

            return RedirectToAction("MedicalTests");
        }
        #endregion
        #region Incident Types
        public ActionResult IncidentTypes()
        {

            ViewBag.HRHealthAndSafety = db.HRHealthAndSafeties.ToList();
            var data = db.HRHealthAndSafeties.ToList();

            return View(data);
        }
        [HttpPost]
        public ActionResult SaveIncidentTypesData(HRHealthAndSafety data)
        {
            data.UserId = (int)Session["UserId"];
            data.DateAdded = DateTime.Now;
            data.BranchId = 1;


            db.HRHealthAndSafeties.Add(data);
            db.SaveChanges();

            return RedirectToAction("IncidentTypes");
        }
        #endregion
        #region Job Requisition Information
        public ActionResult JRDepartment()
        {
            ViewBag.Departments = db.Departments.ToList();
            var departments = db.Departments.ToList();

            return View(departments);
        }
        public ActionResult JobRequisitionInfor()
        {
            ViewBag.Departments = db.Departments.ToList();
            ViewBag.HRJobRequisition = db.HRJobRequisitions.ToList();
            var data = db.HRJobRequisitions.ToList();

            return View(data);
        }
        [HttpPost]
        public ActionResult SaveJobRequisitionInforData(HRJobRequisition data)
        {
            data.UserId = (int)Session["UserId"];
            data.DateAdded = DateTime.Now;
            data.BranchId = 1;


            db.HRJobRequisitions.Add(data);
            db.SaveChanges();

            return RedirectToAction("JobRequisitionInfor");
        }
        #endregion
        #region Applicant Resume Data Base
        public ActionResult ApplicantResume()
        {
            
            ViewBag.HRFileInformation = db.HRFileInformations.ToList();
            var data = db.HRFileInformations.ToList();

            return View(data);
        }
        [HttpPost]
        public ActionResult SaveApplicantResumeData(HRFileInformation data)
        {
            data.UserId = (int)Session["UserId"];
            data.DateAdded = DateTime.Now;
            data.BranchId = 1;


            db.HRFileInformations.Add(data);
            db.SaveChanges();

            return RedirectToAction("ApplicantResume");
        }
        #endregion
        #region Bio Data
        public ActionResult BioData()
        {

            ViewBag.HRBioData = db.HRBioDatas.ToList();
            var data = db.HRBioDatas.ToList();

            return View(data);
        }
        [HttpPost]
        public ActionResult SaveBioData(HRBioData data)
        {
            data.UserId = (int)Session["UserId"];
            data.DateAdded = DateTime.Now;
            data.BranchId = 1;


            db.HRBioDatas.Add(data);
            db.SaveChanges();

            return RedirectToAction("BioData");
        }
        #endregion
        #region Qualifications
        public ActionResult Qualifications()
        {

            ViewBag.HRQualification = db.HRQualifications.ToList();
            var data = db.HRQualifications.ToList();

            return View(data);
        }
        [HttpPost]
        public ActionResult SaveQualificationsData(HRQualification data)
        {
            data.UserId = (int)Session["UserId"];
            data.DateAdded = DateTime.Now;
            data.BranchId = 1;


            db.HRQualifications.Add(data);
            db.SaveChanges();

            return RedirectToAction("Qualifications");
        }
        #endregion
        #region Work Experience
        public ActionResult WorkExperience()
        {

            ViewBag.HRWorkExperience = db.HRWorkExperiences.ToList();
            var data = db.HRWorkExperiences.ToList();

            return View(data);
        }
        [HttpPost]
        public ActionResult SaveWorkExperienceData(HRWorkExperience data)
        {
            data.UserId = (int)Session["UserId"];
            data.DateAdded = DateTime.Now;
            data.BranchId = 1;


            db.HRWorkExperiences.Add(data);
            db.SaveChanges();

            return RedirectToAction("WorkExperience");
        }
        #endregion
        #region Extra Curricular
        #endregion
        #region Refereees
        public ActionResult Refereees()
        {

            ViewBag.HRReferee = db.HRReferees.ToList();
            var data = db.HRReferees.ToList();

            return View(data);
        }
        [HttpPost]
        public ActionResult SaveRefereeesData(HRReferee data)
        {
            data.UserId = (int)Session["UserId"];
            data.DateAdded = DateTime.Now;
            data.BranchId = 1;


            db.HRReferees.Add(data);
            db.SaveChanges();

            return RedirectToAction("Refereees");
        }
        #endregion
        #region Application Form
        public ActionResult ApplicationForm()
        {

            ViewBag.HRApplicationForm = db.HRApplicationForms.ToList();
            var data = db.HRApplicationForms.ToList();

            return View(data);
        }
        [HttpPost]
        public ActionResult SaveApplicationFormData(HRApplicationForm data)
        {
            data.UserId = (int)Session["UserId"];
            data.DateAdded = DateTime.Now;
            data.BranchId = 1;


            db.HRApplicationForms.Add(data);
            db.SaveChanges();

            return RedirectToAction("ApplicationForm");
        }
        #endregion
        #region Interview Setup
        public ActionResult DeptInt()
        {
            ViewBag.Departments = db.Departments.ToList();
            var departments = db.Departments.Where(e => e.DepartmentName != null).ToList();

            return View(departments);
        }
        public ActionResult InterviewDetails()
        {
            ViewBag.Departments = db.Departments.ToList();
            ViewBag.HRInterviewSetUp = db.HRInterviewSetUps.ToList();
            var data = db.HRInterviewSetUps.ToList();

            return View(data);
        }
        [HttpPost]
        public ActionResult SaveInterviewDetailsData(HRInterviewSetUp data)
        {
            data.UserId = (int)Session["UserId"];
            data.DateAdded = DateTime.Now;
            data.BranchId = 1;


            db.HRInterviewSetUps.Add(data);
            db.SaveChanges();

            return RedirectToAction("InterviewDetails");
        }
        #endregion
        #region Confirmations
        public ActionResult Confirmation()
        {

            ViewBag.HRConfirmation = db.HRConfirmations.ToList();
            var data = db.HRConfirmations.ToList();

            return View(data);
        }
        [HttpPost]
        public ActionResult SaveConfirmationData(HRConfirmation data)
        {
            data.UserId = (int)Session["UserId"];
            data.DateAdded = DateTime.Now;
            data.BranchId = 1;


            db.HRConfirmations.Add(data);
            db.SaveChanges();

            return RedirectToAction("Confirmation");
        }
        #endregion
        #region Vetting
        public ActionResult Vetting()
        {

            ViewBag.HRRecruitmentProcessing = db.HRRecruitmentProcessings.ToList();
            var data = db.HRRecruitmentProcessings.ToList();

            return View(data);
        }
        [HttpPost]
        public ActionResult SaveVettingData(HRRecruitmentProcessing data)
        {
            data.UserId = (int)Session["UserId"];
            data.DateAdded = DateTime.Now;
            data.BranchId = 1;


            db.HRRecruitmentProcessings.Add(data);
            db.SaveChanges();

            return RedirectToAction("Vetting");
        }
        #endregion
        #region Tests
        public ActionResult Tests()
        {

            ViewBag.HRTest = db.HRTests.ToList();
            var data = db.HRTests.ToList();

            return View(data);
        }
        [HttpPost]
        public ActionResult SaveTestsData(HRTest data)
        {
            data.UserId = (int)Session["UserId"];
            data.DateAdded = DateTime.Now;
            data.BranchId = 1;


            db.HRTests.Add(data);
            db.SaveChanges();

            return RedirectToAction("Tests");
        }
        #endregion
        #region Staff Work Experience
        public ActionResult StaffWorkExperience()
        {

            ViewBag.HRStaffWorkExperience = db.HRStaffWorkExperiences.ToList();
            var data = db.HRStaffWorkExperiences.ToList();

            return View(data);
        }
        [HttpPost]
        public ActionResult SaveStaffWorkExperienceData(HRStaffWorkExperience data)
        {
            data.UserId = (int)Session["UserId"];
            data.DateAdded = DateTime.Now;
            data.BranchId = 1;


            db.HRStaffWorkExperiences.Add(data);
            db.SaveChanges();

            return RedirectToAction("StaffWorkExperience");
        }
        #endregion
        #region File Details
        public ActionResult DeptFine()
        {
            ViewBag.Departments = db.Departments.ToList();
            var departments = db.Departments.Where(e => e.DepartmentName != null).ToList();

            return View(departments);
        }
        public ActionResult FileDetails()
        {
            ViewBag.Departments = db.Departments.ToList();
            ViewBag.HRFileDetail = db.HRFileDetails.ToList();
            var data = db.HRFileDetails.ToList();

            return View(data);
        }
        [HttpPost]
        public ActionResult SaveFileDetailsData(HRFileDetail data)
        {
            data.UserId = (int)Session["UserId"];
            data.DateAdded = DateTime.Now;
            data.BranchId = 1;


            db.HRFileDetails.Add(data);
            db.SaveChanges();

            return RedirectToAction("FileDetails");
        }
        #endregion
        #region Staff Bio Data
        public ActionResult StaffBioData()
        {

            ViewBag.HRStaffBioData = db.HRStaffBioDatas.ToList();
            var data = db.HRStaffBioDatas.ToList();

            return View(data);
        }
        [HttpPost]
        public ActionResult SaveStaffBioData(HRStaffBioData data)
        {
            data.UserId = (int)Session["UserId"];
            data.DateAdded = DateTime.Now;
            data.BranchId = 1;


            db.HRStaffBioDatas.Add(data);
            db.SaveChanges();

            return RedirectToAction("StaffBioData");
        }
        #endregion
        #region Educational Quaifications
        public ActionResult DeptEQ()
        {
            ViewBag.Departments = db.Departments.ToList();
            var departments = db.Departments.Where(e => e.DepartmentName != null).ToList();

            return View(departments);
        }
        public ActionResult EducationalQuaifications()
        {
            ViewBag.Departments = db.Departments.ToList();
            ViewBag.HRStaffEducationQualification = db.HRStaffEducationQualifications.ToList();
            var data = db.HRStaffEducationQualifications.ToList();

            return View(data);
        }
        [HttpPost]
        public ActionResult SaveEducationalQuaificationsData(HRStaffEducationQualification data)
        {
            data.UserId = (int)Session["UserId"];
            data.DateAdded = DateTime.Now;
            data.BranchId = 1;


            db.HRStaffEducationQualifications.Add(data);
            db.SaveChanges();

            return RedirectToAction("EducationalQuaifications");
        }
        #endregion
        #region Professional Qualifications
        public ActionResult ProfessionalQuaifications()
        {
           
            ViewBag.HRStaffProfessionalQualification = db.HRStaffProfessionalQualifications.ToList();
            var data = db.HRStaffProfessionalQualifications.ToList();

            return View(data);
        }
        [HttpPost]
        public ActionResult SaveProfessionalQuaificationsData(HRStaffProfessionalQualification data)
        {
            data.UserId = (int)Session["UserId"];
            data.DateAdded = DateTime.Now;
            data.BranchId = 1;


            db.HRStaffProfessionalQualifications.Add(data);
            db.SaveChanges();

            return RedirectToAction("ProfessionalQuaifications");
        }
        #endregion
        #region Staff Referees
        public ActionResult StaffReferees()
        {

            ViewBag.HRStaffReferee = db.HRStaffReferees.ToList();
            var data = db.HRStaffReferees.ToList();

            return View(data);
        }
        [HttpPost]
        public ActionResult SaveStaffRefereesData(HRStaffReferee data)
        {
            data.UserId = (int)Session["UserId"];
            data.DateAdded = DateTime.Now;
            data.BranchId = 1;


            db.HRStaffReferees.Add(data);
            db.SaveChanges();

            return RedirectToAction("StaffReferees");
        }
        #endregion
        #region Contract Management
        public ActionResult DepContract()
        {
            ViewBag.Departments = db.Departments.ToList();
            var departments = db.Departments.Where(e => e.DepartmentName != null).ToList();

            return View(departments);
        }

        public ActionResult ContractManagement()
        {
            ViewBag.Departments = db.Departments.ToList();

            ViewBag.HRContractManagement=db.HRContractManagements.ToList();
            var data = db.HRContractManagements.ToList();

            return View(data);
        }
        [HttpPost]
        public ActionResult SaveContractManagementData(HRContractManagement data)
        {
            data.UserId = (int)Session["UserId"];
            data.DateAdded = DateTime.Now;
            data.BranchId = 1;


            db.HRContractManagements.Add(data);
            db.SaveChanges();

            return RedirectToAction("ContractManagement");
        }
        #endregion
        #region Allocation Of Roles and Responsibities
        public ActionResult DepRoles()
        {
            ViewBag.Departments = db.Departments.ToList();
            var departments = db.Departments.Where(e => e.DepartmentName != null).ToList();

            return View(departments);
        }

        public ActionResult AllocationOfRoles()
        {
            ViewBag.Departments = db.Departments.ToList();

            ViewBag.HRAllocationOfRole = db.HRAllocationOfRoles.ToList();
            var data = db.HRAllocationOfRoles.ToList();

            return View(data);
        }
        [HttpPost]
        public ActionResult SaveAllocationOfRolesData(HRAllocationOfRole data)
        {
            data.UserId = (int)Session["UserId"];
            data.DateAdded = DateTime.Now;
            data.BranchId = 1;


            db.HRAllocationOfRoles.Add(data);
            db.SaveChanges();

            return RedirectToAction(" AllocationOfRoles");
        }
        #endregion
        #region Appraisals Roles And Objectives
        public ActionResult DepARoles()
        {
            ViewBag.Departments = db.Departments.ToList();
            var departments = db.Departments.Where(e => e.DepartmentName != null).ToList();

            return View(departments);
        }

        public ActionResult RolesAndObjectives()
        {
            ViewBag.Departments = db.Departments.ToList();

            ViewBag.HRAppraisalRoleAndObjectives = db.HRAppraisalRoleAndObjectives.ToList();
            var data = db.HRAppraisalRoleAndObjectives.ToList();

            return View(data);
        }
        [HttpPost]
        public ActionResult SaveRolesAndObjectivesData(HRAppraisalRoleAndObjective data)
        {
            data.UserId = (int)Session["UserId"];
            data.DateAdded = DateTime.Now;
            data.BranchId = 1;


            db.HRAppraisalRoleAndObjectives.Add(data);
            db.SaveChanges();

            return RedirectToAction("OverallGrading");
        }
        #endregion
        #region Appraisal Overall Grading
        public ActionResult DepOverall()
        {
            ViewBag.Departments = db.Departments.ToList();
            var departments = db.Departments.Where(e => e.DepartmentName != null).ToList();

            return View(departments);
        }

        public ActionResult OverallGrading()
        {
            ViewBag.Departments = db.Departments.ToList();

            ViewBag.HRAppraisalOverrallGrading = db.HRAppraisalOverrallGradings.ToList();
            var data = db.HRAppraisalOverrallGradings.ToList();

            return View(data);
        }
        [HttpPost]
        public ActionResult SaveHRAppraisalOverrallGradingData(HRAppraisalOverrallGrading data)
        {
            data.UserId = (int)Session["UserId"];
            data.DateAdded = DateTime.Now;
            data.BranchId = 1;


            db.HRAppraisalOverrallGradings.Add(data);
            db.SaveChanges();

            return RedirectToAction("OverallGrading");
        }
        #endregion
        #region Tasks and Administration
        public ActionResult DepTasks()
        {
            ViewBag.Departments = db.Departments.ToList();
            var departments = db.Departments.Where(e => e.DepartmentName != null).ToList();

            return View(departments);
        }

        public ActionResult Tasks()
        {
            ViewBag.Departments = db.Departments.ToList();

            ViewBag.HRTaskAndAdministration = db.HRTaskAndAdministrations.ToList();
            var data = db.HRTaskAndAdministrations.ToList();

            return View(data);
        }
        [HttpPost]
        public ActionResult SaveTasksData(HRTaskAndAdministration data)
        {
            data.UserId = (int)Session["UserId"];
            data.DateAdded = DateTime.Now;
            data.BranchId = 1;


            db.HRTaskAndAdministrations.Add(data);
            db.SaveChanges();

            return RedirectToAction("Tasks");
        }
        #endregion
        #region Attendance sheet
        public ActionResult DepAttendance()
        {
            ViewBag.Departments = db.Departments.ToList();
            var departments = db.Departments.Where(e => e.DepartmentName != null).ToList();

            return View(departments);
        }

        public ActionResult Attendance()
        {
            ViewBag.Departments = db.Departments.ToList();

            ViewBag.HRAttendanceSheet = db.HRAttendanceSheets.ToList();
            var data = db.HRAttendanceSheets.ToList();

            return View(data);
        }
        [HttpPost]
        public ActionResult SaveAttendanceData(HRAttendanceSheet data)
        {
            data.UserId = (int)Session["UserId"];
            data.DateAdded = DateTime.Now;
            data.BranchId = 1;


            db.HRAttendanceSheets.Add(data);
            db.SaveChanges();

            return RedirectToAction("Absentism");
        }
        #endregion
        #region Absentism
        public ActionResult DepAbsentism()
        {
            ViewBag.Departments = db.Departments.ToList();
            var departments = db.Departments.Where(e => e.DepartmentName != null).ToList();

            return View(departments);
        }

        public ActionResult Absentism()
        {
            ViewBag.Departments = db.Departments.ToList();

            ViewBag.HRAbsentism = db.HRAbsentism.ToList();
            var data = db.HRAbsentism.ToList();

            return View(data);
        }
        [HttpPost]
        public ActionResult SaveAbsentismData(HRAbsentism data)
        {
            data.UserId = (int)Session["UserId"];
            data.DateAdded = DateTime.Now;
            data.BranchId = 1;


            db.HRAbsentism.Add(data);
            db.SaveChanges();

            return RedirectToAction("Absentism");
        }
        #endregion
        #region Union Details
        public ActionResult UnionDetails()
        {
           
            ViewBag.HRUnionDetail = db.HRUnionDetails.ToList();
            var data = db.HRUnionDetails.ToList();

            return View(data);
        }
        [HttpPost]
        public ActionResult SaveUnionDetailsData(HRUnionDetail data)
        {
            data.UserId = (int)Session["UserId"];
            data.DateAdded = DateTime.Now;
            data.BranchId = 1;


            db.HRUnionDetails.Add(data);
            db.SaveChanges();

            return RedirectToAction("UnionDetails");
        }
        #endregion
        #region Union Registration
        public ActionResult UnionRegistration()
        {

            ViewBag.HRUnionRegistration = db.HRUnionRegistrations.ToList();
            var data = db.HRUnionRegistrations.ToList();

            return View(data);
        }
        [HttpPost]
        public ActionResult SaveUnionRegistrationData(HRUnionRegistration data)
        {
            data.UserId = (int)Session["UserId"];
            data.DateAdded = DateTime.Now;
            data.BranchId = 1;


            db.HRUnionRegistrations.Add(data);
            db.SaveChanges();

            return RedirectToAction("UnionDeregistration");
        }
        #endregion
        #region Union Deregistration
        public ActionResult UnionDeregistration()
        {

            ViewBag.HRUnionDeregistration = db.HRUnionDeregistrations.ToList();
            var data = db.HRUnionDeregistrations.ToList();

            return View(data);
        }
        [HttpPost]
        public ActionResult SaveUnionDeregistrationData(HRUnionDeregistration data)
        {
            data.UserId = (int)Session["UserId"];
            data.DateAdded = DateTime.Now;
            data.BranchId = 1;


            db.HRUnionDeregistrations.Add(data);
            db.SaveChanges();

            return RedirectToAction("UnionDeregistration");
        }
        #endregion
        #region Memos
        public ActionResult Memos()
        {

            ViewBag.HRMemo = db.HRMemos.ToList();
            var data = db.HRMemos.ToList();

            return View(data);
        }
        [HttpPost]
        public ActionResult SaveMemosData(HRMemo data)
        {
            data.UserId = (int)Session["UserId"];
            data.DateAdded = DateTime.Now;
            data.BranchId = 1;


            db.HRMemos.Add(data);
            db.SaveChanges();

            return RedirectToAction("Memos");
        }
        #endregion
        #region Minutes Of Meetings
        public ActionResult Minutes()
        {

            ViewBag.HRMinutesOfMeeting = db.HRMinutesOfMeetings.ToList();
            var data = db.HRMinutesOfMeetings.ToList();

            return View(data);
        }
        [HttpPost]
        public ActionResult SaveMinutesData(HRMinutesOfMeeting data)
        {
            data.UserId = (int)Session["UserId"];
            data.DateAdded = DateTime.Now;
            data.BranchId = 1;


            db.HRMinutesOfMeetings.Add(data);
            db.SaveChanges();

            return RedirectToAction("Minutes");
        }
        #endregion
        #region Postings
        public ActionResult Postings()
        {

            ViewBag.HRPosting = db.HRPostings.ToList();
            var data = db.HRPostings.ToList();

            return View(data);
        }
        [HttpPost]
        public ActionResult SavePostingsData(HRPosting data)
        {
            data.UserId = (int)Session["UserId"];
            data.DateAdded = DateTime.Now;
            data.BranchId = 1;


            db.HRPostings.Add(data);
            db.SaveChanges();

            return RedirectToAction("Postings");
        }
        #endregion
        #region Promotion
        public ActionResult DeptPromo()
        {
           
            var departments = db.Departments.Where(e => e.DepartmentName != null).ToList();

            return View(departments);
        }
        public ActionResult Promotion()
        {
            ViewBag.Departments = db.Departments.ToList();
            ViewBag.HRPromotion = db.HRPromotions.ToList();
            var data = db.HRPromotions.ToList();

            return View(data);
        }
        [HttpPost]
        public ActionResult SavePromotionData(HRPromotion data)
        {
            data.UserId = (int)Session["UserId"];
            data.DateAdded = DateTime.Now;
            data.BranchId = 1;


            db.HRPromotions.Add(data);
            db.SaveChanges();

            return RedirectToAction("Promotion");
        }
        #endregion
        #region Demotion
        public ActionResult DeptDemo()
        {
           
            var departments = db.Departments.Where(e => e.DepartmentName != null).ToList();

            return View(departments);
        }
        public ActionResult Demotion()
        {
            ViewBag.Departments = db.Departments.ToList();
            ViewBag.HRDemotion = db.HRDemotions.ToList();
            var data = db.HRDemotions.ToList();

            return View(data);

        }
        [HttpPost]
        public ActionResult SaveDemotionData(HRDemotion data)
        {
            data.UserId = (int)Session["UserId"];
            data.DateAdded = DateTime.Now;
            data.BranchId = 1;


            db.HRDemotions.Add(data);
            db.SaveChanges();

            return RedirectToAction("Demotion");
        }
        #endregion
        #region Resource Access Control
        public ActionResult DeptRAC()
        {

            var departments = db.Departments.Where(e => e.DepartmentName != null).ToList();

            return View(departments);
        }
        public ActionResult RemoteAccess()
        {
            ViewBag.Departments = db.Departments.ToList();
            ViewBag.HRResourceAccessControl = db.HRResourceAccessControls.ToList();
            var data = db.HRResourceAccessControls.ToList();

            return View(data);

        }
        [HttpPost]
        public ActionResult SaveRemoteAccessData(HRResourceAccessControl data)
        {
            data.UserId = (int)Session["UserId"];
            data.DateAdded = DateTime.Now;
            data.BranchId = 1;


            db.HRResourceAccessControls.Add(data);
            db.SaveChanges();

            return RedirectToAction("RemoteAccess");
        }
        #endregion
        #region Leave Scheduling
        public ActionResult DeptLeave()
        {

            var departments = db.Departments.Where(e => e.DepartmentName != null).ToList();

            return View(departments);
        }
        public ActionResult Leave()
        {
            ViewBag.Departments = db.Departments.ToList();
            ViewBag.HRLeaveScheduling = db.HRLeaveSchedulings.ToList();
            var data = db.HRLeaveSchedulings.ToList();

            return View(data);

        }
        [HttpPost]
        public ActionResult SaveLeaveData(HRLeaveScheduling data)
        {
            data.UserId = (int)Session["UserId"];
            data.DateAdded = DateTime.Now;
            data.BranchId = 1;


            db.HRLeaveSchedulings.Add(data);
            db.SaveChanges();

            return RedirectToAction("Leave");
        }
        #endregion
        #region Shift Scheduling
        public ActionResult DepShift()
        {

            var departments = db.Departments.Where(e => e.DepartmentName != null).ToList();

            return View(departments);
        }
        public ActionResult Shift()
        {
            ViewBag.Departments = db.Departments.ToList();
            ViewBag.HRShiftScheduling = db.HRShiftSchedulings.ToList();
            var data = db.HRShiftSchedulings.ToList();

            return View(data);

        }
        [HttpPost]
        public ActionResult SaveShiftData(HRShiftScheduling data)
        {
            data.UserId = (int)Session["UserId"];
            data.DateAdded = DateTime.Now;
            data.BranchId = 1;


            db.HRShiftSchedulings.Add(data);
            db.SaveChanges();

            return RedirectToAction("Shift");
        }
        #endregion
        #region Training Programme
        public ActionResult Training()
        {
           
            ViewBag.HRTraining = db.HRTrainings.ToList();
            var data = db.HRTrainings.ToList();

            return View(data);

        }
        [HttpPost]
        public ActionResult SaveTrainingData(HRTraining data)
        {
            data.UserId = (int)Session["UserId"];
            data.DateAdded = DateTime.Now;
            data.BranchId = 1;


            db.HRTrainings.Add(data);
            db.SaveChanges();

            return RedirectToAction("Training");
        }
        #endregion
        #region Grievances Resolution
        public ActionResult DepGrievances()
        {

            var departments = db.Departments.Where(e => e.DepartmentName != null).ToList();

            return View(departments);
        }
        public ActionResult Grievances()
        {
            ViewBag.Departments = db.Departments.ToList();
            ViewBag.HRGrievanceResolution = db.HRGrievanceResolutions.ToList();
            var data = db.HRGrievanceResolutions.ToList();

            return View(data);

        }
        [HttpPost]
        public ActionResult SaveGrievancesData(HRGrievanceResolution data)
        {
            data.UserId = (int)Session["UserId"];
            data.DateAdded = DateTime.Now;
            data.BranchId = 1;


            db.HRGrievanceResolutions.Add(data);
            db.SaveChanges();

            return RedirectToAction("Grievances");
        }
        #endregion
        #region Staff Loan Applicant Information
        public ActionResult ApplicantInformation()
        {
           
            ViewBag.HRStaffLoan = db.HRStaffLoans.ToList();
            var data = db.HRStaffLoans.ToList();

            return View(data);

        }
        [HttpPost]
        public ActionResult SaveApplicantInformationData(HRStaffLoan data)
        {
            data.UserId = (int)Session["UserId"];
            data.DateAdded = DateTime.Now;
            data.BranchId = 1;


            db.HRStaffLoans.Add(data);
            db.SaveChanges();

            return RedirectToAction("ApplicantInformation");
        }
        #endregion
        #region Processing & Repayment
        public ActionResult ProcessingAndRepayment()
        {

            ViewBag.HRStaffLoanProcessingAndRepayment = db.HRStaffLoanProcessingAndRepayments.ToList();
            var data = db.HRStaffLoanProcessingAndRepayments.ToList();

            return View(data);

        }
        [HttpPost]
        public ActionResult SaveProcessingAndRepaymentData(HRStaffLoanProcessingAndRepayment data)
        {
            data.UserId = (int)Session["UserId"];
            data.DateAdded = DateTime.Now;
            data.BranchId = 1;


            db.HRStaffLoanProcessingAndRepayments.Add(data);
            db.SaveChanges();

            return RedirectToAction("ProcessingAndRepayment");
        }
        #endregion
        #region Policy Administration Operations
        public ActionResult PolicyAdministration()
        {
            
            ViewBag.HHRPolicy = db.HRPolicies.ToList();
            var data = db.HRPolicies.ToList();

            return View(data);

        }
        [HttpPost]
        public ActionResult SavePolicyAdministrationData(HRPolicy data)
        {
            data.UserId = (int)Session["UserId"];
            data.DateAdded = DateTime.Now;
            data.BranchId = 1;


            db.HRPolicies.Add(data);
            db.SaveChanges();

            return RedirectToAction("PolicyAdministration");
        }
        #endregion
        #region Benefits Processing
        public ActionResult Benefits()
        {
           
            ViewBag.HRBenefitsProcessing = db.HRBenefitsProcessings.ToList();
            var data = db.HRBenefitsProcessings.ToList();

            return View(data);

        }
        [HttpPost]
        public ActionResult SaveBenefitsData(HRBenefitsProcessing data)
        {
            data.UserId = (int)Session["UserId"];
            data.DateAdded = DateTime.Now;
            data.BranchId = 1;


            db.HRBenefitsProcessings.Add(data);
            db.SaveChanges();

            return RedirectToAction("Benefits");
        }
        #endregion
        #region Incentives
        public ActionResult DepIncentives()
        {

            var departments = db.Departments.Where(e => e.DepartmentName != null).ToList();

            return View(departments);
        }
        public ActionResult Incentive()
        {
            ViewBag.Departments = db.Departments.ToList();
            ViewBag.HRIncentive = db.HRIncentives.ToList();
            var data = db.HRIncentives.ToList();

            return View(data);

        }
        [HttpPost]
        public ActionResult SaveIncentiveData(HRIncentive data)
        {
            data.UserId = (int)Session["UserId"];
            data.DateAdded = DateTime.Now;
            data.BranchId = 1;


            db.HRIncentives.Add(data);
            db.SaveChanges();

            return RedirectToAction("Incentive");
        }
        #endregion
        #region Contract Termination Reason
        public ActionResult Reason()
        {
            
            ViewBag.HRContractTerminationReason = db.HRContractTerminationReasons.ToList();
            var data = db.HRContractTerminationReasons.ToList();

            return View(data);

        }
        [HttpPost]
        public ActionResult SaveReasonData(HRContractTerminationReason data)
        {
            data.UserId = (int)Session["UserId"];
            data.DateAdded = DateTime.Now;
            data.BranchId = 1;


            db.HRContractTerminationReasons.Add(data);
            db.SaveChanges();

            return RedirectToAction("Reason");
        }
        #endregion
        #region Contract Termination CheckOff
        public ActionResult CheckOff()
        {

            ViewBag.HRContractTerminationCheckOff = db.HRContractTerminationCheckOffs.ToList();
            var data = db.HRContractTerminationCheckOffs.ToList();

            return View(data);

        }
        [HttpPost]
        public ActionResult SaveCheckOffData(HRContractTerminationCheckOff data)
        {
            data.UserId = (int)Session["UserId"];
            data.DateAdded = DateTime.Now;
            data.BranchId = 1;


            db.HRContractTerminationCheckOffs.Add(data);
            db.SaveChanges();

            return RedirectToAction("CheckOff");
        }
        #endregion
        #region Contract Termination Checkout
        public ActionResult CheckOut()
        {

            ViewBag.HRContractTerminationCheckOut = db.HRContractTerminationCheckOuts.ToList();
            var data = db.HRContractTerminationCheckOuts.ToList();

            return View(data);

        }
        [HttpPost]
        public ActionResult SaveCheckOutData(HRContractTerminationCheckOut data)
        {
            data.UserId = (int)Session["UserId"];
            data.DateAdded = DateTime.Now;
            data.BranchId = 1;


            db.HRContractTerminationCheckOuts.Add(data);
            db.SaveChanges();

            return RedirectToAction("CheckOut");
        }
        #endregion
        #region Operator Checkoff System
        public ActionResult Operator()
        {
            
            ViewBag.HROperatorCheckOff = db.HROperatorCheckOffs.ToList();
            var data = db.HROperatorCheckOffs.ToList();

            return View(data);

        }
        [HttpPost]
        public ActionResult SaveOperatorData(HROperatorCheckOff data)
        {
            data.UserId = (int)Session["UserId"];
            data.DateAdded = DateTime.Now;
            data.BranchId = 1;


            db.HROperatorCheckOffs.Add(data);
            db.SaveChanges();

            return RedirectToAction("Operator");
        }
        #endregion
        #region Check off Guaranteering
        public ActionResult Guaranteering()
        {

            ViewBag.HRCheckOffGuaranteering = db.HRCheckOffGuaranteerings.ToList();
            var data = db.HRCheckOffGuaranteerings.ToList();

            return View(data);

        }
        [HttpPost]
        public ActionResult SaveGuaranteeringData(HRCheckOffGuaranteering data)
        {
            data.UserId = (int)Session["UserId"];
            data.DateAdded = DateTime.Now;
            data.BranchId = 1;


            db.HRCheckOffGuaranteerings.Add(data);
            db.SaveChanges();

            return RedirectToAction("Guaranteering");
        }
        #endregion
        #region Claim Management
        public ActionResult ClaimManagement()
        {
           
            ViewBag.HRClaimManagement = db.HRClaimManagements.ToList();
            var data = db.HRClaimManagements.ToList();

            return View(data);

        }
        [HttpPost]
        public ActionResult SaveClaimManagementData(HRClaimManagement data)
        {
            data.UserId = (int)Session["UserId"];
            data.DateAdded = DateTime.Now;
            data.BranchId = 1;


            db.HRClaimManagements.Add(data);
            db.SaveChanges();

            return RedirectToAction("ClaimManagement");
        }
        #endregion
    }
}