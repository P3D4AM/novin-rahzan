using System;
using System.Windows.Forms;

namespace DevComponents.DotNetBar;

public class DataConvertEventArgs : ConvertEventArgs
{
	private object _ᬐ;

	private string _ᬑ = "";

	public object DataItem => _ᬐ;

	public string FieldName => _ᬑ;

	public DataConvertEventArgs(object value, Type desiredType, object dataItem, string fieldName)
		: base(value, desiredType)
	{
		_ᬐ = dataItem;
		_ᬑ = fieldName;
	}
}
