using System.Collections.Generic;
using System.Data;
using System.Globalization;
using Microsoft.Data.Common;

namespace Microsoft.Data.ProviderBase;

internal sealed class FieldNameLookup
{
	private readonly string[] _fieldNames;

	private readonly int _defaultLocaleID;

	private Dictionary<string, int> _fieldNameLookup;

	private CompareInfo _compareInfo;

	public FieldNameLookup(string[] fieldNames, int defaultLocaleID)
	{
		_defaultLocaleID = defaultLocaleID;
		if (fieldNames == null)
		{
			throw ADP.ArgumentNull("fieldNames");
		}
		_fieldNames = fieldNames;
	}

	public FieldNameLookup(IDataReader reader, int defaultLocaleID)
	{
		_defaultLocaleID = defaultLocaleID;
		string[] array = new string[reader.FieldCount];
		for (int i = 0; i < array.Length; i++)
		{
			array[i] = reader.GetName(i);
		}
		_fieldNames = array;
	}

	public int GetOrdinal(string fieldName)
	{
		if (fieldName == null)
		{
			throw ADP.ArgumentNull("fieldName");
		}
		int num = IndexOf(fieldName);
		if (num == -1)
		{
			throw ADP.IndexOutOfRange(fieldName);
		}
		return num;
	}

	private int IndexOf(string fieldName)
	{
		if (_fieldNameLookup == null)
		{
			GenerateLookup();
		}
		if (!_fieldNameLookup.TryGetValue(fieldName, out var value))
		{
			value = LinearIndexOf(fieldName, CompareOptions.IgnoreCase);
			if (value == -1)
			{
				value = LinearIndexOf(fieldName, CompareOptions.IgnoreCase | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreWidth);
			}
		}
		return value;
	}

	private CompareInfo GetCompareInfo()
	{
		if (_defaultLocaleID != -1)
		{
			return CompareInfo.GetCompareInfo(_defaultLocaleID);
		}
		return CultureInfo.InvariantCulture.CompareInfo;
	}

	private int LinearIndexOf(string fieldName, CompareOptions compareOptions)
	{
		if (_compareInfo == null)
		{
			_compareInfo = GetCompareInfo();
		}
		for (int i = 0; i < _fieldNames.Length; i++)
		{
			if (_compareInfo.Compare(fieldName, _fieldNames[i], compareOptions) == 0)
			{
				_fieldNameLookup[fieldName] = i;
				return i;
			}
		}
		return -1;
	}

	private void GenerateLookup()
	{
		int num = _fieldNames.Length;
		Dictionary<string, int> dictionary = new Dictionary<string, int>(num);
		int num2 = num - 1;
		while (0 <= num2)
		{
			string key = _fieldNames[num2];
			dictionary[key] = num2;
			num2--;
		}
		_fieldNameLookup = dictionary;
	}
}
