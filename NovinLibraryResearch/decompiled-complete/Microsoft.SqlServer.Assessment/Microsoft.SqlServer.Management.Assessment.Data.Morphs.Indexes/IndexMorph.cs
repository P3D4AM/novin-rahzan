using System.Collections.Generic;
using System.Linq;

namespace Microsoft.SqlServer.Management.Assessment.Data.Morphs.Indexes;

internal abstract class IndexMorph : MultidimensionalCachedMorph
{
	protected class IndexDetails
	{
		private readonly IDataRow row;

		public string Name => IndexDefinitionMorph.GetRowData<string>(row, "IndexName");

		public string FilterDefinition => IndexDefinitionMorph.GetRowData<string>(row, "FilterDefinition");

		public string[] KeyColumns => IndexDefinitionMorph.GetRowData<string[]>(row, "KeyColumns");

		public string[] IncludedColumns => IndexDefinitionMorph.GetRowData<string[]>(row, "IncludedColumns");

		public IEnumerable<string> IncludedColumnsOrdered => IncludedColumns.OrderBy((string c) => c);

		public string Definition => string.Join(",", KeyColumns) + "," + string.Join(",", IncludedColumns);

		public bool IsPrimaryKey => IndexDefinitionMorph.GetRowData<bool>(row, "IsPrimaryKey");

		public bool IsUnique => IndexDefinitionMorph.GetRowData<bool>(row, "IsUnique");

		public string OrderedColumns => string.Join(",", KeyColumns.OrderBy((string c) => c)) + "," + string.Join(",", IncludedColumns.OrderBy((string c) => c));

		public IEnumerable<string> KeyColumnsOrdered => KeyColumns.OrderBy((string c) => c);

		public string AllColumnsOrdered => string.Join(",", from c in KeyColumns.Union(IncludedColumns)
			orderby c
			select c);

		public IndexDetails(IDataRow dataRow)
		{
			row = dataRow;
		}

		public bool IsSimilarTo(IndexDetails i)
		{
			if (Name == i.Name || !IncludedColumns.Any() || !i.IncludedColumns.Any())
			{
				return false;
			}
			if (KeyColumns.SequenceEqual(i.KeyColumns) && IncludedColumns.SequenceEqual(i.IncludedColumns))
			{
				return false;
			}
			bool flag = KeyColumnsOrdered.SequenceEqual(i.KeyColumnsOrdered);
			bool flag2 = IncludedColumnsOrdered.SequenceEqual(i.IncludedColumnsOrdered);
			if ((!flag || !flag2) && (Definition == i.Definition || AllColumnsOrdered == i.AllColumnsOrdered))
			{
				return true;
			}
			if (flag != flag2)
			{
				return true;
			}
			if (AllColumnsOrdered == i.AllColumnsOrdered)
			{
				return FilterDefinition != i.FilterDefinition;
			}
			return false;
		}
	}

	protected abstract IEnumerable<Measurement> ProcessTableIndexes(IGrouping<string, IndexDetails> tableIndexes);

	protected static IEnumerable<IGrouping<string, IndexDetails>> Duplicates(IGrouping<string, IndexDetails> tableIndexes)
	{
		return from idx in tableIndexes
			group idx by idx.Definition + idx.OrderedColumns into gr
			where gr.Count() > 1
			select gr;
	}

	public override IEnumerable<IDataRow> Transform(IDataView source)
	{
		IEnumerable<IGrouping<string, IndexDetails>> source2 = from row in source
			group new IndexDetails(row) by IndexDefinitionMorph.GetRowData<string>(row, "TableName");
		return source2.SelectMany(ProcessTableIndexes);
	}
}
