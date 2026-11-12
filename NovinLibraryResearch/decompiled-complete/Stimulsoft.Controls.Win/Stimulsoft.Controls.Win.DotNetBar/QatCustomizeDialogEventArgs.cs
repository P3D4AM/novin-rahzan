using System;
using System.Windows.Forms;

namespace Stimulsoft.Controls.Win.DotNetBar;

public class QatCustomizeDialogEventArgs : EventArgs
{
	public bool Cancel;

	public Form Dialog;

	public QatCustomizeDialogEventArgs(Form dialog)
	{
		Dialog = dialog;
	}
}
