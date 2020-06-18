using Domain.Models;
using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DataProcessor
{
    public static class WriteData
    {

        public static Response Write (string path, Action<string> setter, List<ReportData> swapData, List<ReportData> shortShifts, List<ReportData> holidaySickness, List<ReportData> headCount)
        {

            var file = new Application
            {
                Visible = false,
                DisplayAlerts = false
            };

            setter("Creating spreadsheet...");

            var workBook = file.Workbooks.Add(Type.Missing);

            var swapSheet = (Worksheet)workBook.ActiveSheet;
            swapSheet.Name = "Swaps";

            var shortShiftSheet = (Worksheet)workBook.Worksheets.Add();
            shortShiftSheet.Name = "Short Shifts";

            var holidaySicknessSheet = (Worksheet)workBook.Worksheets.Add();
            holidaySicknessSheet.Name = "Holiday-Sickness";

            var headcountSheet = (Worksheet)workBook.Worksheets.Add();
            headcountSheet.Name = "HeadCount";

            setter("Writing Swap data...");
            var swapResponse = WriteSwapData(swapSheet, swapData);
            if (!string.IsNullOrEmpty(swapResponse))
            {
                Close(workBook, file);
                return GetResponse("Swap", swapResponse);
            }

            setter("Writing Short shifts data...");
            var shortShiftResponse = WriteOBShortShiftData(shortShiftSheet, shortShifts);
            if (!string.IsNullOrEmpty(shortShiftResponse))
            {
                Close(workBook, file);
                return GetResponse("Short shifts", shortShiftResponse);
            }

            setter("Writing Holiday/Sickness data...");
            var hsResponse = WriteHolidaySicknessData(holidaySicknessSheet, holidaySickness);
            if (!string.IsNullOrEmpty(hsResponse))
            {
                Close(workBook, file);
                return GetResponse("Holidays / Sickness", hsResponse);
            }

            setter("Writing Headcount data...");
            var headcount = WriteOBHeadcountData(headcountSheet, headCount);
            if (!string.IsNullOrEmpty(headcount))
            {
                Close(workBook, file);
                return GetResponse("Headcount", headcount);
            }

            setter("Saving file...");
            var save = Save(workBook, path);
            if (!string.IsNullOrEmpty(headcount))
            {
                Close(workBook, file);
                return GetResponse("'Save File'", headcount);
            }

            Close(workBook, file);

            return new Response { Success = true };

        }

        public static Response Write(string path, Action<string> setter, IEnumerable<ReportData> data, IEnumerable<RosterData> roster)
        {
            setter("Building Reports...");
            var extractedData = ExtractData.GetReports(data, roster).ToList();

            var swapData = extractedData[0];
            var shortShifts = extractedData[1];
            var holidaySickness = extractedData[2];
            var headCount = extractedData[3];

            return Write(path, setter, swapData, shortShifts, holidaySickness, headCount);
        }

        private static string WriteSwapData (Worksheet workSheet, List<ReportData> data)
        {
            workSheet.Cells[1, 1] = "Employee Name";
            workSheet.Cells[1, 2] = "Employee ID";
            workSheet.Cells[1, 3] = "Day";
            workSheet.Cells[1, 4] = "Notes";

            var columnHeadingsRange = workSheet.Range[workSheet.Cells[1, 1], workSheet.Cells[1, 4]];
            SetHeaderColor(columnHeadingsRange);
            SetBorder(columnHeadingsRange);

            var row = 2;
            try
            {
                foreach (var item in data)
                {
                    workSheet.Cells[row, 1] = item.Name;
                    workSheet.Cells[row, 2] = item.EmployeeID;
                    workSheet.Cells[row, 3] = item.Day;
                    workSheet.Cells[row, 4] = item.Notes;
                    var cellRange = workSheet.Range[workSheet.Cells[row, 1], workSheet.Cells[row, 4]];
                    SetBorder(cellRange);
                    row++;
                }
                return null;
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }

        private static string WriteOBShortShiftData(Worksheet workSheet, List<ReportData> data)
        {
            workSheet.Cells[1, 1] = "Employee Name";
            workSheet.Cells[1, 2] = "Employee ID";
            workSheet.Cells[1, 3] = "Day";
            workSheet.Cells[1, 4] = "Sch Hrs.";
            workSheet.Cells[1, 5] = "Shift Start Date";
            workSheet.Cells[1, 6] = "Shift Start Time";
            workSheet.Cells[1, 7] = "Shift End Time";

            var columnHeadingsRange = workSheet.Range[workSheet.Cells[1, 1], workSheet.Cells[1, 7]];
            SetHeaderColor(columnHeadingsRange);
            SetBorder(columnHeadingsRange);

            var row = 2;
            try
            {
                foreach (var item in data)
                {
                    workSheet.Cells[row, 1] = item.Name;
                    workSheet.Cells[row, 2] = item.EmployeeID;
                    workSheet.Cells[row, 3] = item.Day;
                    workSheet.Cells[row, 4] = item.ScheduledShiftHours;
                    workSheet.Cells[row, 5] = item.ShiftStartDate;
                    workSheet.Cells[row, 6] = item.ShiftStartTime;
                    workSheet.Cells[row, 7] = item.ShiftEndTime;

                    var cellRange = workSheet.Range[workSheet.Cells[row, 1], workSheet.Cells[row, 7]];
                    SetBorder(cellRange);
                    row++;
                   
                }
                return null;
            }
            catch(Exception e)
            {
                return e.Message;
            }
           
        }

        private static string WriteHolidaySicknessData(Worksheet workSheet, List<ReportData> data)
        {
            workSheet.Cells[1, 1] = "Employee Name";
            workSheet.Cells[1, 2] = "Employee ID";
            workSheet.Cells[1, 3] = "Day";
            workSheet.Cells[1, 4] = "Segmented Start Time";
            workSheet.Cells[1, 5] = "Pay Code";
            workSheet.Cells[1, 6] = "Amount";

            var columnHeadingsRange = workSheet.Range[workSheet.Cells[1, 1], workSheet.Cells[1, 6]];
            SetHeaderColor(columnHeadingsRange);
            SetBorder(columnHeadingsRange);

            var row = 2;
            try
            {
                foreach (var item in data)
                {
                    workSheet.Cells[row, 1] = item.Name;
                    workSheet.Cells[row, 2] = item.EmployeeID;
                    workSheet.Cells[row, 3] = item.Day;
                    workSheet.Cells[row, 4] = item.SegmentedStartTime;
                    workSheet.Cells[row, 5] = item.PayCode;
                    workSheet.Cells[row, 6] = item.Amount;

                    var cellRange = workSheet.Range[workSheet.Cells[row, 1], workSheet.Cells[row, 6]];
                    SetBorder(cellRange);
                    row++;
                }
                return null;
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }

        private static string WriteOBHeadcountData(Worksheet workSheet, List<ReportData> data)
        {
            workSheet.Cells[1, 1] = "Employee ID";
            workSheet.Cells[1, 2] = "Name";
            workSheet.Cells[1, 3] = "Shift Start Time";
            workSheet.Cells[1, 4] = "Department";
            workSheet.Cells[1, 5] = "Shift";

            var columnHeadingsRange = workSheet.Range[workSheet.Cells[1, 1], workSheet.Cells[1, 5]];
            SetHeaderColor(columnHeadingsRange);
            SetBorder(columnHeadingsRange);

            var row = 2;
            try
            {
                foreach (var item in data)
                {
                    workSheet.Cells[row, 1] = item.EmployeeID;
                    workSheet.Cells[row, 2] = item.Name;
                    workSheet.Cells[row, 3] = item.ShiftStartTime;
                    workSheet.Cells[row, 4] = item.DepartmentID;
                    workSheet.Cells[row, 5] = item.ShiftPattern;

                    var cellRange = workSheet.Range[workSheet.Cells[row, 1], workSheet.Cells[row, 5]];
                    SetBorder(cellRange);
                    row++;
                }
                return null;
            }
            catch (Exception e)
            {
                return e.Message;
            }

            
        }

        private static string Save(Workbook workBook, string path)
        {

            try
            {
                workBook.SaveAs(path);
                return null;
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }

        private static void SetBorder(dynamic range)
        {
            range.EntireColumn.AutoFit();
            Borders border = range.Borders;
            border.LineStyle = XlLineStyle.xlContinuous;
            border.Weight = 2d;
        }

        private static void SetHeaderColor(dynamic range)
        {
            ;
            range.Interior.Color = XlRgbColor.rgbLightSteelBlue;
        }

        private static void Close (Workbook workBook, Application file)
        {
            workBook.Close();
            file.Quit();

        }

        private static Response GetResponse(string process, string message, bool success = false)
        {
            return new Response
            {
                Success = success,
                Message = $"Failed to write {process} data with error: {message}"
            };
        }


    }
}
