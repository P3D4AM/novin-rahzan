using System;
using System.Windows.Forms;

namespace Stimulsoft.Controls.Win.DotNetBar;

public class DataConvertEventArgs : ConvertEventArgs
{
	private object _DataItem;

	private string _FieldName = "";

	public object DataItem => _DataItem;

	public string FieldName => _FieldName;

	public DataConvertEventArgs(object value, Type desiredType, object dataItem, string fieldName)
		: base(value, desiredType)
	{
		_DataItem = dataItem;
		_FieldName = fieldName;
	}
}
