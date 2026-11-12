using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.SqlServer.Management.Sdk.Sfc;

namespace Microsoft.SqlServer.Management.Smo;

internal class SmoCollectionCompararer : IComparer<ISfcSimpleNode>
{
	private IComparer comparer;

	internal SmoCollectionCompararer(IComparer comparer)
	{
		if (comparer == null)
		{
			throw new ArgumentNullException("comparer");
		}
		this.comparer = comparer;
	}

	public int Compare(ISfcSimpleNode left, ISfcSimpleNode right)
	{
		if (left == null && right == null)
		{
			return 0;
		}
		if (left == null)
		{
			return -1;
		}
		if (right == null)
		{
			return 1;
		}
		SqlSmoObject sqlSmoObject = left.ObjectReference as SqlSmoObject;
		SqlSmoObject obj = right.ObjectReference as SqlSmoObject;
		ObjectKeyBase key = sqlSmoObject.key;
		ObjectKeyBase key2 = obj.key;
		return ((IComparer)key.GetComparer(comparer)).Compare((object?)key, (object?)key2);
	}
}
