using System;
using System.Collections;
using System.Configuration;

namespace Microsoft.Data;

internal sealed class LocalDBInstancesCollection : ConfigurationElementCollection
{
	private class TrimOrdinalIgnoreCaseStringComparer : IComparer
	{
		public int Compare(object x, object y)
		{
			if (x is string text)
			{
				x = text.Trim();
			}
			if (y is string text2)
			{
				y = text2.Trim();
			}
			return StringComparer.OrdinalIgnoreCase.Compare(x, y);
		}
	}

	private static readonly TrimOrdinalIgnoreCaseStringComparer s_comparer = new TrimOrdinalIgnoreCaseStringComparer();

	internal LocalDBInstancesCollection()
		: base((IComparer)s_comparer)
	{
	}

	protected override ConfigurationElement CreateNewElement()
	{
		return (ConfigurationElement)(object)new LocalDBInstanceElement();
	}

	protected override object GetElementKey(ConfigurationElement element)
	{
		return ((LocalDBInstanceElement)(object)element).Name;
	}
}
