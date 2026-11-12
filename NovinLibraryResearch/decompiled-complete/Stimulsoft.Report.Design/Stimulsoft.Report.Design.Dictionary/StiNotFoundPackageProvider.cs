using System;
using System.Windows.Forms;
using Stimulsoft.Base.Exceptions;

namespace Stimulsoft.Report.Design.Dictionary;

public class StiNotFoundPackageProvider : IStiNotFoundProvider
{
	public void Show(string namePackage)
	{
		Show(namePackage, null);
	}

	public void Show(string namePackage, Form owner)
	{
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (owner == null)
			{
				owner = Form.ActiveForm;
			}
			StiNotFoundPackageForm stiNotFoundPackageForm = new StiNotFoundPackageForm(namePackage);
			try
			{
				if (owner != null)
				{
					((Form)stiNotFoundPackageForm).Owner = owner;
				}
				else
				{
					((Form)stiNotFoundPackageForm).StartPosition = (FormStartPosition)1;
				}
				((Form)stiNotFoundPackageForm).ShowDialog();
			}
			finally
			{
				((IDisposable)stiNotFoundPackageForm)?.Dispose();
			}
		}
		catch
		{
		}
	}
}
