using Caresoft2._0.CrystalReports.Doctors;
using Caresoft2._0.CrystalReports.MOH711.A;
using Caresoft2._0.CrystalReports.MOH711.B;
using Caresoft2._0.CrystalReports.MOH711.C;
using Caresoft2._0.CrystalReports.MOH711.D;
using Caresoft2._0.CrystalReports.MOH711.E;
using Caresoft2._0.CrystalReports.MOH711.F1;
using Caresoft2._0.CrystalReports.MOH711.H;
using Caresoft2._0.CrystalReports.MOH711.I;

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
        public ActionResult GenerateReport()
        {
            ReportDocument rd = new ReportDocument();
            rd.Load(Path.Combine(Server.MapPath("~/CrystalReports/MOH711/Main.rpt")));
            //rd.SetDataSource(dataSet);

            
            rd.Subreports["MOH711.rpt"].SetDataSource(GetAncSampleData());
            rd.Subreports["MOH711B.rpt"].SetDataSource(GetMaternityAndDSampleData());
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

       

       

        private object GetPhysiotherapyServiceSampleData()
        {

            
            var dataSet = new Caresoft2._0.CrystalReports.MOH711.K.Physiotherapy_Service();

            //dataSet.PysiotherapyService.AddPysiotherapyServiceRow(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,0);

            return dataSet;

        }

        private object GetMSocialWorkJSampleData()
        {
            //MSocialWorkJ mSocialWorkJ = new MSocialWorkJ();
            var dataset = new Caresoft2._0.CrystalReports.MOH711.J.MSocialworkJ();
            //dataset.MSocialWorkJ.AddMSocialWorkJRow(0, 0, 0, 0, 0, 0, 0, 0, 0, 0);


            return dataset;

        }

        private object GetRehabServicesISampleData()
        {
            RehabServicesI rehabServicesI = new RehabServicesI();
            //rehabServicesI.RehabI.AddRehabIRow(0, 2, 1, 3, 1);
            return rehabServicesI;
        }

        private object GetPNCHSampleData()
        {
            PNCH pNCH = new PNCH();
            //pNCH.PNC.AddPNCRow(0, 0, 4, 1, 2, 4, 1, 0, 3, 2, 1, 0, 4, 0);
            return pNCH;
        }

        private object GetCervicalSampleData()
        {
            Caresoft2._0.CrystalReports.MOH711.G.DataSet1 dataSet = new G.DataSet1();

            //dataSet.CervicalCancer_G.AddCervicalCancer_GRow(1, 2, 3, 4, 5, 6, 7, 8, 9, 8, 7, 6, 5, 4, 3, 2, 1, 2, 3, 4, 5, 7, 8, 9, 8, 7, 6, 5, 4, 3);

            return dataSet;
        }

        private object GetDatasetFSampleData()
        {
            DataSetF datasetF = new DataSetF();
            //datasetF.F.AddFRow(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
            return datasetF;
        }

        private object GetDatasetESampleData()
        {
            DatasetE datasetE = new DatasetE();
            //datasetE._DatasetE.AddDatasetERow(1, 2);
            return datasetE;

        }

        private object GetFPSampleData()
        {
            FP datasetFP = new FP();
            //datasetFP.FamilyPlanningD.AddFamilyPlanningDRow(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33, 34);
            return datasetFP;
        }

        private object GetSGBVCCCSampleData()
        {
            SGBVCCC datasetSGBVCCC = new SGBVCCC();
            //datasetSGBVCCC.SGBV.AddSGBVRow(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
            return datasetSGBVCCC;
        }

        



        private ANC GetAncSampleData()
        {
            ANC dataSetANC = new ANC();
            
            //dataSetANC.A_ANC_PMCT1.AddA_ANC_PMCT1Row(20, 8, 12, 4, 4, 1, 6, 0, 1, 0, 1, 3, 6, 1, 0, 0, 0);

            return dataSetANC;
        }

        public ActionResult GetMAndDReport (DateTime FromDate, DateTime ToDate)
        {
            var data = db.MCHDeliveries
                .Where(p => DbFunctions.TruncateTime(p.DateOfDelivery) >= FromDate && DbFunctions.TruncateTime(p.DateOfDelivery) <= ToDate)
                .ToList();
            var ND = db.MCHDeliveries
                .Where(p => p.ModeOfDelivery.Contains("Normal Deliveries")).Count();
            var CS = db.MCHDeliveries
                .Where(p => p.ModeOfDelivery.Contains("Caesarean Sections")).Count();
            var BD = db.MCHDeliveries
                .Where(p => p.ModeOfDelivery.Contains("Breech Deliveries")).Count();
            var AVD = db.MCHDeliveries
                .Where(p => p.ModeOfDelivery.Contains("Assisted Vaginal Deliveries")).Count();
            var TD =ND+CS+BD+AVD;
            MAndD datasetMAndD = new MAndD();
            datasetMAndD._B__Maternity_and_Delivery.Add_B__Maternity_and_DeliveryRow(ND, CS, BD, AVD, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);

            return DataSet();
            

        }

        private object GetMaternityAndDSampleData()
        {


            MAndD datasetMAndD = new MAndD();
            //datasetMAndD._B__Maternity_and_Delivery.Add_B__Maternity_and_DeliveryRow(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
            return datasetMAndD;
        }
        private ActionResult DataSet()
        {
            //throw new NotImplementedException();
            return null;
        }
    }
   
}