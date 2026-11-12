using System;

namespace Stimulsoft.Base;

public class StiShowDialogCompleteEvetArgs : EventArgs
{
	public bool DialogResult { get; }

	public StiShowDialogCompleteEvetArgs(bool dialogResult)
	{
		DialogResult = dialogResult;
	}
}
