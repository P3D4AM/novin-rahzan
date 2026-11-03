using System;
using Stimulsoft.Report.Dictionary;

namespace Stimulsoft.Report.Events;

public class StiNewRequestFromUserDialogCreatingEventArgs : EventArgs
{
	public StiVariable Variable { get; }

	public int Width { get; set; }

	public StiNewRequestFromUserDialogCreatingEventArgs(StiVariable variable, int width)
	{
		Variable = variable;
		Width = width;
	}

	public StiNewRequestFromUserDialogCreatingEventArgs(StiVariable variable, double width)
	{
		Variable = variable;
		Width = (int)width;
	}
}
