using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CaresoftHMISDataAccess;

namespace Caresoft2._0.Controllers
{
    public class Seeder
    {
        private CaresoftHMISEntities db = new CaresoftHMISEntities();
        public void SeedPatientDemographics()
        {
            string[] relations = new string[] { "Son", "Daughter", "Father", "Mother", "Brother", "Sister" };
            if (db.Relationships.ToList().Count() < relations.Count())
            {

                var realationship = new Relationship();
                foreach (var rel in relations)
                {
                    if (db.Relationships.FirstOrDefault(e => e.RelationshipName.Trim().ToLower().Equals(rel.ToLower().Trim())) == null)
                    {
                        realationship.RelationshipName = rel;
                        db.Relationships.Add(realationship);
                        db.SaveChanges();
                    }

                }
            }

            string[] bloodGroups = new string[] { "A+", "A-", "B+", "B-", "AB+", "AB-", "O+", "O-" };
            if (db.Relationships.ToList().Count() < bloodGroups.Count())
            {

                var bg = new BloodGroup();
                foreach (var bgName in bloodGroups)
                {
                    if (db.BloodGroups.FirstOrDefault(e =>
                    e.BloodGroupName.Trim().ToLower().Equals(bgName.ToLower().Trim())) == null)
                    {
                        bg.BloodGroupName = bgName;
                        db.BloodGroups.Add(bg);
                        db.SaveChanges();
                    }

                }
            }

            string[] religions = new string[] { "Christian", "Muslim", "Hindu" };
            if (db.Religions.ToList().Count() < religions.Count())
            {

                var r = new Religion();
                foreach (var rName in religions)
                {
                    if (db.Religions.FirstOrDefault(e =>
                    e.ReligionName.Trim().ToLower().Equals(rName.ToLower().Trim())) == null)
                    {
                        r.ReligionName = rName;
                        db.Religions.Add(r);
                        db.SaveChanges();
                    }

                }
            }

            string[] maritals = new string[] { "Single", "Married", "Separated", "Divorced", "Widowed", "Baby", "Prefer not to answer" };
            if (db.MaritalStatus.ToList().Count() < maritals.Count())
            {

                var m = new MaritalStatu();
                foreach (var mName in maritals)
                {
                    if (db.MaritalStatus.FirstOrDefault(e =>
                    e.MaritalStatusName.Trim().ToLower().Equals(mName.ToLower().Trim())) == null)
                    {
                        m.MaritalStatusName = mName;
                        db.MaritalStatus.Add(m);
                        db.SaveChanges();
                    }

                }
            }
            string[] idTypes = new string[] { "National Card", "License", "Birth Cirtificate" };
            if (db.IdentificationTypes.ToList().Count() < idTypes.Count())
            {
                var it = new IdentificationType();
                foreach (var itName in idTypes)
                {
                    if (db.IdentificationTypes.FirstOrDefault(e =>
                    e.IdentificationTypeName.Trim().ToLower().Equals(itName.ToLower().Trim())) == null)
                    {
                        it.IdentificationTypeName = itName;
                        db.IdentificationTypes.Add(it);
                        db.SaveChanges();
                    }

                }
            }
        }

        public void SeedServicesMetaData()
        {
            string[] serviceGroups = new string[] { "Consultation", "Labs", "XRAY", "Drugs", "Procedure", "Package" };
            if (db.ServiceGroups.ToList().Count() < serviceGroups.Count())
            {

                var sg = new ServiceGroup();
                foreach (var sgNmae in serviceGroups)
                {
                    if (db.ServiceGroups.FirstOrDefault(e => e.ServiceGroupName.Trim().ToLower().Equals(sgNmae.ToLower().Trim())) == null)
                    {
                        sg.ServiceGroupName = sgNmae;
                        db.ServiceGroups.Add(sg);
                        db.SaveChanges();
                    }

                }
            }
        }

        public void SeedSalutations()
        {
            string[] salutes = new string[] { "Mr", "Mrs", "Miss", "Baby", "Master" };
            if (db.Salutations.ToList().Count() < salutes.Count())
            {
                foreach (var m in salutes)
                {
                    var pm = new Salutation();
                    if (db.Salutations.FirstOrDefault(e => e.SalutationName.Trim().ToLower().Equals(m.ToLower().Trim())) == null)
                    {
                        pm.SalutationName = m;
                        db.Salutations.Add(pm);
                        db.SaveChanges();
                    }

                }
            }
        }

        public void SeedPaymentModes()
        {
            string[] modes = new string[] { "Cash", "MPesa", "CCard" };
            if (db.PaymentModes.ToList().Count() < modes.Count())
            {

                var pm = new PaymentMode();
                foreach (var m in modes)
                {
                    if (db.PaymentModes.FirstOrDefault(e => e.PaymentModeName.Trim().ToLower().Equals(m.ToLower().Trim())) == null)
                    {
                        pm.PaymentModeName = m;
                        db.PaymentModes.Add(pm);
                        db.SaveChanges();
                    }

                }
            }
        }

        public int SeedCashAsATariff()
        {
            var tariff = new Tariff();

            var companyCash = db.Companies.FirstOrDefault(e => e.CompanyName.Trim().ToLower().Equals("cash"));
            if(companyCash == null)
            {
                var com = new Company();
                com.CompanyName = "Cash";
                var typeGeneral = db.CompanyTypes.FirstOrDefault(e => e.CompanyTypeName.Trim().ToLower().Equals("general"));
                if (typeGeneral == null)
                {
                    var comType = new CompanyType();
                    comType.CompanyTypeName = "General";
                    comType.DateAdded = DateTime.Now;
                    db.CompanyTypes.Add(comType);
                    db.SaveChanges();
                    com.CompanyTypeId = comType.Id;
                }
                else
                {
                    com.CompanyTypeId = typeGeneral.Id;
                }
                com.DateAdded = DateTime.Now;
                db.Companies.Add(com);
                db.SaveChanges();
                tariff.CompanyId = com.Id;
            }
            else
            {
                tariff.CompanyId = companyCash.Id;
            }
            tariff.TariffName = "Cash";
            tariff.DateAdded = DateTime.Now;
            db.Tariffs.Add(tariff);
            db.SaveChanges();

            return tariff.Id;
        }
    }
}