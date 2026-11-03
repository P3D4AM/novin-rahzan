using System.ComponentModel;

namespace DevComponents.Editors;

public class ButtonClickEventArgs : CancelEventArgs
{
	private object __232A;

	public object Button => __232A;

	public ButtonClickEventArgs(object button)
	{
		__232A = button;
	}
}
