using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CaresoftHMISDataAccess;

namespace Caresoft2._0.CustomData
{
    public class RegistrationData
    {
        public List<CompanyType> MainCategories { get; set; }
        public List<Company> Categories { get; set; }
        public List<Salutation> Salutations { get; set; }
        public List<BloodGroup> BloodGroups { get; set; }
        public List<MaritalStatu> MaritalStatus { get; set; }
        public List<Relationship> Relationships { get; set; }
        public List<Religion> Religions { get; set; }
        public List<IdentificationType> IdentificationTypes { get; set; }
        public Patient Patient { get; set; }

    }
}