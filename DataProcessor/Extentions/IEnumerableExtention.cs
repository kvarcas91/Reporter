using Domain.Models;
using Domain.Models.Base;
using System.Collections.Generic;
using System.Linq;

namespace DataProcessor.Extentions
{
    public static class IEnumerableExtention
    {
        public static List<T> GetDistinct<T>(this List<T> data) where T : class, IEntity
        {
            var distinctData = data.GroupBy(x => x.EmployeeID).Select(y => y.First());
            var output = new List<T>();

            foreach (var item in distinctData)
            {
                output.Add(item);
            }
            return output;
        }

        public static IEnumerable<T> AddPattern<T>(this IEnumerable<T> data, IEnumerable<RosterData> roster) where T : class, IEmployeeData, IEntity
        {
            var distinctData = data.Distinct();
            var output = new List<T>();
            foreach (var item in data)
            {
                var department = roster.Where(empl => empl.EmployeeID.Equals(item.EmployeeID)).Select(x => x.DepartmentID).FirstOrDefault();
                var shiftPattern = roster.Where(empl => empl.EmployeeID.Equals(item.EmployeeID)).Select(x => x.ShiftPattern).FirstOrDefault();
                item.AddPattern(department, shiftPattern);
            }

            return data;
        }
    }
}
