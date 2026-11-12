using System;
using System.Data;

namespace Microsoft.Data.SqlClient;

internal sealed class _SqlMetaDataSet : ICloneable
{
	internal ushort id;

	internal int[] indexMap;

	internal int visibleColumns;

	internal DataTable schemaTable;

	internal readonly SqlTceCipherInfoTable cekTable;

	internal readonly _SqlMetaData[] metaDataArray;

	internal int Length => metaDataArray.Length;

	internal _SqlMetaData this[int index]
	{
		get
		{
			return metaDataArray[index];
		}
		set
		{
			metaDataArray[index] = value;
		}
	}

	internal _SqlMetaDataSet(int count, SqlTceCipherInfoTable cipherTable)
	{
		cekTable = cipherTable;
		metaDataArray = new _SqlMetaData[count];
		for (int i = 0; i < metaDataArray.Length; i++)
		{
			metaDataArray[i] = new _SqlMetaData(i);
		}
	}

	private _SqlMetaDataSet(_SqlMetaDataSet original)
	{
		id = original.id;
		indexMap = original.indexMap;
		visibleColumns = original.visibleColumns;
		schemaTable = original.schemaTable;
		if (original.metaDataArray == null)
		{
			metaDataArray = null;
			return;
		}
		metaDataArray = new _SqlMetaData[original.metaDataArray.Length];
		for (int i = 0; i < metaDataArray.Length; i++)
		{
			metaDataArray[i] = (_SqlMetaData)original.metaDataArray[i].Clone();
		}
	}

	public object Clone()
	{
		return new _SqlMetaDataSet(this);
	}
}
