using System;
using System.ComponentModel;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace DevComponents.DotNetBar;

public static class TaskDialog
{
	private static _2F05 __2F04 = null;

	private static bool _ⳇ = true;

	private static bool _᮰ = true;

	private static MarkupLinkClickEventHandler Ӟ;

	public static bool EnableGlass
	{
		get
		{
			return _ⳇ;
		}
		set
		{
			_ⳇ = value;
		}
	}

	public static bool AntiAlias
	{
		get
		{
			return _᮰;
		}
		set
		{
			_᮰ = value;
		}
	}

	public static event MarkupLinkClickEventHandler MarkupLinkClick
	{
		add
		{
			MarkupLinkClickEventHandler markupLinkClickEventHandler = Ӟ;
			MarkupLinkClickEventHandler markupLinkClickEventHandler2;
			do
			{
				markupLinkClickEventHandler2 = markupLinkClickEventHandler;
				MarkupLinkClickEventHandler value2 = (MarkupLinkClickEventHandler)Delegate.Combine(markupLinkClickEventHandler2, value);
				markupLinkClickEventHandler = Interlocked.CompareExchange(ref Ӟ, value2, markupLinkClickEventHandler2);
			}
			while ((object)markupLinkClickEventHandler != markupLinkClickEventHandler2);
		}
		remove
		{
			MarkupLinkClickEventHandler markupLinkClickEventHandler = Ӟ;
			MarkupLinkClickEventHandler markupLinkClickEventHandler2;
			do
			{
				markupLinkClickEventHandler2 = markupLinkClickEventHandler;
				MarkupLinkClickEventHandler value2 = (MarkupLinkClickEventHandler)Delegate.Remove(markupLinkClickEventHandler2, value);
				markupLinkClickEventHandler = Interlocked.CompareExchange(ref Ӟ, value2, markupLinkClickEventHandler2);
			}
			while ((object)markupLinkClickEventHandler != markupLinkClickEventHandler2);
		}
	}

	public static eTaskDialogResult Show(string dialogTitle, string dialogHeader, string dialogText, eTaskDialogButton dialogButtons)
	{
		TaskDialogInfo info = new TaskDialogInfo(dialogTitle, eTaskDialogIcon.Information, dialogHeader, dialogText, dialogButtons);
		return Show(info);
	}

	public static eTaskDialogResult Show(string dialogTitle, string dialogHeader, string dialogText, eTaskDialogButton dialogButtons, eTaskDialogBackgroundColor dialogColor)
	{
		TaskDialogInfo info = new TaskDialogInfo(dialogTitle, eTaskDialogIcon.Information, dialogHeader, dialogText, dialogButtons, dialogColor);
		return Show(info);
	}

	public static eTaskDialogResult Show(string dialogTitle, eTaskDialogIcon dialogIcon, string dialogHeader, string dialogText, eTaskDialogButton dialogButtons)
	{
		TaskDialogInfo info = new TaskDialogInfo(dialogTitle, dialogIcon, dialogHeader, dialogText, dialogButtons);
		return Show(info);
	}

	public static eTaskDialogResult Show(string dialogTitle, eTaskDialogIcon dialogIcon, string dialogHeader, string dialogText, eTaskDialogButton dialogButtons, eTaskDialogBackgroundColor dialogColor)
	{
		TaskDialogInfo info = new TaskDialogInfo(dialogTitle, dialogIcon, dialogHeader, dialogText, dialogButtons, dialogColor);
		return Show(info);
	}

	public static eTaskDialogResult Show(TaskDialogInfo info)
	{
		return Show(null, info);
	}

	public static eTaskDialogResult Show(IWin32Window owner, TaskDialogInfo info)
	{
		eTaskDialogResult eTaskDialogResult2 = eTaskDialogResult.None;
		_2F05 _2F052 = new _2F05();
		try
		{
			__2F04 = _2F052;
			if (!_᮰)
			{
				_2F052.Ⲭ = _᮰;
			}
			_2F052.EnableGlass = _ⳇ && !StyleManager.IsMetro(StyleManager.Style);
			((Form)_2F052).TopMost = info.TopMost;
			if (!info.FormCloseEnabled)
			{
				_2F052.CloseEnabled = false;
			}
			_2F052._2F1C(owner, info);
			return _2F052._2F07;
		}
		finally
		{
			((Component)(object)_2F052).Dispose();
			__2F04 = null;
		}
	}

	public static void Close()
	{
		Close(eTaskDialogResult.None);
	}

	public static void Close(eTaskDialogResult result)
	{
		if (__2F04 == null)
		{
			throw new NullReferenceException("Task Dialog Form is not shown.");
		}
		__2F04._2F26(result);
	}

	internal static Image _0EC5(eTaskDialogIcon ᆠ)
	{
		if (ᆠ == eTaskDialogIcon.None)
		{
			return null;
		}
		return (Image)(object)BarFunctions.ই("SystemImages.Task" + ᆠ.ToString() + ".png");
	}

	internal static void ۃ(object _٠, MarkupLinkClickEventArgs ؿ)
	{
		Ӟ?.Invoke(_٠, ؿ);
	}
}
