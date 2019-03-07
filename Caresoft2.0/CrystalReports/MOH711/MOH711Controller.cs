
using Caresoft2._0.CrystalReports.MOH711.A;
using Caresoft2._0.CrystalReports.MOH711.B;
using Caresoft2._0.CrystalReports.MOH711.C;
using Caresoft2._0.CrystalReports.MOH711.D;
using Caresoft2._0.CrystalReports.MOH711.E;
using Caresoft2._0.CrystalReports.MOH711.F1;
using Caresoft2._0.CrystalReports.MOH711.H;
using Caresoft2._0.CrystalReports.MOH711.I;
using Caresoft2._0.CrystalReports.MOH711.J;
using CaresoftHMISDataAccess;
using CrystalDecisions.CrystalReports.Engine;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web.Mvc;

namespace Caresoft2._0.CrystalReports.MOH711
{
    [Auth]
    public class MOH711Controller : Controller
    {
        private CaresoftHMISEntities db = new CaresoftHMISEntities();
        private object dataSet;
        private object opd;
        private readonly int patient;

        public int MOH711 { get; private set; }
        public IEnumerable<object> MCHDeliveries { get; private set; }
        public object CervicalCancerG { get; private set; }

        public ActionResult Index()
        {
            return View();
        }

        //MOH711/GenerateReport
        public ActionResult GenerateReport(DateTime FromDate, DateTime ToDate)
        {
            var fromDate = FromDate;
            var toDate = ToDate.Date;

            ReportDocument rd = new ReportDocument();
            rd.Load(Path.Combine(Server.MapPath("~/CrystalReports/MOH711/Main.rpt")));
            //rd.SetDataSource(dataSet);

            var ANC = GetAncSampleData();
            var NewMAndD = GetMaternityAndDSampleData( FromDate, ToDate);
            var SGBVCCC = GetSGBVCCCSampleData();
            var FP = GetFPSampleData();
            var DatasetE = GetDatasetESampleData();
            var DatasetF = GetDatasetFSampleData();
            var CervicalCancerG = GetCervicalSampleData();
            var PNCH = GetPNCHSampleData();
            var RehabServicesI = GetRehabServicesISampleData();
            var MSocialWorkJ = GetMSocialWorkJSampleData();
            var PhysiotherapyService = GetPhysiotherapyServiceSampleData();

            

            rd.Subreports["MOH711.rpt"].SetDataSource(GetAncSampleData());
            rd.Subreports["MOH711B.rpt"].SetDataSource(GetMaternityAndDSampleData( FromDate,ToDate));
            rd.Subreports["SGBV.rpt"].SetDataSource(GetSGBVCCCSampleData());
            rd.Subreports["FamilyPlanningD.rpt"].SetDataSource(GetFPSampleData());
            rd.Subreports["PACServicesE.rpt"].SetDataSource(GetDatasetESampleData());
            rd.Subreports["CHANISF.rpt"].SetDataSource(GetDatasetFSampleData());
            rd.Subreports["CervicalCancerG.rpt"].SetDataSource(GetCervicalSampleData());
            rd.Subreports["PostNatalCareH.rpt"].SetDataSource(GetPNCHSampleData());
            rd.Subreports["RehabI.rpt"].SetDataSource(GetRehabServicesISampleData());
            rd.Subreports["MSocialWorkJ.rpt"].SetDataSource(GetMSocialWorkJSampleData());
            rd.Subreports["PhysiotherapyService.rpt"].SetDataSource(GetPhysiotherapyServiceSampleData());
           

            Response.Buffer = false;
            Response.ClearContent();
            Response.ClearHeaders();

            Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
            stream.Seek(0, SeekOrigin.Begin);
            string fileName = "Report Sample-" + DateTime.Today + ".pdf";
            return File(stream, "application/pdf", fileName);
        }
        private ANC GetAncSampleData()
        {
            ANC dataSetANC = new ANC();
            var NewClients = db.MCHPreventativeServices.Count(p => p.OPDNo == 1);
            var Revisits = db.MCHPreventativeServices.Count(p => p.OPDNo > 1);

            dataSetANC.A_ANC_PMCT1.AddA_ANC_PMCT1Row(NewClients, Revisits, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);

            return dataSetANC;
        }


        private object GetPhysiotherapyServiceSampleData()
        {

            var dataSet = new Caresoft2._0.CrystalReports.MOH711.K.Physiotherapy_Service();

            dataSet.PysiotherapyService.AddPysiotherapyServiceRow(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,0);

            return dataSet;

        }

