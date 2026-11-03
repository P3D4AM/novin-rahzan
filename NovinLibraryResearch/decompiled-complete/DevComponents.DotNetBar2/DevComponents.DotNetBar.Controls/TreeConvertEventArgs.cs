using System;
using System.Windows.Forms;

namespace DevComponents.DotNetBar.Controls;

public class TreeConvertEventArgs : ConvertEventArgs
{
	private object __2705;

	private string _ᬑ = "";

	public object ListItem => __2705;

	public string FieldName => _ᬑ;

	public TreeConvertEventArgs(object value, Type desiredType, object listItem, string fieldName)
		: base(value, desiredType)
	{
		__2705 = listItem;
		_ᬑ = fieldName;
	}
}
