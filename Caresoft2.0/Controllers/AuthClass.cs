using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CaresoftHMISDataAccess;

namespace Caresoft2._0.Controllers
{
    public class AuthClass
    {
        private CaresoftHMISEntities db = new CaresoftHMISEntities();

        public String getAuthToken(int userId)
        {

            return "ZwerEodfdggHcXXtRE67asdDEpokKLSA";
        }

        public User getThisUser(string authToken)
        {
            var entity = db.Users.FirstOrDefault(e => e.AuthToken.Equals(authToken));
            if (entity != null)
            {
                return entity;
            }
            else
            {
                return null;
            }
        }


    }
}