using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;

namespace Microsoft.Data.SqlClient.Server;

internal class SmiUniqueKeyProperty : SmiMetaDataProperty
{
	private IList<bool> _columns;

	internal bool this[int ordinal]
	{
		get
		{
			if (_columns.Count <= ordinal)
			{
				return false;
			}
			return _columns[ordinal];
		}
	}

	internal SmiUniqueKeyProperty(IList<bool> columnIsKey)
	{
		_columns = new List<bool>(columnIsKey).AsReadOnly();
	}

	[Conditional("DEBUG")]
	internal void CheckCount(int countToMatch)
	{
	}

	internal override string TraceString()
	{
		string text = "UniqueKey(";
		bool flag = false;
		for (int i = 0; i < _columns.Count; i++)
		{
			if (flag)
			{
				text += ",";
			}
			else
			{
				flag = true;
			}
			if (_columns[i])
			{
				text += i.ToString(CultureInfo.InvariantCulture);
			}
		}
		return text + ")";
	}
}
