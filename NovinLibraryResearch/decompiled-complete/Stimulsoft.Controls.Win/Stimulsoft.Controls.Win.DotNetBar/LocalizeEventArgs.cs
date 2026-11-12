using System;

namespace Stimulsoft.Controls.Win.DotNetBar;

public class LocalizeEventArgs : EventArgs
{
	public bool Handled;

	public string Key = "";

	public string LocalizedValue = "";
}
