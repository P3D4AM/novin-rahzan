using System;
using System.Windows.Forms;

namespace Stimulsoft.Report;

public class StiSaveChangesInDialogBoxDesignerEvenArgs : EventArgs
{
	public DialogResult DialogResult { get; set; }

	public bool ShowDefaultDialogBox { get; set; } = true;


	public string Message { get; set; } = "";

}
