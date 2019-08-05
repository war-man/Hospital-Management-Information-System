using CaresoftHMISDataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Caresoft2._0.Utils
{
    public class OPDRegMigration
    {
        private CaresoftHMISEntities db = new CaresoftHMISEntities();

        public string Migrate()
        {
            var data = db.OpdRegisters.ToList();
            foreach (var i in data)
            {
                var opReg = db.OpdRegisters.Find(i.Id);
                var branchini = db.KeyValuePairs.FirstOrDefault(e => e.Key_.ToLower().Equals("branchid"))?.Value ?? "0";
                opReg.Id = branchini + "|" + i.Id;
                db.SaveChanges();

                if (opReg.AdmissionRecommendations.Count() > 0)
                {
                    var lis = db.AdmissionRecommendations.Where(e => e.OPDNo == i.Id).ToList();
                    foreach(var u in lis)
                    {
                        var ad = db.AdmissionRecommendations.Find(u.Id);
                        ad.OPDNo = opReg.Id;
                        db.SaveChanges();
                    }
                }
                if (opReg.Allergies.Count() > 0)
                {
                    var lis = db.Allergies.Where(e => e.OPDno == i.Id).ToList();
                    foreach (var u in lis)
                    {
                        var ad = db.Allergies.Find(u.Id);
                        ad.OPDno = opReg.Id;
                        db.SaveChanges();
                    }
                }
                if (opReg.AnaestheticRecords.Count() > 0)
                {
                    var lis = db.AnaestheticRecords.Where(e => e.IPDNo == i.Id).ToList();
                    foreach (var u in lis)
                    {
                        var ad = db.AnaestheticRecords.Find(u.Id);
                        ad.IPDNo = opReg.Id;
                        db.SaveChanges();
                    }
                }

                if (opReg.AssignmentDetails.Count() > 0)
                {
                    var lis = db.AssignmentDetails.Where(e => e.OPDNo == i.Id).ToList();
                    foreach (var u in lis)
                    {
                        var ad = db.AssignmentDetails.Find(u.Id);
                        ad.OPDNo = opReg.Id;
                        db.SaveChanges();
                    }
                }
                if (opReg.BedShiftLogs.Count() > 0)
                {
                    var lis = db.BedShiftLogs.Where(e => e.OPDNo == i.Id).ToList();
                    foreach (var u in lis)
                    {
                        var ad = db.BedShiftLogs.Find(u.Id);
                        ad.OPDNo = opReg.Id;
                        db.SaveChanges();
                    }
                }
                if (opReg.BillPayments.Count() > 0)
                {
                    var lis = db.BillPayments.Where(e => e.OPDNo == i.Id).ToList();
                    foreach (var u in lis)
                    {
                        var ad = db.BillPayments.Find(u.Id);
                        ad.OPDNo = opReg.Id;
                        db.SaveChanges();
                    }
                }
                if (opReg.BillServices.Count() > 0)
                {
                    var lis = db.BillServices.Where(e => e.OPDNo == i.Id).ToList();
                    foreach (var u in lis)
                    {
                        var ad = db.BillServices.Find(u.Id);
                        ad.OPDNo = opReg.Id;
                        db.SaveChanges();
                    }
                }
                if (opReg.BloodPressureCharts.Count() > 0)
                {
                    var lis = db.BloodPressureCharts.Where(e => e.IPDNo == i.Id).ToList();
                    foreach (var u in lis)
                    {
                        var ad = db.BloodPressureCharts.Find(u.Id);
                        ad.IPDNo = opReg.Id;
                        db.SaveChanges();
                    }
                }
                if (opReg.BloodSugarCharts.Count() > 0)
                {
                    var lis = db.BloodSugarCharts.Where(e => e.IPDNo == i.Id).ToList();
                    foreach (var u in lis)
                    {
                        var ad = db.BloodSugarCharts.Find(u.Id);
                        ad.IPDNo = opReg.Id;
                        db.SaveChanges();
                    }
                }
                if (opReg.Complaints.Count() > 0)
                {
                    var lis = db.Complaints.Where(e => e.OpdIpdNumber == i.Id).ToList();
                    foreach (var u in lis)
                    {
                        var ad = db.Complaints.Find(u.Id);
                        ad.OpdIpdNumber = opReg.Id;
                        db.SaveChanges();
                    }
                }
                if (opReg.CorneaClinicEntries.Count() > 0)
                {
                    var lis = db.CorneaClinicEntries.Where(e => e.OPDNo == i.Id).ToList();
                    foreach (var u in lis)
                    {
                        var ad = db.CorneaClinicEntries.Find(u.Id);
                        ad.OPDNo = opReg.Id;
                        db.SaveChanges();
                    }
                }
                if (opReg.CSSDIssueToPatients.Count() > 0)
                {
                    var lis = db.CSSDIssueToPatients.Where(e => e.OPDNo == i.Id).ToList();
                    foreach (var u in lis)
                    {
                        var ad = db.CSSDIssueToPatients.Find(u.Id);
                        ad.OPDNo = opReg.Id;
                        db.SaveChanges();
                    }
                }
                if (opReg.DeathCertificates.Count() > 0)
                {
                    var lis = db.DeathCertificates.Where(e => e.OPDno == i.Id).ToList();
                    foreach (var u in lis)
                    {
                        var ad = db.DeathCertificates.Find(u.Id);
                        ad.OPDno = opReg.Id;
                        db.SaveChanges();
                    }
                }
                if (opReg.Deformities.Count() > 0)
                {
                    var lis = db.Deformities.Where(e => e.OPDno== i.Id).ToList();
                    foreach (var u in lis)
                    {
                        var ad = db.Deformities.Find(u.Id);
                        ad.OPDno = opReg.Id;
                        db.SaveChanges();
                    }
                }
                if (opReg.DischargeRecommendations.Count() > 0)
                {
                    var lis = db.DischargeRecommendations.Where(e => e.IpdNo == i.Id).ToList();
                    foreach (var u in lis)
                    {
                        var ad = db.DischargeRecommendations.Find(u.Id);
                        ad.IpdNo = opReg.Id;
                        db.SaveChanges();
                    }
                }
                if (opReg.DoctorNotes.Count() > 0)
                {
                    var lis = db.DoctorNotes.Where(e => e.IpdOpdNo == i.Id).ToList();
                    foreach (var u in lis)
                    {
                        var ad = db.DoctorNotes.Find(u.Id);
                        ad.IpdOpdNo = opReg.Id;
                        db.SaveChanges();
                    }
                }
                if (opReg.FamilyHealthOverviews.Count() > 0)
                {
                    var lis = db.FamilyHealthOverviews.Where(e => e.OPDNo == i.Id).ToList();
                    foreach (var u in lis)
                    {
                        var ad = db.FamilyHealthOverviews.Find(u.Id);
                        ad.OPDNo = opReg.Id;
                        db.SaveChanges();
                    }
                }
                if (opReg.FamilyMedicalHistoryResponses.Count() > 0)
                {
                    var lis = db.FamilyMedicalHistoryResponses.Where(e => e.OPDNo == i.Id).ToList();
                    foreach (var u in lis)
                    {
                        var ad = db.FamilyMedicalHistoryResponses.Find(u.Id);
                        ad.OPDNo = opReg.Id;
                        db.SaveChanges();
                    }
                }
                if (opReg.Fibromyalgias.Count() > 0)
                {
                    var lis = db.Fibromyalgias.Where(e => e.OPDno == i.Id).ToList();
                    foreach (var u in lis)
                    {
                        var ad = db.Fibromyalgias.Find(u.Id);
                        ad.OPDno = opReg.Id;
                        db.SaveChanges();
                    }
                }
                if (opReg.FitnessCertificates.Count() > 0)
                {
                    var lis = db.FitnessCertificates.Where(e => e.OPDno == i.Id).ToList();
                    foreach (var u in lis)
                    {
                        var ad = db.FitnessCertificates.Find(u.Id);
                        ad.OPDno = opReg.Id;
                        db.SaveChanges();
                    }
                }
                if (opReg.GlucomaPerfomaEntries.Count() > 0)
                {
                    var lis = db.GlucomaPerfomaEntries.Where(e => e.OPDNo == i.Id).ToList();
                    foreach (var u in lis)
                    {
                        var ad = db.GlucomaPerfomaEntries.Find(u.Id);
                        ad.OPDNo = opReg.Id;
                        db.SaveChanges();
                    }
                }
                if (opReg.HTCServiceSummaries.Count() > 0)
                {
                    var lis = db.HTCServiceSummaries.Where(e => e.OPDNo == i.Id).ToList();
                    foreach (var u in lis)
                    {
                        var ad = db.HTCServiceSummaries.Find(u.Id);
                        ad.OPDNo = opReg.Id;
                        db.SaveChanges();
                    }
                }
                if (opReg.Hypermobilities.Count() > 0)
                {
                    var lis = db.Hypermobilities.Where(e => e.OPDno == i.Id).ToList();
                    foreach (var u in lis)
                    {
                        var ad = db.Hypermobilities.Find(u.Id);
                        ad.OPDno = opReg.Id;
                        db.SaveChanges();
                    }
                }
                if (opReg.ImmunizationAdmins.Count() > 0)
                {
                    var lis = db.ImmunizationAdmins.Where(e => e.OPDNo == i.Id).ToList();
                    foreach (var u in lis)
                    {
                        var ad = db.ImmunizationAdmins.Find(u.Id);
                        ad.OPDNo = opReg.Id;
                        db.SaveChanges();
                    }
                }
                if (opReg.InformedConsents.Count() > 0)
                {
                    var lis = db.InformedConsents.Where(e => e.OPDno == i.Id).ToList();
                    foreach (var u in lis)
                    {
                        var ad = db.InformedConsents.Find(u.Id);
                        ad.OPDno = opReg.Id;
                        db.SaveChanges();
                    }
                }
                if (opReg.InfusionTherapies.Count() > 0)
                {
                    var lis = db.InfusionTherapies.Where(e => e.OPDNo == i.Id).ToList();
                    foreach (var u in lis)
                    {
                        var ad = db.InfusionTherapies.Find(u.Id);
                        ad.OPDNo = opReg.Id;
                        db.SaveChanges();
                    }
                }
                if (opReg.InsuranceCards.Count() > 0)
                {
                    var lis = db.InsuranceCards.Where(e => e.OPDNo == i.Id).ToList();
                    foreach (var u in lis)
                    {
                        var ad = db.InsuranceCards.Find(u.Id);
                        ad.OPDNo = opReg.Id;
                        db.SaveChanges();
                    }
                }
                if (opReg.InsuranceInvoices.Count() > 0)
                {
                    var lis = db.InsuranceInvoices.Where(e => e.OpdId == i.Id).ToList();
                    foreach (var u in lis)
                    {
                        var ad = db.InsuranceInvoices.Find(u.Id);
                        ad.OpdId = opReg.Id;
                        db.SaveChanges();
                    }
                }
                if (opReg.IntakeInputOutputs.Count() > 0)
                {
                    var lis = db.IntakeInputOutputs.Where(e => e.OpdNo == i.Id).ToList();
                    foreach (var u in lis)
                    {
                        var ad = db.IntakeInputOutputs.Find(u.Id);
                        ad.OpdNo = opReg.Id;
                        db.SaveChanges();
                    }
                }
                if (opReg.JPayAutoPayments.Count() > 0)
                {
                    var lis = db.JPayAutoPayments.Where(e => e.OPDNo == i.Id).ToList();
                    foreach (var u in lis)
                    {
                        var ad = db.JPayAutoPayments.Find(u.Id);
                        ad.OPDNo = opReg.Id;
                        db.SaveChanges();
                    }
                }
                if (opReg.JPayments.Count() > 0)
                {
                    var lis = db.JPayments.Where(e => e.OPDNo == i.Id).ToList();
                    foreach (var u in lis)
                    {
                        var ad = db.JPayments.Find(u.Id);
                        ad.OPDNo = opReg.Id;
                        db.SaveChanges();
                    }
                }
                if (opReg.LasikClinicEntries.Count() > 0)
                {
                    var lis = db.LasikClinicEntries.Where(e => e.OPDNo == i.Id).ToList();
                    foreach (var u in lis)
                    {
                        var ad = db.LasikClinicEntries.Find(u.Id);
                        ad.OPDNo = opReg.Id;
                        db.SaveChanges();
                    }
                }
                if (opReg.LeaveLetters.Count() > 0)
                {
                    var lis = db.LeaveLetters.Where(e => e.OPDno == i.Id).ToList();
                    foreach (var u in lis)
                    {
                        var ad = db.LeaveLetters.Find(u.Id);
                        ad.OPDno = opReg.Id;
                        db.SaveChanges();
                    }
                }
            }
            return "";
        }
    }
}