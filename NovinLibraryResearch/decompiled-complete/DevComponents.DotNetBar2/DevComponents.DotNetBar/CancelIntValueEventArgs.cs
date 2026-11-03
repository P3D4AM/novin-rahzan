using System.ComponentModel;

namespace DevComponents.DotNetBar;

public class CancelIntValueEventArgs : CancelEventArgs
{
	public int NewValue;

	public eEventSource EventSource = eEventSource.Code;
}
