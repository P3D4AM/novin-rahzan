using System;
using System.ComponentModel;
using System.Drawing.Design;
using System.Windows.Forms;

namespace Stimulsoft.Report.Components.Design;

public class StiImageExpressionEditor : StiExpressionEditor
{
	public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
	{
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		if (context != null && context.Instance != null)
		{
			return (UITypeEditorEditStyle)2;
		}
		return base.GetEditStyle(context);
	}

	public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
	{
		//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
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
				return stiImage.TakeGdiImage();
			}
			if (context.PropertyDescriptor.Name == "DataColumn")
			{
				return stiImage.DataColumn;
			}
			if (context.PropertyDescriptor.Name == "ImageData")
			{
				return stiImage.ImageData;
			}
			if (context.PropertyDescriptor.Name == "ImageURL")
			{
				return stiImage.ImageURL;
			}
			return stiImage.File;
		}
		finally
		{
			((IDisposable)stiImageEditorForm)?.Dispose();
		}
	}
}
