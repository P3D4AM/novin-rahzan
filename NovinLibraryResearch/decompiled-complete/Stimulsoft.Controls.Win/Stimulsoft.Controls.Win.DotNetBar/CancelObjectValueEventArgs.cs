using System.ComponentModel;

namespace Stimulsoft.Controls.Win.DotNetBar;

public class CancelObjectValueEventArgs : CancelEventArgs
{
	public object Data;

	public CancelObjectValueEventArgs(object o)
	{
		Data = o;
	}
}
