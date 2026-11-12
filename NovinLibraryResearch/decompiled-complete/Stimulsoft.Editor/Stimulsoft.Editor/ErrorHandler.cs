using System;
using System.Windows.Forms;

namespace Stimulsoft.Editor;

public sealed class ErrorHandler
{
	private static ErrorBehavior errorBehavior;

	public static ErrorBehavior ErrorBehavior
	{
		get
		{
			return errorBehavior;
		}
		set
		{
			errorBehavior = value;
		}
	}

	private ErrorHandler()
	{
	}

	public static void Error(Exception exception)
	{
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		if (exception != null)
		{
			switch (errorBehavior)
			{
			case ErrorBehavior.Message:
				MessageBox.Show(exception.Message, StringConsts.ErrorCaption, (MessageBoxButtons)0, (MessageBoxIcon)16);
				break;
			case ErrorBehavior.Exception:
				throw exception;
			}
		}
	}
}
