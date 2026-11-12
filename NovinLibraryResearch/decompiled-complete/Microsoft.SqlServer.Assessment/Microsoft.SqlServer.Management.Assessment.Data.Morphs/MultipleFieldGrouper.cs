using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Microsoft.SqlServer.Management.Assessment.Properties;

namespace Microsoft.SqlServer.Management.Assessment.Data.Morphs;

internal class MultipleFieldGrouper : Grouper
{
	private class GroupsComparer : IEqualityComparer<object[]>
	{
		public static GroupsComparer Instance { get; } = new GroupsComparer();


		private GroupsComparer()
		{
		}

		public bool Equals(object[] x, object[] y)
		{
			return StructuralComparisons.StructuralEqualityComparer.Equals(x, y);
		}

		public int GetHashCode(object[] obj)
		{
			return StructuralComparisons.StructuralEqualityComparer.GetHashCode(obj);
		}
	}

	internal string[] FieldNames;

	public MultipleFieldGrouper(IEnumerable<string> fields)
	{
		FieldNames = fields?.ToArray() ?? Array.Empty<string>();
		if (fields.Contains(null))
		{
			throw new ArgumentException(Resources.GroupingColumnNameCannotBeNull);
		}
	}

	public override IEnumerable<DataRowGroup> Transform(IDataView source)
	{
		if (source == null || !source.Any() || source.Any((IDataRow s) => s == null))
		{
			return Enumerable.Empty<DataRowGroup>();
		}
		Dictionary<object[], DataRowGroup> dictionary = new Dictionary<object[], DataRowGroup>(GroupsComparer.Instance);
		foreach (IDataRow item in source)
		{
			object[] array = new object[FieldNames.Length];
			for (int i = 0; i < FieldNames.Length; i++)
			{
				if (item.TryGetData(FieldNames[i], out var value))
				{
					array[i] = value;
				}
				else
				{
					array[i] = null;
				}
			}
			if (!dictionary.TryGetValue(array, out var value2))
			{
				Measurement measurement = new Measurement(FieldNames.Length);
				for (int j = 0; j < FieldNames.Length; j++)
				{
					measurement.Add(FieldNames[j], array[j]);
				}
				DataRowGroup dataRowGroup = default(DataRowGroup);
				dataRowGroup.Result = measurement;
				value2 = dataRowGroup;
				dictionary.Add(array, value2);
			}
			if (value2.Data == null)
			{
				value2.Data = new List<IDataRow>();
				dictionary[array] = value2;
			}
			((IList<IDataRow>)value2.Data).Add(item);
		}
		return dictionary.Values;
	}
}
