using System;

namespace Stimulsoft.Controls.Win.DotNetBar;

public class EndUserCustomizeEventArgs : EventArgs
{
	public readonly eEndUserCustomizeAction Action;

	public object ActionData;

	public EndUserCustomizeEventArgs(eEndUserCustomizeAction action)
	{
		Action = action;
	}

	public EndUserCustomizeEventArgs(eEndUserCustomizeAction action, object actionData)
	{
		Action = action;
		ActionData = actionData;
	}
}
