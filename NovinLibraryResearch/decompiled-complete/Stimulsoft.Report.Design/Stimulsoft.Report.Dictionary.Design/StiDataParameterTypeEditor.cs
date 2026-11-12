using System;
using System.ComponentModel;
using System.Drawing.Design;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace Stimulsoft.Report.Dictionary.Design;

public class StiDataParameterTypeEditor : UITypeEditor
{
	private IWindowsFormsEditorService edSvc;

	public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
	{
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		if (context != null && context.Instance != null)
		{
			return (UITypeEditorEditStyle)3;
		}
		return ((UITypeEditor)this).GetEditStyle(context);
	}

	internal void OnDoubleClick(object sender, EventArgs e)
	{
		edSvc.CloseDropDown();
	}

	public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Expected O, but got Unknown
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0083: Expected O, but got Unknown
		edSvc = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
		if (!(((StiDataParameter)context.Instance).DataSource is StiSqlSource stiSqlSource))
		{
			return value;
		}
		Type parameterTypesEnum = stiSqlSource.GetParameterTypesEnum();
		Array values = Enum.GetValues(parameterTypesEnum);
		object[] array = new object[values.Length];
		values.CopyTo(array, 0);
		ListBox val = new ListBox
		{
			Height = 200,
			Width = 150,
			Sorted = true,
			BorderStyle = (BorderStyle)0
		};
		val.Items.AddRange(array);
		int num = 0;
		foreach (object item in val.Items)
		{
			if ((int)item == (int)value)
			{
				((ListControl)val).SelectedIndex = num;
				break;
			}
			num++;
		}
		((Control)val).DoubleClick += OnDoubleClick;
		val.Click += OnDoubleClick;
		edSvc.DropDownControl((Control)(object)val);
		return (int)val.SelectedItem;
	}
}
