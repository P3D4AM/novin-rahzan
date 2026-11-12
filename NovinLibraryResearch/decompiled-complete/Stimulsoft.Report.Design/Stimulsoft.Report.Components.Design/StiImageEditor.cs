using System;
using System.ComponentModel;
using System.Drawing.Design;
using System.Windows.Forms;

namespace Stimulsoft.Report.Components.Design;

public class StiImageEditor : UITypeEditor
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
		//IL_00f7: Unknown result type (might be due to invalid IL or missing references)
		Array array = context.Instance as Array;
		StiImage stiImage = context.Instance as StiImage;
		StiImageEditorForm.StartTab startTab = StiImageEditorForm.StartTab.None;
		if (context.PropertyDescriptor.Name == "Image")
		{
			startTab = StiImageEditorForm.StartTab.Image;
		}
		else if (context.PropertyDescriptor.Name == "DataColumn")
		{
			startTab = StiImageEditorForm.StartTab.DataColumn;
		}
		else if (context.PropertyDescriptor.Name == "ImageData")
		{
			startTab = StiImageEditorForm.StartTab.ImageData;
		}
		else if (context.PropertyDescriptor.Name == "ImageURL")
		{
			startTab = StiImageEditorForm.StartTab.ImageURL;
		}
		else if (context.PropertyDescriptor.Name == "File")
		{
			startTab = StiImageEditorForm.StartTab.File;
		}
		if (array != null && array.GetLength(0) > 0)
		{
			stiImage = array.GetValue(0) as StiImage;
		}
		StiImageEditorForm stiImageEditorForm = new StiImageEditorForm(stiImage, startTab);
		try
		{
			if (stiImage.Report.Designer != null && stiImage.Report.Designer.DesignerControl != null)
			{
				stiImage.Report.Designer.DesignerControl.AddOwnedForm((Form)(object)stiImageEditorForm);
			}
			((Form)stiImageEditorForm).ShowDialog();
			if (context.PropertyDescriptor.Name == "Image")
			{
				if (array != null)
				{
					for (int i = 0; i < array.GetLength(0); i++)
					{
						StiImage stiImage2 = array.GetValue(i) as StiImage;
						stiImage2.PutImage(stiImage.TakeImage());
					}
				}
				return StiNullImage.Null;
			}
			if (context.PropertyDescriptor.Name == "DataColumn")
			{
				if (array != null)
				{
					for (int j = 0; j < array.GetLength(0); j++)
					{
						StiImage stiImage3 = array.GetValue(j) as StiImage;
						stiImage3.DataColumn = stiImage.DataColumn.Clone() as string;
					}
				}
				return stiImage.DataColumn;
			}
			if (context.PropertyDescriptor.Name == "ImageData")
			{
				if (array != null)
				{
					for (int k = 0; k < array.GetLength(0); k++)
					{
						StiImage stiImage4 = array.GetValue(k) as StiImage;
						stiImage4.ImageData.Value = stiImage.ImageData.Value.Clone() as string;
					}
				}
				return stiImage.ImageData;
			}
			if (context.PropertyDescriptor.Name == "ImageURL")
			{
				if (array != null)
				{
					for (int l = 0; l < array.GetLength(0); l++)
					{
						StiImage stiImage5 = array.GetValue(l) as StiImage;
						stiImage5.ImageURL.Value = stiImage.ImageURL.Value.Clone() as string;
					}
				}
				return stiImage.ImageURL;
			}
			if (array != null)
			{
				for (int m = 0; m < array.GetLength(0); m++)
				{
					StiImage stiImage6 = array.GetValue(m) as StiImage;
					stiImage6.File = stiImage.File.Clone() as string;
				}
			}
			return stiImage.File;
		}
		finally
		{
			((IDisposable)stiImageEditorForm)?.Dispose();
		}
	}
}
