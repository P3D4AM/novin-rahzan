using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;

namespace Microsoft.Data.SqlClient.Server;

internal class SmiOrderProperty : SmiMetaDataProperty
{
	internal struct SmiColumnOrder
	{
		internal int SortOrdinal;

		internal SortOrder Order;

		internal string TraceString()
		{
			return string.Format(CultureInfo.InvariantCulture, "{0} {1}", SortOrdinal, Order);
		}
	}

	private IList<SmiColumnOrder> _columns;

	internal SmiColumnOrder this[int ordinal]
	{
		get
		{
			if (_columns.Count <= ordinal)
			{
				SmiColumnOrder result = default(SmiColumnOrder);
				result.Order = SortOrder.Unspecified;
				result.SortOrdinal = -1;
				return result;
			}
			return _columns[ordinal];
		}
	}

	internal SmiOrderProperty(IList<SmiColumnOrder> columnOrders)
	{
		_columns = new List<SmiColumnOrder>(columnOrders).AsReadOnly();
	}

	[Conditional("DEBUG")]
	internal void CheckCount(int countToMatch)
	{
	}

	internal override string TraceString()
	{
		string text = "SortOrder(";
		bool flag = false;
		foreach (SmiColumnOrder column in _columns)
		{
			if (flag)
			{
				text += ",";
			}
			else
			{
				flag = true;
			}
			if (SortOrder.Unspecified != column.Order)
			{
				text += column.TraceString();
			}
		}
		return text + ")";
	}
}
