using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.Layout;
using Stimulsoft.Controls.Win.DotNetBar;
using Stimulsoft.Controls.Win.DotNetBar.Controls;
using Stimulsoft.Controls.Win.DotNetBar.Metro.ColorTables;

namespace Stimulsoft.Report.Win;

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

	public static eDotNetBarStyle GetBarStyle()
	{
		return (eDotNetBarStyle)7;
	}

	public static eSplitterStyle GetSplitterStyle()
	{
		return (eSplitterStyle)2;
	}

	public static eTabStripStyle GetTabControlStyle()
	{
		return (eTabStripStyle)11;
	}

	public static void ApplyStyle(Control parentControl)
	{
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		InternalApplyStyle(parentControl);
		if (StiOptions.Windows.LastGlobalGuiStyle != StiGlobalGuiStyle.Office2013)
		{
			StyleManager.MetroColorGeneratorParameters = MetroColorGeneratorParameters.DarkBlue;
			StyleManager.Style = (eStyle)9;
			StiOptions.Windows.LastGlobalGuiStyle = StiGlobalGuiStyle.Office2013;
		}
	}

	private static void InternalApplyStyle(Control parentControl)
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_009c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00eb: Expected O, but got Unknown
		ButtonX val = (ButtonX)(object)((parentControl is ButtonX) ? parentControl : null);
		if (val != null)
		{
			((PopupItemControl)val).Style = GetBarStyle();
		}
		ComboBoxEx val2 = (ComboBoxEx)(object)((parentControl is ComboBoxEx) ? parentControl : null);
		if (val2 != null)
		{
			val2.Style = (eDotNetBarStyle)8;
		}
		TabControl val3 = (TabControl)(object)((parentControl is TabControl) ? parentControl : null);
		if (val3 != null)
		{
			val3.Style = GetTabControlStyle();
		}
		PropertyGrid val4 = (PropertyGrid)(object)((parentControl is PropertyGrid) ? parentControl : null);
		if (val4 != null)
		{
			((Control)val4).BackColor = GetPropertyGridBackColor();
			val4.LineColor = Color.FromArgb(255, 240, 240, 240);
			val4.HelpBackColor = GetPropertyGridBackColor();
		}
		CheckBoxX val5 = (CheckBoxX)(object)((parentControl is CheckBoxX) ? parentControl : null);
		if (val5 != null)
		{
			((BaseItemControl)val5).Style = (eDotNetBarStyle)8;
		}
		ExpandableSplitter val6 = (ExpandableSplitter)(object)((parentControl is ExpandableSplitter) ? parentControl : null);
		if (val6 != null)
		{
			val6.Style = GetSplitterStyle();
		}
		ExpandablePanel val7 = (ExpandablePanel)(object)((parentControl is ExpandablePanel) ? parentControl : null);
		if (val7 != null)
		{
			((PanelEx)val7).ColorSchemeStyle = (eDotNetBarStyle)8;
		}
		Bar val8 = (Bar)(object)((parentControl is Bar) ? parentControl : null);
		if (val8 != null)
		{
			val8.Style = (eDotNetBarStyle)8;
		}
		foreach (Control item in (ArrangedElementCollection)parentControl.Controls)
		{
			Control parentControl2 = item;
			ApplyStyle(parentControl2);
		}
	}
}
