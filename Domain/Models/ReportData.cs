using Domain.Models.Base;
using System;

namespace Domain.Models
{
    public class ReportData : IEntity, IAddable, IEmployeeData
    {
        public ReportData()
        {
           
        }

        public bool Add(string[] data)
        {
            try
            {
                Name = data[0];
                EmployeeID = data[1];
                ScheduledShiftHours = Convert.ToDouble(data[2]);
                RowType = data[3];
                Day = data[4];
                PrimaryJob = data[5];
                ShiftStartDate = data[6];
                ShiftStartTime = data[7];
                ShiftEndTime = data[8];
                ShiftEndDate = data[9];
                SegmentedStartDate = data[10];
                Type = data[11];
                SegmentedStartTime = data[12];
                SegmentedEndTime = data[13];
                SegmentedEndDate = data[14];
                ScheduledHours = data[15];
                PayCode = data[16];
                Amount = data[17];
                SymbolicValue = data[18];
                OverrideAccrualDays = data[19];
                TransferJob = data[20];
                TransferLaborAccount = data[21];
                TransferWorkRule = data[22];
                Comments = data[23];
                Notes = data[24];
                ShiftLabel = data[25];
                JobCovered = data[26];
                return true;
            }
            catch
            {
                return false;
            }
        }

        public void AddPattern (string departmentID, string shiftPattern)
        {
            DepartmentID = departmentID;
            ShiftPattern = shiftPattern;
        }

        public string Name { get; private set; }
        public string EmployeeID { get; private set; }
        public double ScheduledShiftHours { get; private set; }
        public string RowType { get; private set; }
        public string Day { get; private set; }
        public string PrimaryJob { get; private set; }
        public string ShiftStartDate { get; private set; }
        public string ShiftStartTime { get; private set; }
        public string ShiftEndTime { get; private set; }
        public string ShiftEndDate { get; private set; }
        public string SegmentedStartDate { get; private set; }
        public string Type { get; private set; }
        public string SegmentedStartTime { get; private set; }
        public string SegmentedEndTime { get; private set; }
        public string SegmentedEndDate { get; private set; }
        public string ScheduledHours { get; private set; }
        public string PayCode { get; private set; }
        public string Amount { get; private set; }
        public string SymbolicValue { get; private set; }
        public string OverrideAccrualDays { get; private set; }
        public string TransferJob { get; private set; }
        public string TransferLaborAccount { get; private set; }
        public string TransferWorkRule { get; private set; }
        public string Comments { get; private set; }
        public string Notes { get; private set; }
        public string ShiftLabel { get; private set; }
        public string JobCovered { get; private set; }
        public string DepartmentID { get; private set; }
        public string ShiftPattern { get; private set; }

        public override string ToString()
        {
            return $"{Name}\t{EmployeeID}\t{ScheduledShiftHours}\t{RowType}\t{Day}\t{PrimaryJob}\t{ShiftStartDate}\t{ShiftStartTime}\t{ShiftEndTime}\t{ShiftEndDate}\t{SegmentedEndTime}\t{ SegmentedEndDate}\t{ ScheduledHours}\t{ PayCode}\t{ Amount}\t{ SymbolicValue}\t{ OverrideAccrualDays}\t{ TransferJob}\t{ TransferLaborAccount}\t{ TransferWorkRule}\t{ Comments}\t{ Notes}\t{ ShiftLabel}\t{ JobCovered}\n";
        }
    
    }
}