        private object GetMSocialWorkJSampleData()
        {
            MSocialWorkJ mSocialWorkJ = new MSocialWorkJ();
            var dataset = new Caresoft2._0.CrystalReports.MOH711.J.MSocialworkJ();
            dataset.MSocialWorkJ.AddMSocialWorkJRow(0, 0, 0, 0, 0, 0, 0, 0, 0, 0);


            return dataset;

        }

        private object GetRehabServicesISampleData()
        {
            RehabServicesI rehabServicesI = new RehabServicesI();
            rehabServicesI.RehabI.AddRehabIRow(0, 0, 0, 0, 0);
            return rehabServicesI;
        }

        private object GetPNCHSampleData()
        {
            PNCH pNCH = new PNCH();
            pNCH.PNC.AddPNCRow(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
            return pNCH;
        }

        private object GetCervicalSampleData()
        {
            Caresoft2._0.CrystalReports.MOH711.G.DataSet1 dataSet = new G.DataSet1();

            dataSet.CervicalCancer_G.AddCervicalCancer_GRow(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);

            return dataSet;
        }

        private object GetDatasetFSampleData()
        {
            DataSetF datasetF = new DataSetF();
           datasetF.F.AddFRow(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
            return datasetF;
        }

        private object GetDatasetESampleData()
        {
            DatasetE datasetE = new DatasetE();
            datasetE._DatasetE.AddDatasetERow(0, 0);
            return datasetE;

        }

        private object GetFPSampleData()
        {
            FP datasetFP = new FP();
            datasetFP.FamilyPlanningD.AddFamilyPlanningDRow(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
            return datasetFP;
        }

        private object GetSGBVCCCSampleData()
        {
            SGBVCCC datasetSGBVCCC = new SGBVCCC();
            datasetSGBVCCC.SGBV.AddSGBVRow(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
            return datasetSGBVCCC;
        }

        



        

        //public ActionResult GetMAndDReport (DateTime FromDate, DateTime ToDate)
        //{
        //    var data = db.MCHDeliveries
        //        .Where(p => DbFunctions.TruncateTime(p.DateOfDelivery) >= FromDate && DbFunctions.TruncateTime(p.DateOfDelivery) <= ToDate)
        //        .ToList();
        //    var ND = db.MCHDeliveries
        //        .Where(p => p.ModeOfDelivery.Contains("Normal Deliveries")).Count();
        //    var CS = db.MCHDeliveries
        //        .Where(p => p.ModeOfDelivery.Contains("Caesarean Sections")).Count();
        //    var BD = db.MCHDeliveries
        //        .Where(p => p.ModeOfDelivery.Contains("Breech Deliveries")).Count();
        //    var AVD = db.MCHDeliveries
        //        .Where(p => p.ModeOfDelivery.Contains("Assisted Vaginal Deliveries")).Count();
        //    var TD =ND+CS+BD+AVD;
        //    MAndD datasetMAndD = new MAndD();
        //    datasetMAndD._B__Maternity_and_Delivery.Add_B__Maternity_and_DeliveryRow(ND, CS, BD, AVD, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);

        //    return DataSet();
            

        //}


