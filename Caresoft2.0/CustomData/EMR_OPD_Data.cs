using CaresoftHMISDataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Caresoft2._0.CustomData
{
    public class EMR_OPD_Data
    {
        public Patient Patient { get; set; }
        public OpdRegister OpdRegister { get; set; }
        public List<OpdRegister> OPDs { get; set; }
        public List<Complaint> Complaints { get; set; }
        public List<SystemExamCategory> SystemExamCategories { get; set; }
        public List<MedicalHistoryQuestion> MedHistoryQuestions { get; set; }
        public List<FamilyMedicalHistoryQuestion> FamilyMedHistQuestions { get; set; }
        public Complaint TodaysComplaint { get; set; }
        public List<Questionnaire> Questionnaires { get; set; }
    }
}