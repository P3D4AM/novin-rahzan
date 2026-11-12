using System;
using System.Windows.Forms;

namespace Stimulsoft.Base;

public static class StiExceptionProvider
{
	public static IStiCustomExceptionProvider CustomExceptionProvider { get; set; }

	public static bool HideMessages { get; set; }

	public static bool DisableSendError { get; set; }

	public static string ServerUrl { get; set; }

	public static bool HideExceptions { get; set; }

	public static bool Show(Exception exception, bool rethrow = false, Form owner = null)
	{
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Invalid comparison between Unknown and I4
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		if (CustomExceptionProvider != null)
		{
			CustomExceptionProvider.Show(exception);
		}
		else if (!HideMessages)
		{
			if (owner == null)
			{
				owner = Form.ActiveForm;
			}
			StiExceptionPreviewForm stiExceptionPreviewForm = new StiExceptionPreviewForm(exception);
			try
			{
				if (owner != null)
				{
					((Form)stiExceptionPreviewForm).Owner = owner;
				}
				else
				{
					((Form)stiExceptionPreviewForm).StartPosition = (FormStartPosition)1;
				}
				if ((int)((Form)stiExceptionPreviewForm).ShowDialog() == 1)
				{
					StiExceptionForm stiExceptionForm = new StiExceptionForm(exception);
					try
					{
						if (owner != null)
						{
							((Form)stiExceptionForm).Owner = owner;
						}
						else
						{
							((Form)stiExceptionForm).StartPosition = (FormStartPosition)1;
						}
						((Form)stiExceptionForm).ShowDialog();
					}
					finally
					{
						((IDisposable)stiExceptionForm)?.Dispose();
					}
				}
			}
			finally
			{
				((IDisposable)stiExceptionPreviewForm)?.Dispose();
			}
		}
		else
		{
			if (rethrow)
			{
				return !HideExceptions;
			}
			if (!HideExceptions)
			{
				throw exception;
			}
		}
		return false;
	}
}
