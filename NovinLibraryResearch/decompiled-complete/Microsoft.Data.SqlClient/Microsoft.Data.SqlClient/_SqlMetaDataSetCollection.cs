using System;
using System.Collections.Generic;

namespace Microsoft.Data.SqlClient;

internal sealed class _SqlMetaDataSetCollection : ICloneable
{
	private readonly List<_SqlMetaDataSet> altMetaDataSetArray;

	internal _SqlMetaDataSet metaDataSet;

	internal _SqlMetaDataSetCollection()
	{
		altMetaDataSetArray = new List<_SqlMetaDataSet>();
	}

	internal void SetAltMetaData(_SqlMetaDataSet altMetaDataSet)
	{
		int id = altMetaDataSet.id;
		for (int i = 0; i < altMetaDataSetArray.Count; i++)
		{
			if (altMetaDataSetArray[i].id == id)
			{
				altMetaDataSetArray[i] = altMetaDataSet;
				return;
			}
		}
		altMetaDataSetArray.Add(altMetaDataSet);
	}

	internal _SqlMetaDataSet GetAltMetaData(int id)
	{
		foreach (_SqlMetaDataSet item in altMetaDataSetArray)
		{
			if (item.id == id)
			{
				return item;
			}
		}
		return null;
	}

	public object Clone()
	{
		_SqlMetaDataSetCollection sqlMetaDataSetCollection = new _SqlMetaDataSetCollection();
		sqlMetaDataSetCollection.metaDataSet = ((metaDataSet == null) ? null : ((_SqlMetaDataSet)metaDataSet.Clone()));
		foreach (_SqlMetaDataSet item in altMetaDataSetArray)
		{
			sqlMetaDataSetCollection.altMetaDataSetArray.Add((_SqlMetaDataSet)item.Clone());
		}
		return sqlMetaDataSetCollection;
	}
}
