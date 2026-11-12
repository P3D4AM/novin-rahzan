using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using DevComponents.Schedule;

namespace DevComponents.DotNetBar.Design;

public class TimeZoneSelectionEditor : UITypeEditor
{
	private IWindowsFormsEditorService _EditorService;

	public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
	{
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Expected O, but got Unknown
		if (context != null && context.Instance != null && provider != null)
		{
			ref IWindowsFormsEditorService editorService = ref _EditorService;
			object? service = provider.GetService(typeof(IWindowsFormsEditorService));
			editorService = (IWindowsFormsEditorService)((service is IWindowsFormsEditorService) ? service : null);
			if (_EditorService != null)
			{
				ListBox val = new ListBox();
				List<DevComponents.Schedule.TimeZoneInfo> systemTimeZones = DevComponents.Schedule.TimeZoneInfo.GetSystemTimeZones();
				int selectedIndex = -1;
				for (int i = 0; i < systemTimeZones.Count; i++)
				{
					DevComponents.Schedule.TimeZoneInfo timeZoneInfo = systemTimeZones[i];
					val.Items.Add((object)timeZoneInfo.Id);
					if (timeZoneInfo.Id.Equals((string)value))
					{
						selectedIndex = i;
					}
				}
				((ListControl)val).SelectedIndex = selectedIndex;
				val.SelectedIndexChanged += SelectedChanged;
				_EditorService.DropDownControl((Control)(object)val);
				return val.SelectedItem;
			}
		}
		return value;
	}

	private void SelectedChanged(object sender, EventArgs e)
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
