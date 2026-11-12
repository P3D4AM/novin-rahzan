using System.ComponentModel;

namespace Stimulsoft.Controls.Win.DotNetBar;

public class CancelIntValueEventArgs : CancelEventArgs
{
	public int NewValue;

	public eEventSource EventSource = eEventSource.Code;
}
