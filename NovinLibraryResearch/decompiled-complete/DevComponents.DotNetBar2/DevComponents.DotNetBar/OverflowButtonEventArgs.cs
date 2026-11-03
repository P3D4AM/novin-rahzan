using System;

namespace DevComponents.DotNetBar;

public class OverflowButtonEventArgs : EventArgs
{
	private readonly ButtonItem ᄑ;

	public ButtonItem OverflowButton => ᄑ;

	public OverflowButtonEventArgs(ButtonItem ob)
	{
		ᄑ = ob;
	}
}
