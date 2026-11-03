using System.ComponentModel;

namespace DevComponents.DotNetBar.Controls;

public class CancelSourceEventArgs : CancelEventArgs
{
	public eEventSource Source = eEventSource.Code;

	public object Data;

	public CancelSourceEventArgs(eEventSource source)
	{
		Source = source;
	}

	public CancelSourceEventArgs(eEventSource source, object data)
	{
		Source = source;
		Data = data;
	}
}
