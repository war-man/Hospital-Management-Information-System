using CaresoftHMISDataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Caresoft2._0.Areas.Theatre
{
    public class Seeder
    {
        private CaresoftHMISEntities db = new CaresoftHMISEntities();
        public void setUpSettingsParameters()
        {

            //Theatre Marquee
            var marq = new KeyValuePair
            {
                Key_ = "TheatreMarquee",
                Value = "Enter Theatre Marquee",
                Owner = "Theatre"

            };
            if (db.KeyValuePairs.Where(e => e.Key_ == marq.Key_ && e.Owner == marq.Owner) == null)
            {
                db.KeyValuePairs.Add(marq);
            }

            var depId = new KeyValuePair
            {
                Key_ = "TheatreDepartmentName",
                Value = "Theatre",
                Owner = "Theatre"

            };
            if (db.KeyValuePairs.Where(e => e.Key_ == marq.Key_ && e.Owner == marq.Owner) == null)
            {
                db.KeyValuePairs.Add(depId);
            }

            int res = db.SaveChanges();
        }
    }
}