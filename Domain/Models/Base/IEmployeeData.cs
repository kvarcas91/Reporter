namespace Domain.Models.Base
{
    public interface IEmployeeData
    {
        string DepartmentID { get; }
        string ShiftPattern { get; }

        void AddPattern(string departmentID, string ShiftPattern);
    }
}
