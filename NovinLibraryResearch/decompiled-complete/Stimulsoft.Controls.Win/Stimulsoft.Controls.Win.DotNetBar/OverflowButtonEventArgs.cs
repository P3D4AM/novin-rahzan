using System;

namespace Stimulsoft.Controls.Win.DotNetBar;

public class OverflowButtonEventArgs : EventArgs
{
	private readonly ButtonItem m_OverflowButton;

	public ButtonItem OverflowButton => m_OverflowButton;

	public OverflowButtonEventArgs(ButtonItem ob)
	{
		m_OverflowButton = ob;
	}
}
