using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;

namespace Microsoft.SqlServer.Management.Assessment.Data.Morphs.Indexes;

internal class IndexDefinitionMorph : MultidimensionalCachedMorph
{
	[StructLayout(LayoutKind.Sequential, Size = 1)]
	public struct Field
	{
		public const string TableName = "TableName";

		public const string IndexName = "IndexName";

		public const string KeyCols = "KeyColumns";

		public const string IncludedCols = "IncludedColumns";

		public const string FilterDefinition = "FilterDefinition";

		public const string IsPrimaryKey = "IsPrimaryKey";

		public const string IsUnique = "IsUnique";

		public const string Type = "Type";

		public const string KeyColsTypes = "KeyColumnTypes";

		public const string DisplayName = "FullName";

		public const string KeySize = "KeySize";

		public const string IsDisabled = "IsDisabled";

		public const string FillFactor = "FillFactor";

		public const string Hits = "Hits";

		public const string ReadsRatio = "ReadsRatio";
	}

	private class IndexInfo
	{
		private string Name { get; }

		private string Table { get; }

		private string DisplayName => Table + "." + Name;

		private bool IsPrimaryKey { get; }

		private byte Type { get; }

		private bool IsUnique { get; }

		private byte FillFactor { get; }

		private bool IsDisabled { get; }

		private decimal Hits { get; }

		private decimal ReadsRatio { get; }

		private string FilterDefinition { get; }

		public List<IndexColumn> Columns { get; }

		private IEnumerable<string> KeyColumns => from c in Columns
			where !c.IsIncluded
			orderby c.KeyOriginal
			select c.Name;

		private IEnumerable<string> IncludedColumns => from c in Columns
			where c.IsIncluded
			orderby c.KeyOriginal
			select c.Name;

		private IEnumerable<string> KeyColumnOrderedTypes => from c in Columns
			where !c.IsIncluded
			orderby c.KeyOriginal
			select c.TypeName;

		private long KeyColsDataLengthBytes => Columns.Where((IndexColumn c) => c.KeyOriginal > 0).Sum((IndexColumn c) => c.Size);

		public IndexInfo(string table, string name, bool isPrimaryKey, byte type, byte fillFactor, bool isDisabled, bool isUnique, decimal hits, decimal readsRatio, string filterDefinition)
		{
			Table = table;
			Name = name;
			IsPrimaryKey = isPrimaryKey;
			Type = type;
			FillFactor = fillFactor;
			IsDisabled = isDisabled;
			IsUnique = isUnique;
			Hits = hits;
			ReadsRatio = readsRatio;
			FilterDefinition = filterDefinition;
			Columns = new List<IndexColumn>();
		}

		public Measurement ToMeasurement(string tableName)
		{
			return new Measurement
			{
				{ "IndexName", Name },
				{ "TableName", tableName },
				{ "IsPrimaryKey", IsPrimaryKey },
				{ "IsUnique", IsUnique },
				{ "Type", Type },
				{
					"KeyColumns",
					KeyColumns.ToArray()
				},
				{
					"IncludedColumns",
					IncludedColumns.ToArray()
				},
				{
					"KeyColumnTypes",
					KeyColumnOrderedTypes.ToArray()
				},
				{ "FullName", DisplayName },
				{ "KeySize", KeyColsDataLengthBytes },
				{ "FillFactor", FillFactor },
				{ "IsDisabled", IsDisabled },
				{ "Hits", Hits },
				{ "ReadsRatio", ReadsRatio },
				{ "FilterDefinition", FilterDefinition }
			};
		}
	}

	private class IndexColumn
	{
		public string Name { get; }

		public bool IsIncluded { get; }

		public byte KeyOriginal { get; }

		public string TypeName { get; }

		public short Size { get; }

		public IndexColumn(string name, bool isIncluded, byte ordinal, string typeName, short colTypeSize)
		{
			Name = name;
			IsIncluded = isIncluded;
			KeyOriginal = ordinal;
			TypeName = typeName;
			Size = colTypeSize;
		}
	}

	public static T GetRowData<T>(IDataRow row, string colName)
	{
		if (!row.TryGetData(colName, out var value))
		{
			throw new SqlAssessmentMissingDataException(colName);
		}
		if (value == null)
		{
			return default(T);
		}
		try
		{
			return (T)Convert.ChangeType(value, typeof(T), CultureInfo.InvariantCulture);
		}
		catch (InvalidCastException innerException)
		{
			throw new SqlAssessmentTypeMismatchException(typeof(T), value.GetType(), value, innerException);
		}
		catch (FormatException innerException2)
		{
			throw new SqlAssessmentFormatException(typeof(T), value.ToString(), innerException2);
		}
		catch (OverflowException innerException3)
		{
			throw new SqlAssessmentOverflowException(typeof(T), innerException3);
		}
	}

	public override IEnumerable<IDataRow> Transform(IDataView source)
	{
		Dictionary<string, Dictionary<string, IndexInfo>> dictionary = new Dictionary<string, Dictionary<string, IndexInfo>>();
		foreach (IDataRow item in source)
		{
			string rowData = GetRowData<string>(item, "TableName");
			if (!dictionary.TryGetValue(rowData, out var value))
			{
				value = (dictionary[rowData] = new Dictionary<string, IndexInfo>());
			}
			string rowData2 = GetRowData<string>(item, "IndexName");
			if (!value.TryGetValue(rowData2, out var value2))
			{
				value2 = (value[rowData2] = new IndexInfo(rowData, rowData2, GetRowData<bool>(item, "is_primary_key"), GetRowData<byte>(item, "IndexType"), GetRowData<byte>(item, "fill_factor"), GetRowData<bool>(item, "is_disabled"), GetRowData<bool>(item, "is_unique"), GetRowData<decimal>(item, "Hits"), GetRowData<decimal>(item, "Reads_Ratio"), GetRowData<string>(item, "filter_definition")));
			}
			value2.Columns.Add(new IndexColumn(GetRowData<string>(item, "ColumnName"), GetRowData<bool>(item, "is_included_column"), GetRowData<byte>(item, "key_ordinal"), GetRowData<string>(item, "ColTypeName"), GetRowData<short>(item, "ColTypeSize")));
		}
		return dictionary.SelectMany((KeyValuePair<string, Dictionary<string, IndexInfo>> tblInfo) => tblInfo.Value.Select((KeyValuePair<string, IndexInfo> indexInfo) => indexInfo.Value.ToMeasurement(tblInfo.Key)));
	}
}
