using System.Collections.Generic;
using System.Diagnostics;

namespace Microsoft.Data.SqlClient.Server;

internal class SmiDefaultFieldsProperty : SmiMetaDataProperty
{
	private IList<bool> _defaults;

	internal bool this[int ordinal]
	{
		get
		{
			if (_defaults.Count <= ordinal)
			{
				return false;
			}
			return _defaults[ordinal];
		}
	}

	internal SmiDefaultFieldsProperty(IList<bool> defaultFields)
	{
		_defaults = new List<bool>(defaultFields).AsReadOnly();
	}

	[Conditional("DEBUG")]
	internal void CheckCount(int countToMatch)
	{
	}

	internal override string TraceString()
	{
		string text = "DefaultFields(";
		bool flag = false;
		for (int i = 0; i < _defaults.Count; i++)
		{
			if (flag)
			{
				text += ",";
			}
			else
			{
				flag = true;
			}
			if (_defaults[i])
			{
				text += i;
			}
		}
		return text + ")";
	}
}
