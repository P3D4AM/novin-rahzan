using System;
using System.Windows.Forms;

namespace Stimulsoft.Editor;

internal class PopupWnd
{
	private PopupNativeWindow nativeWindow;

	private PopupNativeWindow mdiNativeWindow;

	private ICodeCompletionWindow popup;

	private Control popupParent;

	public ICodeCompletionWindow Popup => popup;

	private PopupNativeWindow GetNativeWindow(IntPtr handle)
	{
		NativeWindow val = NativeWindow.FromHandle(handle);
		if (val is PopupNativeWindow)
		{
			return (PopupNativeWindow)(object)val;
		}
		return new PopupNativeWindow(handle);
	}

	public PopupWnd(ICodeCompletionWindow popup, Control popupParent)
	{
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		this.popup = popup;
		this.popupParent = popupParent;
		if (popupParent != null && popupParent.IsHandleCreated)
		{
			nativeWindow = GetNativeWindow(popupParent.Handle);
			nativeWindow.Add(Popup);
		}
		if (popupParent is Form && ((Form)popupParent).IsMdiChild)
		{
			Form mdiParent = ((Form)popupParent).MdiParent;
			if (mdiParent != null && ((Control)mdiParent).IsHandleCreated)
			{
				mdiNativeWindow = GetNativeWindow(((Control)mdiParent).Handle);
				mdiNativeWindow.Add(Popup);
			}
		}
	}

	public void Release()
	{
		if (nativeWindow != null)
		{
			nativeWindow.Remove(Popup);
		}
		nativeWindow = null;
		if (mdiNativeWindow != null)
		{
			mdiNativeWindow.Remove(Popup);
		}
		mdiNativeWindow = null;
	}
}
