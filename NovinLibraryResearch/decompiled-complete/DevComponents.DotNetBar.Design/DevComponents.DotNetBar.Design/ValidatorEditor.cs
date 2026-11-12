using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing.Design;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using DevComponents.DotNetBar.Validator;

namespace DevComponents.DotNetBar.Design;

public class ValidatorEditor : UITypeEditor
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
				val.Items.Add((object)"(none)");
				val.Items.Add((object)"Required Field Validator");
				val.Items.Add((object)"Regular Expression Validator");
				val.Items.Add((object)"Compare Fields Validator");
				val.Items.Add((object)"Range Validator");
				val.Items.Add((object)"Custom Code Validator");
				val.SelectedIndexChanged += ValidatorListSelectedIndexChanged;
				_EditorService.DropDownControl((Control)(object)val);
				int selectedIndex = ((ListControl)val).SelectedIndex;
				if (selectedIndex <= 0)
				{
					return null;
				}
				selectedIndex--;
				Type type = null;
				type = selectedIndex switch
				{
					1 => typeof(RegularExpressionValidator), 
					2 => typeof(CompareValidator), 
					3 => typeof(RangeValidator), 
					4 => typeof(CustomValidator), 
					_ => typeof(RequiredFieldValidator), 
				};
				if (provider.GetService(typeof(IDesignerHost)) is IDesignerHost designerHost)
				{
					ValidatorBase validatorBase = designerHost.CreateComponent(type) as ValidatorBase;
					validatorBase.HighlightColor = eHighlightColor.Red;
					validatorBase.ErrorMessage = "Your error message here.";
					return validatorBase;
				}
			}
			return result;
		}
		return null;
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
