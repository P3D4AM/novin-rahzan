using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Stimulsoft.Controls.Win.DotNetBar;

public static class TaskDialog
{
	private static TaskDialogForm _TaskDialogForm = null;

	private static bool _EnableGlass = true;

	private static bool _AntiAlias = true;

	public static bool EnableGlass
	{
		get
		{
			return _EnableGlass;
		}
		set
		{
			_EnableGlass = value;
		}
	}

	public static bool AntiAlias
	{
		get
		{
			return _AntiAlias;
		}
		set
		{
			_AntiAlias = value;
		}
	}

	public static event MarkupLinkClickEventHandler MarkupLinkClick;

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
		TaskDialogForm taskDialogForm = new TaskDialogForm();
		try
		{
			_TaskDialogForm = taskDialogForm;
			if (!_AntiAlias)
			{
				taskDialogForm.AntiAlias = _AntiAlias;
			}
			taskDialogForm.EnableGlass = _EnableGlass && StyleManager.Style != eStyle.Metro;
			((Form)taskDialogForm).TopMost = info.TopMost;
			taskDialogForm.ShowTaskDialog(owner, info);
			return taskDialogForm.Result;
		}
		finally
		{
			((Component)(object)taskDialogForm).Dispose();
			_TaskDialogForm = null;
		}
	}

	public static void Close()
	{
		Close(eTaskDialogResult.None);
	}

	public static void Close(eTaskDialogResult result)
	{
		if (_TaskDialogForm == null)
		{
			throw new NullReferenceException("Task Dialog Form is not shown.");
		}
		_TaskDialogForm.CloseDialog(result);
	}

	internal static Image GetImage(eTaskDialogIcon icon)
	{
		if (icon == eTaskDialogIcon.None)
		{
			return null;
		}
		return (Image)(object)BarFunctions.LoadBitmap("SystemImages.Task" + icon.ToString() + ".png");
	}

	internal static void InvokeMarkupLinkClick(object sender, MarkupLinkClickEventArgs e)
	{
		TaskDialog.MarkupLinkClick?.Invoke(sender, e);
	}
}
