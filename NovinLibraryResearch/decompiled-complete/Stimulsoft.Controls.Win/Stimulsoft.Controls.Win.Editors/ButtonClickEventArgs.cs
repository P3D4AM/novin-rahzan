using System.ComponentModel;

namespace Stimulsoft.Controls.Win.Editors;

public class ButtonClickEventArgs : CancelEventArgs
{
	private object _Button;

	public object Button => _Button;

	public ButtonClickEventArgs(object button)
	{
		_Button = button;
	}
}
