using System;
using System.ComponentModel;
using System.Drawing.Design;
using System.Windows.Forms;

namespace Stimulsoft.Report.Components.Design;

public class StiFileImageEditor : UITypeEditor
{
	public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
	{
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		if (context != null && context.Instance != null)
		{
			return (UITypeEditorEditStyle)2;
		}
		return ((UITypeEditor)this).GetEditStyle(context);
	}

	public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Invalid comparison between Unknown and I4
		if (context == null || context.Instance == null || provider == null)
		{
			return null;
		}
		OpenFileDialog val = new OpenFileDialog();
		try
		{
			((FileDialog)val).Filter = "All image files (*.bmp,*.jpg,*.jpeg,*.gif,*.tiff,*.png,*.ico,*.wmf,*.emf)|*.bmp;*.jpg;*.jpeg;*.gif;*.tiff,*.png;*.ico;*.wmf;*.emf|Bitmap files (*.bmp,*.jpg,*.jpeg,*.gif,*.tiff,*.png,*.ico)|*.bmp;*.jpg;*.jpeg;*.gif;*.tiff,*.png;*.ico|Meta files (*.wmf,*.emf)|*.wmf;*.emf";
			((FileDialog)val).RestoreDirectory = true;
			if ((int)((CommonDialog)val).ShowDialog() == 1 && ((FileDialog)val).FileName.Length > 0)
			{
				return ((FileDialog)val).FileName;
			}
			return value;
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}
}
