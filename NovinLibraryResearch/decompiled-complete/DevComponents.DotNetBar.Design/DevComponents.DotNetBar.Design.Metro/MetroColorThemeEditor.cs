using System;
using System.ComponentModel;
using System.Drawing.Design;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using DevComponents.DotNetBar.Metro.ColorTables;

namespace DevComponents.DotNetBar.Design.Metro;

public class MetroColorThemeEditor : UITypeEditor
{
	private IWindowsFormsEditorService edSvc;

	public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
	{
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Expected O, but got Unknown
		if (context != null && context.Instance != null && provider != null)
		{
			ref IWindowsFormsEditorService reference = ref edSvc;
			object? service = provider.GetService(typeof(IWindowsFormsEditorService));
			reference = (IWindowsFormsEditorService)((service is IWindowsFormsEditorService) ? service : null);
			if (edSvc != null)
			{
				ListBox val = new ListBox();
				val.SelectedIndexChanged += SelectedChanged;
				MetroColorGeneratorParameters metroColorGeneratorParameters = (MetroColorGeneratorParameters)value;
				MetroColorGeneratorParameters[] allPredefinedThemes = MetroColorGeneratorParameters.GetAllPredefinedThemes();
				string text = null;
				MetroColorGeneratorParameters[] array = allPredefinedThemes;
				for (int i = 0; i < array.Length; i++)
				{
					MetroColorGeneratorParameters metroColorGeneratorParameters2 = array[i];
					val.Items.Add((object)metroColorGeneratorParameters2.ThemeName);
					if (metroColorGeneratorParameters.BaseColor == metroColorGeneratorParameters2.BaseColor && metroColorGeneratorParameters.CanvasColor == metroColorGeneratorParameters2.CanvasColor)
					{
						val.SelectedItem = metroColorGeneratorParameters2.ThemeName;
						text = metroColorGeneratorParameters2.ThemeName;
					}
				}
				edSvc.DropDownControl((Control)(object)val);
				if (val.SelectedItem != null && text != (string)val.SelectedItem)
				{
					return allPredefinedThemes[((ListControl)val).SelectedIndex];
				}
			}
		}
		return value;
	}

	private void SelectedChanged(object sender, EventArgs e)
	{
		if (edSvc != null)
		{
			edSvc.CloseDropDown();
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
