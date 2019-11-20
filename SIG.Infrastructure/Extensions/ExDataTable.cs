using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;

namespace SIG.Infrastructure.Extensions
{
    public static class ExDataTable
    {
        public static IList<T> ToList<T>(this DataTable table) where T : new()
        {
            if (table == null) return null;
            IList<PropertyInfo> properties = typeof(T).GetProperties().ToList();
            IList<T> result = new List<T>();

            foreach (var row in table.Rows)
            {
                var item = CreateItemFromRow<T>((DataRow)row, properties);
                result.Add(item);
            }

            return result;
        }

        private static T CreateItemFromRow<T>(DataRow row, IEnumerable<PropertyInfo> properties) where T : new()
        {
            T item = new T();
            foreach (var property in properties)
            {
                property.SetValue(item, row[property.Name], null);
            }
            return item;
        }
    }
}
