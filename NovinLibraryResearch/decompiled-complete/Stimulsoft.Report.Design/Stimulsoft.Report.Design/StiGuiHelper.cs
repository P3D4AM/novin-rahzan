using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.Layout;
using Stimulsoft.Controls.Win.DotNetBar;
using Stimulsoft.Controls.Win.DotNetBar.Controls;
using Stimulsoft.Controls.Win.DotNetBar.Metro.ColorTables;

namespace Stimulsoft.Report.Design;

public class StiGuiHelper
{
	public static Color GetColorRulerBackground()
	{
		return Color.FromArgb(255, 254, 254, 254);
	}

	public static Color GetColorRulerLightBackground()
	{
		return Color.FromArgb(255, 255, 255, 255);
	}

	public static Color GetColorRulerDarkBackground()
	{
		return Color.FromArgb(255, 230, 230, 230);
	}

	public static Color GetColorRulerLightBorder()
	{
		return Color.FromArgb(255, 226, 226, 226);
	}

	public static Color GetColorRulerDarkBorder()
	{
		return Color.FromArgb(255, 226, 226, 226);
	}

	public static Color GetColorRulerSelectionBackground()
	{
		return Color.FromArgb(255, 194, 213, 242);
	}

	public static Color GetColorPageViewBackground()
	{
		return GetWorkspaceColor();
	}

	public static Color GetLabelBackgroundColor()
	{
		return Color.FromArgb(255, 221, 231, 238);
	}

	public static Color GetLabelForegroundColor()
	{
		return Color.FromArgb(255, 0, 21, 110);
	}

	public static Color GetWorkspaceColor()
	{
		return Color.FromArgb(255, 220, 226, 232);
	}

	public static Color GetControlColor()
	{
		return Color.White;
	}

	public static Color GetPropertyGridBackColor()
	{
		return Color.White;
	}

	public static Color GetPropertyGridHelpBackColor()
	{
		return Color.FromArgb(255, 194, 213, 242);
	}

	public static eDotNetBarStyle GetButtonStyle()
	{
		return eDotNetBarStyle.Metro;
	}

	public static eSplitterStyle GetSplitterStyle()
	{
		return eSplitterStyle.Office2007;
	}

	public static eTabStripStyle GetTabControlStyle()
	{
		return eTabStripStyle.Metro;
	}

	public static void ApplyStyle(Control parentControl)
	{
		InternalApplyStyle(parentControl);
		if (StiOptions.Windows.LastGlobalGuiStyle != StiGlobalGuiStyle.Office2013)
		{
			StiOptions.Windows.LastGlobalGuiStyle = StiGlobalGuiStyle.Office2013;
		}
		if (StyleManager.Style != eStyle.Metro)
		{
			StyleManager.Style = eStyle.Metro;
		}
		if (!StyleManager.MetroColorGeneratorParameters.Equals(MetroColorGeneratorParameters.DarkBlue))
		{
			StyleManager.MetroColorGeneratorParameters = MetroColorGeneratorParameters.DarkBlue;
		}
	}

	private static void InternalApplyStyle(Control parentControl)
	{
		//IL_00e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00eb: Expected O, but got Unknown
		if (parentControl is ButtonX buttonX)
		{
			buttonX.Style = GetButtonStyle();
		}
		if (parentControl is ComboBoxEx comboBoxEx)
		{
			comboBoxEx.Style = eDotNetBarStyle.StyleManagerControlled;
		}
		if (parentControl is TabControl tabControl)
		{
			tabControl.Style = GetTabControlStyle();
		}
		PropertyGrid val = (PropertyGrid)(object)((parentControl is PropertyGrid) ? parentControl : null);
		if (val != null)
		{
			((Control)val).BackColor = GetPropertyGridBackColor();
			val.LineColor = Color.FromArgb(255, 240, 240, 240);
			val.HelpBackColor = GetPropertyGridBackColor();
		}
		if (parentControl is CheckBoxX checkBoxX)
		{
			checkBoxX.Style = eDotNetBarStyle.StyleManagerControlled;
		}
		if (parentControl is ExpandableSplitter expandableSplitter)
		{
			expandableSplitter.Style = GetSplitterStyle();
		}
		if (parentControl is ExpandablePanel expandablePanel)
		{
			expandablePanel.ColorSchemeStyle = eDotNetBarStyle.StyleManagerControlled;
		}
		if (parentControl is Bar bar)
		{
			bar.Style = eDotNetBarStyle.StyleManagerControlled;
		}
		foreach (Control item in (ArrangedElementCollection)parentControl.Controls)
		{
			Control parentControl2 = item;
			ApplyStyle(parentControl2);
		}
	}
}