       public class MaternityServices
        {
            public string NameOfService { get; set; }
            public int Total { get; set; }

            
        }
        private NewMAndD GetMaternityAndDSampleData(DateTime FromDate, DateTime ToDate)
        {
            var deliveries = db.BillServices.Where(e => DbFunctions.TruncateTime(e.DateAdded) >= FromDate &&
            DbFunctions.TruncateTime(e.DateAdded) <= ToDate && e.Service.Department.DepartmentName.ToUpper()
            .Contains("MATERNITY"));

            NewMAndD newMAndD = new NewMAndD();

            MaternityServices MSNormalDelivery = new MaternityServices()
            {
                NameOfService = "NormalDelivery",
                Total = deliveries.Count(e => e.Service.ServiceName.Trim().ToUpper().Contains("NORMAL DELIVERY"))
            };
            MaternityServices MSCaesareanSection = new MaternityServices()
            {
                NameOfService = "Caesarean Section",
                Total = deliveries.Count(e => e.Service.ServiceName.Trim().ToUpper().Contains("DELIVERY CS"))
            };
            MaternityServices MSBreechDeliveries = new MaternityServices()
            {
                NameOfService = "Breech Deliveries",
                Total = deliveries.Count(e => e.Service.ServiceName.Trim().ToUpper().Contains("BREECH DELIVERIES"))
            };
            MaternityServices MSAssistedVaginalDelivery = new MaternityServices()
            {
                NameOfService = "Assisted Vaginal Delivery ",
                Total = deliveries.Count(e => e.Service.ServiceName.Trim().ToUpper().Contains("ASSISTED VAGINAL DELIVERY"))
            };
            MaternityServices MSTotalDeliveries = new MaternityServices()
            {
                NameOfService = "TOTAL DELIVERIES ",
                Total = 0
            };
            MaternityServices MSLiveBirths = new MaternityServices()
            {
                NameOfService = "Live Births",
                Total = 0
            };
            MaternityServices MSFreshStillBirth = new MaternityServices()
            {
                NameOfService = "Fresh Still Births",
                Total = 0
            };
            MaternityServices MSMaceratedStillBirths = new MaternityServices()
            {
                NameOfService = "Macerated Still Births",
                Total = 0
            };
            MaternityServices MSBirthWithDeformities = new MaternityServices()
            {
                NameOfService = "Birth with Deformities",
                Total = 0
            };
            MaternityServices MSLowAPGARScore = new MaternityServices()
            {
                NameOfService = " No. of Low APGAR Score",
                Total = 0
            };
            MaternityServices MSLowBirthWeight = new MaternityServices()
            {
                NameOfService = "Low Birth Weight Babies(under 2500gms)",
                Total = 0
            };
            MaternityServices MSBabiesGivenTetracyclineAtBirth = new MaternityServices()
            {
                NameOfService = "No. of bAbies given tetracycline at birth",
                Total = 0
            };
            MaternityServices MSPreTermBabies = new MaternityServices()
            {
                NameOfService = "Pre-Term babies",
                Total = 0
            };
            MaternityServices MSBabiesDischargedAlive = new MaternityServices()
            {
                NameOfService = "No.of babies discharged alive",
                Total = 0
            };
            MaternityServices MSInfantsInitiatedOnBreastFeeding = new MaternityServices()
            {
                NameOfService = "No.of infants initiated on breestfeeding within 1 hour after birth",
                Total = 0
            };
            MaternityServices MSTotalDeliveriesFromHIVWomen = new MaternityServices()
            {
                NameOfService = "Total Deliveries from HIV +ve women",
                Total = 0
            };
            MaternityServices MSNeotanalDeaths = new MaternityServices()
            {
                NameOfService = "Neotanal Deaths",
                Total = 0
            };
            MaternityServices MSNoOfAdolescentsMaternalDeaths = new MaternityServices()
            {
                NameOfService = "No.of adolescents (10-19YRS) Maternal Deaths",
                Total = 0
            };
            MaternityServices MSMaternalDeaths = new MaternityServices()
            {
                NameOfService = "MaternalDeaths",
                Total = 0
            };
            MaternityServices MSMaternalDeathsAudited = new MaternityServices()
            {
                NameOfService = "Maternal Deaths Audited",
                Total = 0
            };

            List<MaternityServices> LstMaternityServices = new List<MaternityServices>();
            LstMaternityServices.Add(MSNormalDelivery);
            LstMaternityServices.Add(MSCaesareanSection);
            LstMaternityServices.Add(MSBreechDeliveries);
            LstMaternityServices.Add(MSAssistedVaginalDelivery);
            LstMaternityServices.Add(MSTotalDeliveries);
            LstMaternityServices.Add(MSLiveBirths);
            LstMaternityServices.Add(MSFreshStillBirth);
            LstMaternityServices.Add(MSMaceratedStillBirths);
            LstMaternityServices.Add(MSBirthWithDeformities);
            LstMaternityServices.Add(MSLowAPGARScore);
            LstMaternityServices.Add(MSLowBirthWeight);
            LstMaternityServices.Add(MSBabiesGivenTetracyclineAtBirth);
            LstMaternityServices.Add(MSPreTermBabies);
            LstMaternityServices.Add(MSBabiesDischargedAlive);
            LstMaternityServices.Add(MSInfantsInitiatedOnBreastFeeding);
            LstMaternityServices.Add(MSTotalDeliveriesFromHIVWomen);
            LstMaternityServices.Add(MSNeotanalDeaths);
            LstMaternityServices.Add(MSNoOfAdolescentsMaternalDeaths);
            LstMaternityServices.Add(MSMaternalDeaths);
            LstMaternityServices.Add(MSMaternalDeathsAudited);

            

            foreach (var item in LstMaternityServices)
            {
                newMAndD._NewMAndD.AddNewMAndDRow(
                        item.NameOfService,
                        item.Total.ToString()
                    );
            }


            return newMAndD;
        }
        private ActionResult DataSet()
        {
            //throw new NotImplementedException();
            return null;
        }
    }
   
}