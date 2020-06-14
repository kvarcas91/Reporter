using Domain.Models;
using Domain.Models.Base;
using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataProcessor
{
    public static class WriteData
    {
        public static Response Write(string path, Action<string> setter, IEnumerable<ReportData> data, IEnumerable<RosterData> roster)
        {
            setter("Building Reports..");
            var extractedData = ExtractData.GetReports(data, roster);
            var response = new Response();

            var file = new Application
            {
                Visible = false,
                DisplayAlerts = false
            };

            var workBook = file.Workbooks.Add(Type.Missing);

            var swapSheet = (Worksheet)workBook.ActiveSheet;
            swapSheet.Name = "Swaps";

            var shortShiftSheet = (Worksheet)workBook.Worksheets.Add();
            shortShiftSheet.Name = "Short Shifts";

            var holidaySicknessSheet = (Worksheet)workBook.Worksheets.Add();
            holidaySicknessSheet.Name = "Holiday-Sickness";

            var headcountSheet = (Worksheet)workBook.Worksheets.Add();
            headcountSheet.Name = "HeadCount";


            return response;
        }

       
    }
}
