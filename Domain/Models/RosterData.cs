using Domain.Models.Base;

namespace Domain.Models
{
    public class RosterData : IEntity, IAddable
    {

        public RosterData()
        {

        }

        public bool Add(string[] rosterData)
        {
            try
            {
                EmployeeID = rosterData[0];
                UserID = rosterData[1];
                EmployeeName = rosterData[2];
                BadgeID = rosterData[3];
                DepartmentID = rosterData[4];
                EmploymentStartDate = rosterData[5];
                EmploymentType = rosterData[6];
                EmployeeStatus = rosterData[7];
                ManagerName = rosterData[8];
                AgencyCode = rosterData[9];
                ShiftPattern = rosterData[10];

                return true;
            }
            catch
            {
                return false;
            }
        }

        public string EmployeeID { get; private set; }
        public string UserID { get; private set; }
        public string EmployeeName { get; private set; }
        public string BadgeID { get; private set; }
        public string DepartmentID { get; private set; }
        public string EmploymentStartDate { get; private set; }
        public string EmploymentType { get; private set; }
        public string EmployeeStatus { get; private set; }
        public string ManagerName { get; private set; }
        public string AgencyCode { get; private set; }
        public string ShiftPattern { get; private set; }

       

        public override string ToString()
        {
            return $"{EmployeeID}\t{UserID}\t{EmployeeName}\t{BadgeID}\t{DepartmentID}\t{EmploymentStartDate}\t{EmploymentType}\t{EmployeeStatus}\t{ManagerName}\t{AgencyCode}\t{ShiftPattern}\n";
        }
    }
}
