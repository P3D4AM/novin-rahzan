using System;
using System.Windows.Forms;

namespace Stimulsoft.Editor;

public class KeyPreviewEventArgs : EventArgs
{
	public Message Message;

	public bool Handled;

	public KeyPreviewEventArgs(Message m)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		Message = m;
	}
}
