using Domain.Models;
using Domain.Models.Base;
using Microsoft.VisualBasic.FileIO;
using System.Collections.Generic;
using System.Linq;

namespace DataProcessor
{
    public static class ExtractData
    {
        public static (IEnumerable<T>, IEnumerable<string[]>, string[]) GetData<T>(string path) where T : class, IAddable, new()
        {
            var dataList = new List<T>();
            var errorList = new List<string[]>();
            string[] headers;

            using (TextFieldParser csvParser = new TextFieldParser(path))
            {
                csvParser.CommentTokens = new string[] { "#" };
                csvParser.SetDelimiters(new string[] { "," });
                csvParser.HasFieldsEnclosedInQuotes = true;

                csvParser.CommentTokens = new string[] { "#" };
                csvParser.SetDelimiters(new string[] { "," });
                csvParser.HasFieldsEnclosedInQuotes = true;

                headers = csvParser.ReadLine().Replace("\"", "").Replace(" ", "").Split(',');
                if (headers.Length == 0) errorList.Add(new string[] { "issue with getting header data" });

                while (!csvParser.EndOfData)
                {

                    // Read current line fields, pointer moves to the next line.
                    string[] fields = csvParser.ReadFields();
                    var data = new T();
                    var results = data.Add(fields);
                    if (results) dataList.Add(data);
                    else errorList.Add(fields);
                }
            }
            return (dataList, errorList, headers);
        }

        public static IEnumerable<List<ReportData>> GetReports(IEnumerable<ReportData> data, IEnumerable<RosterData> roster)
        {
            var output = new List<List<ReportData>>();

            var swaps = new List<ReportData>();
            var shortShifts = new List<ReportData>();
            var holidaySickness = new List<ReportData>();
            var headcount = new List<ReportData>();

            foreach (var item in data)
            {

                // Get swaps
                if (item.Notes.ToLower().Contains("swap"))
                {
                    swaps.Add(item);
                }

                // Get Short Shifts
                if (string.IsNullOrEmpty(item.PayCode) && (item.ScheduledShiftHours > 0 && item.ScheduledShiftHours < 10))
                {
                    shortShifts.Add(item);
                }

                if (!string.IsNullOrEmpty(item.PayCode))
                {
                    holidaySickness.Add(item);
                }

                if (item.ScheduledShiftHours > 0 && string.IsNullOrEmpty(item.PayCode))
                {
                    headcount.Add(item);
                }

            }
            //output.Add(Sort<ReportData>.ByEmployeeID(swaps).ToList());
            var enumerable = swaps.Distinct();
            output.Add(enumerable.ToList());
            output.Add(shortShifts);
            output.Add(holidaySickness.OrderBy(x => x.PayCode).ToList());
            //output.Add(Sort<ReportData>.ByEmployeeIDWithDepartmentAndShift(headcount, roster).ToList());
            return output;
        }

    }
}
