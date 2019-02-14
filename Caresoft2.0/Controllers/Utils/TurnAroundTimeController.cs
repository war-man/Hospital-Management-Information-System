using CaresoftHMISDataAccess;

namespace Caresoft2._0.Controllers.Utils
{
    public class TurnAroundTime
    {
        private CaresoftHMISEntities db = new CaresoftHMISEntities();

        // GET: TurnAroundTime
        public int insert(PatientTurnAroundTime aroundTime)
        {
            db.PatientTurnAroundTimes.Add(aroundTime);
            var res = db.SaveChanges();
            return res;
        }
    }
}