using System;
using System.ComponentModel;
using System.Drawing.Design;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using DevComponents.DotNetBar.Validator;

namespace DevComponents.DotNetBar.Design;

public class ValidationExpressionEditor : UITypeEditor
{
	private IWindowsFormsEditorService _EditorService;

	public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
	{
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Expected O, but got Unknown
		if (context != null && context.Instance != null && provider != null)
		{
			ValidatorBase result = value as ValidatorBase;
			ref IWindowsFormsEditorService editorService = ref _EditorService;
			object? service = provider.GetService(typeof(IWindowsFormsEditorService));
			editorService = (IWindowsFormsEditorService)((service is IWindowsFormsEditorService) ? service : null);
			if (_EditorService != null)
			{
				ListBox val = new ListBox();
				val.Items.Add((object)"At least 5 characters");
				val.Items.Add((object)"Phone Number");
				val.Items.Add((object)"E-mail Address");
				val.Items.Add((object)"ZIP Code");
				val.Items.Add((object)"Social Security Number");
				val.Items.Add((object)"URL");
				val.Items.Add((object)"First, Last Name");
				val.SelectedIndexChanged += ValidatorListSelectedIndexChanged;
				_EditorService.DropDownControl((Control)(object)val);
				if (((ListControl)val).SelectedIndex < 0)
				{
					return result;
				}
				string text = "";
				if (((ListControl)val).SelectedIndex == 1)
				{
					return "^[01]?[- .]?(\\([2-9]\\d{2}\\)|[2-9]\\d{2})[- .]?\\d{3}[- .]?\\d{4}$";
				}
				if (((ListControl)val).SelectedIndex == 2)
				{
					return "^([0-9a-zA-Z]([-\\.\\w]*[0-9a-zA-Z])*@([0-9a-zA-Z][-\\w]*[0-9a-zA-Z]\\.)+[a-zA-Z]{2,9})$";
				}
				if (((ListControl)val).SelectedIndex == 3)
				{
					return "^(\\d{5}-\\d{4}|\\d{5}|\\d{9})$|^([a-zA-Z]\\d[a-zA-Z] \\d[a-zA-Z]\\d)$";
				}
				if (((ListControl)val).SelectedIndex == 4)
				{
					return "^\\d{3}-\\d{2}-\\d{4}$";
				}
				if (((ListControl)val).SelectedIndex == 5)
				{
					return "^(ht|f)tp(s?)\\:\\/\\/[0-9a-zA-Z]([-.\\w]*[0-9a-zA-Z])*(:(0-9)*)*(\\/?)([a-zA-Z0-9\\-\\.\\?\\,\\'\\/\\\\\\+&amp;%\\$#_]*)?$";
				}
				if (((ListControl)val).SelectedIndex == 6)
				{
					return "^[a-zA-Z''-'\\s]{1,40}$";
				}
				return "\\S{5,5}";
			}
			return "";
		}
		return "";
	}

	private void ValidatorListSelectedIndexChanged(object sender, EventArgs e)
	{
		if (_EditorService != null)
		{
			_EditorService.CloseDropDown();
		}
	}

	public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
	{
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		if (context != null && context.Instance != null)
		{
			return (UITypeEditorEditStyle)3;
		}
		return ((UITypeEditor)this).GetEditStyle(context);
	}
}
