using System;
using System.Collections;
using Microsoft.Data.Common;

namespace Microsoft.Data.SqlClient;

public sealed class SqlBulkCopyColumnMappingCollection : CollectionBase
{
	private enum MappingSchema
	{
		Undefined,
		NamesNames,
		NemesOrdinals,
		OrdinalsNames,
		OrdinalsOrdinals
	}

	private bool _readOnly;

	private MappingSchema _mappingSchema;

	public SqlBulkCopyColumnMapping this[int index] => (SqlBulkCopyColumnMapping)base.List[index];

	internal bool ReadOnly
	{
		get
		{
			return _readOnly;
		}
		set
		{
			_readOnly = value;
		}
	}

	internal SqlBulkCopyColumnMappingCollection()
	{
	}

	public SqlBulkCopyColumnMapping Add(SqlBulkCopyColumnMapping bulkCopyColumnMapping)
	{
		AssertWriteAccess();
		if ((ADP.IsEmpty(bulkCopyColumnMapping.SourceColumn) && bulkCopyColumnMapping.SourceOrdinal == -1) || (ADP.IsEmpty(bulkCopyColumnMapping.DestinationColumn) && bulkCopyColumnMapping.DestinationOrdinal == -1))
		{
			throw SQL.BulkLoadNonMatchingColumnMapping();
		}
		base.InnerList.Add(bulkCopyColumnMapping);
		return bulkCopyColumnMapping;
	}

	public SqlBulkCopyColumnMapping Add(string sourceColumn, string destinationColumn)
	{
		AssertWriteAccess();
		SqlBulkCopyColumnMapping bulkCopyColumnMapping = new SqlBulkCopyColumnMapping(sourceColumn, destinationColumn);
		return Add(bulkCopyColumnMapping);
	}

	public SqlBulkCopyColumnMapping Add(int sourceColumnIndex, string destinationColumn)
	{
		AssertWriteAccess();
		SqlBulkCopyColumnMapping bulkCopyColumnMapping = new SqlBulkCopyColumnMapping(sourceColumnIndex, destinationColumn);
		return Add(bulkCopyColumnMapping);
	}

	public SqlBulkCopyColumnMapping Add(string sourceColumn, int destinationColumnIndex)
	{
		AssertWriteAccess();
		SqlBulkCopyColumnMapping bulkCopyColumnMapping = new SqlBulkCopyColumnMapping(sourceColumn, destinationColumnIndex);
		return Add(bulkCopyColumnMapping);
	}

	public SqlBulkCopyColumnMapping Add(int sourceColumnIndex, int destinationColumnIndex)
	{
		AssertWriteAccess();
		SqlBulkCopyColumnMapping bulkCopyColumnMapping = new SqlBulkCopyColumnMapping(sourceColumnIndex, destinationColumnIndex);
		return Add(bulkCopyColumnMapping);
	}

	private void AssertWriteAccess()
	{
		if (ReadOnly)
		{
			throw SQL.BulkLoadMappingInaccessible();
		}
	}

	public new void Clear()
	{
		AssertWriteAccess();
		base.Clear();
	}

	public bool Contains(SqlBulkCopyColumnMapping value)
	{
		return -1 != base.InnerList.IndexOf(value);
	}

	public void CopyTo(SqlBulkCopyColumnMapping[] array, int index)
	{
		base.InnerList.CopyTo(array, index);
	}

	internal void CreateDefaultMapping(int columnCount)
	{
		for (int i = 0; i < columnCount; i++)
		{
			base.InnerList.Add(new SqlBulkCopyColumnMapping(i, i));
		}
	}

	public int IndexOf(SqlBulkCopyColumnMapping value)
	{
		return base.InnerList.IndexOf(value);
	}

	public void Insert(int index, SqlBulkCopyColumnMapping value)
	{
		AssertWriteAccess();
		base.InnerList.Insert(index, value);
	}

	public void Remove(SqlBulkCopyColumnMapping value)
	{
		AssertWriteAccess();
		base.InnerList.Remove(value);
	}

	public new void RemoveAt(int index)
	{
		AssertWriteAccess();
		base.RemoveAt(index);
	}

	internal void ValidateCollection()
	{
		IEnumerator enumerator = GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				SqlBulkCopyColumnMapping sqlBulkCopyColumnMapping = (SqlBulkCopyColumnMapping)enumerator.Current;
				MappingSchema mappingSchema = ((sqlBulkCopyColumnMapping.SourceOrdinal != -1) ? ((sqlBulkCopyColumnMapping.DestinationOrdinal == -1) ? MappingSchema.OrdinalsNames : MappingSchema.OrdinalsOrdinals) : ((sqlBulkCopyColumnMapping.DestinationOrdinal == -1) ? MappingSchema.NamesNames : MappingSchema.NemesOrdinals));
				if (_mappingSchema == MappingSchema.Undefined)
				{
					_mappingSchema = mappingSchema;
				}
				else if (_mappingSchema != mappingSchema)
				{
					throw SQL.BulkLoadMappingsNamesOrOrdinalsOnly();
				}
			}
		}
		finally
		{
			IDisposable disposable = enumerator as IDisposable;
			if (disposable != null)
			{
				disposable.Dispose();
			}
		}
	}
}
