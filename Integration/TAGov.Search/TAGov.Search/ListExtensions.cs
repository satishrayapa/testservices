using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;

namespace TAGov.Search
{
	public static class ListExtensions
	{
		public static DataTable ToDataTable<T>(this IList<T> items)
		{
			DataTable dataTable = new DataTable(typeof(T).Name);

			PropertyInfo[] props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
			foreach (PropertyInfo prop in props)
			{
				var type = prop.PropertyType.IsGenericType && prop.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>) ? Nullable.GetUnderlyingType(prop.PropertyType) : prop.PropertyType;
				// ReSharper disable once AssignNullToNotNullAttribute
				dataTable.Columns.Add(prop.Name, type);
			}
			foreach (T item in items)
			{
				var values = new object[props.Length];
				for (int i = 0; i < props.Length; i++)
				{
					values[i] = props[i].GetValue(item, null);
				}
				dataTable.Rows.Add(values);
			}

			return dataTable;
		}
	}
}
