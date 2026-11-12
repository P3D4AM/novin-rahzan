using System.Collections.Generic;
using Microsoft.SqlServer.Management.SqlParser.Metadata;

namespace Microsoft.SqlServer.Management.SqlParser.MetadataSerialization;

internal class TypeDataTable
{
	public class Row
	{
		private readonly int id;

		private readonly ReadOnlyArray<int> values;

		public int Id => id;

		public ReadOnlyArray<int> Values => values;

		public Row(int id, int[] values)
		{
			this.id = id;
			this.values = new ReadOnlyArray<int>(values);
		}
	}

	private readonly MetadataTypeInfo typeInfo;

	private readonly string name;

	private readonly SortedList<int, Row> rows;

	public string Name => name;

	public MetadataTypeInfo TypeInfo => typeInfo;

	public IEnumerable<Row> Rows => rows.Values;

	public TypeDataTable(MetadataTypeInfo typeInfo)
	{
		this.typeInfo = typeInfo;
		rows = new SortedList<int, Row>();
		name = typeInfo.Type.Name.Substring(1);
	}

	public void AddRow(int id, int[] values)
	{
		rows.Add(id, new Row(id, values));
	}

	public Row GetRowById(int id)
	{
		return rows[id];
	}
}
