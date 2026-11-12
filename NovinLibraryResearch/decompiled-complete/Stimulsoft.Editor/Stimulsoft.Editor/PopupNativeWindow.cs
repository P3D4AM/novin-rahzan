using System;
using System.Collections;
using System.Windows.Forms;

namespace Stimulsoft.Editor;

internal class PopupNativeWindow : NativeWindow
{
	private ArrayList popups;

	public PopupNativeWindow(IntPtr handle)
	{
		popups = new ArrayList();
		((NativeWindow)this).AssignHandle(handle);
	}

	~PopupNativeWindow()
	{
		try
		{
			popups.Clear();
		}
		finally
		{
			((NativeWindow)this).Finalize();
		}
	}

	protected override void WndProc(ref Message m)
	{
		int msg = ((Message)(ref m)).Msg;
		if (msg == 134 && ((Message)(ref m)).WParam == IntPtr.Zero)
		{
			foreach (ICodeCompletionWindow popup in popups)
			{
				if (popup.Visible)
				{
					((Message)(ref m)).Result = (IntPtr)1;
					return;
				}
			}
		}
		((NativeWindow)this).WndProc(ref m);
	}

	public void Add(ICodeCompletionWindow popup)
	{
		popups.Add(popup);
	}

	public void Remove(ICodeCompletionWindow popup)
	{
		popups.Remove(popup);
	}
}
