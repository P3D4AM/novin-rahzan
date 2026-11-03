using System;
using System.Windows.Forms;

namespace Stimulsoft.Report.Viewer;

public class StiTextEditorEventArgs : EventArgs
{
	public Control TextBox { get; }

	public StiTextEditorEventArgs(Control textBox)
	{
		TextBox = textBox;
	}
}
