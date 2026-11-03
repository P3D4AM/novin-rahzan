using System;
using System.Windows.Forms;

namespace DevComponents.DotNetBar;

public class CheckStateConvertEventArgs : EventArgs
{
	public readonly object Value;

	public CheckState? CheckState = null;

	public CheckStateConvertEventArgs(object value)
	{
		Value = value;
	}
}
